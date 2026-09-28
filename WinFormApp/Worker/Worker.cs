using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormApp
{
    public class ReferenceList : HashSet<PortableExecutableReference> { }

    public class DocInfo
    {
        public string full { get; set; }
        public string code { get; set; }

        public DocInfo(string full, string code)
        {
            this.full = full;
            this.code = code;
        }

        public override string ToString()
        {
            return $"{full}\n{code}";
        }
    }

    public class SignatureHelpResult
    {
        public List<string> Signatures = new List<string>();
    }

    public class Worker
    {
        public bool FindCC = false;

        public static string TargetOfInvocation = "Exception has been thrown by the target of an invocation";

        public ReferenceList References { get; } = new ReferenceList();

        private readonly MefHostServices _host;

        private static readonly ImmutableArray<Type> _defaultTypes = new[] {
            typeof(object),
            typeof(Thread),
            typeof(Task),
            typeof(List<>),
            typeof(Regex),
            typeof(StringBuilder),
            typeof(Uri),
            typeof(Enumerable),
            typeof(IEnumerable),
            typeof(Path),
            typeof(Assembly),
            typeof(Microsoft.CSharp.RuntimeBinder.Binder)
        }.ToImmutableArray();

        private static readonly CSharpCompilationOptions _options =
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(false)
                .WithOptimizationLevel(OptimizationLevel.Release);

        private AdhocWorkspace workspace = null;
        private Project project = null;
        private SyntaxTree[] trees = null;
        private Compilation compilation = null;
        private Assembly assembly = null;
        private readonly ConsoleOutput _consoleOutput = new ConsoleOutput();

        public Action<string> WriteInfo = (s) => { };

        public bool DoCoverage = false;
        public HashSet<MarkInfo> CCList = new HashSet<MarkInfo>();

        public Worker()
        {
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);

            _host = MefHostServices.Create(MefHostServices.DefaultAssemblies);

            workspace = new AdhocWorkspace(_host);

            AddNetFrameworkDefaultReferences();

            AddThirdPartyRefs();

            MakeAndAddProject();
        }


        public bool AddAssembly(string assemblyDll)
        {
            if (string.IsNullOrEmpty(assemblyDll)) return false;

            var file = Path.GetFullPath(assemblyDll);

            if (!File.Exists(file))
            {
                var path = Path.GetDirectoryName(typeof(object).Assembly.Location);
                file = Path.Combine(path, assemblyDll);
                if (!File.Exists(file))
                    return false;
            }

            if (References.Any(r => r.FilePath == file)) return true;

            try
            {
                var reference = MetadataReference.CreateFromFile(file);
                References.Add(reference);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool AddAssembly(Type type)
        {
            try
            {
                if (References.Any(r => r.FilePath == type.Assembly.Location))
                    return true;

                var systemReference = MetadataReference.CreateFromFile(type.Assembly.Location);
                References.Add(systemReference);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void AddNetFrameworkDefaultReferences()
	{
  	  AddAssembly(typeof(object));
  	  AddAssembly(typeof(Console));
 	  AddAssembly(typeof(Enumerable));
	  AddAssembly(typeof(Thread));
  	  AddAssembly(typeof(Task));
  	  AddAssembly(typeof(Microsoft.CSharp.RuntimeBinder.Binder));
  	  AddAssembly("System.Runtime.dll");
	}

        public void AddThirdPartyRefs()
        {
            foreach (var full in RefsForm.GetRefsFilesList())
                AddAssembly(full);
        }

        private void MakeAndAddProject()
        {
            var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(),
                 VersionStamp.Create(), "RapidProject", "RapidProject", LanguageNames.CSharp).
                 WithMetadataReferences(References);

            project = workspace.AddProject(projectInfo);
        }

        public void UpdateDocuments(List<DocInfo> list)
        {
            workspace.ClearSolution();
            MakeAndAddProject();

            var solution = workspace.CurrentSolution;

            foreach (var item in list)
                AddDocument(item, ref solution);

            project = solution.GetProject(project.Id);
        }

        private Document FindDocByName(string full)
        {
            return project.Documents.FirstOrDefault(d => d.Name.Equals(full));
        }

        public async Task<List<Tuple<string,string>>> ReadCompletionItems(string docname, string word, int position)
        {
            Document doc = FindDocByName(docname);
            if (doc == null) return new List<Tuple<string, string>>();

            try
            {
                var completionService = CompletionService.GetService(doc);
                if (completionService == null) return new List<Tuple<string, string>>();

                // ConfigureAwait(false) da ne blokira UI nit
                var results = await completionService.GetCompletionsAsync(doc, position).ConfigureAwait(false);

                var list = new List<Tuple<string, string>>();
                if (results == null)
                    return list;

                foreach (var item in results.ItemsList)
                {
                    try
                    {
                        var desc = await completionService.GetDescriptionAsync(doc, item).ConfigureAwait(false);
                        list.Add(new Tuple<string,string>( item.DisplayText, desc.Text ?? item.DisplayText));
                    }
                    catch
                    {
                        list.Add(new Tuple<string,string>( item.DisplayText, item.DisplayText));
                    }
                }
                return list;
            }
            catch
            {
                return new List<Tuple<string, string>>();
            }
        }

        // backward compat - stari poziv
        public async Task<List<Tuple<string,string>>> ReadCompletionItems(string docname, string word)
        {
            return await ReadCompletionItems(docname, word, 0).ConfigureAwait(false);
        }

        // Signature Help (parametri metode dok kucaš unutar zagrada).
        // Roslyn-ov ugrađeni SignatureHelpService je interni API, pa ovde
        // pravimo sopstvenu, jednostavnu verziju preko javnog SemanticModel-a.
        public async Task<SignatureHelpResult> GetSignatureHelp(string docname, int position)
        {
            try
            {
                Document doc = FindDocByName(docname);
                if (doc == null) return null;

                var semanticModel = await doc.GetSemanticModelAsync().ConfigureAwait(false);
                var root = await doc.GetSyntaxRootAsync().ConfigureAwait(false);
                if (semanticModel == null || root == null) return null;
                if (position < 0 || position > root.FullSpan.Length) return null;

                var token = root.FindToken(position);
                ArgumentListSyntax argList = null;

                for (var n = token.Parent; n != null; n = n.Parent)
                {
                    if (n is ArgumentListSyntax al &&
                        al.OpenParenToken.Span.End <= position &&
                        position <= al.CloseParenToken.Span.Start)
                    {
                        argList = al;
                        break;
                    }
                    if (n is StatementSyntax) break; // ne izlazi izvan trenutne naredbe
                }

                if (argList == null || !(argList.Parent is InvocationExpressionSyntax invocation))
                    return null;

                var candidates = semanticModel.GetMemberGroup(invocation.Expression)
                    .OfType<IMethodSymbol>().ToList();

                if (candidates.Count == 0)
                {
                    var symInfo = semanticModel.GetSymbolInfo(invocation);
                    if (symInfo.Symbol is IMethodSymbol single)
                        candidates.Add(single);
                    else
                        candidates.AddRange(symInfo.CandidateSymbols.OfType<IMethodSymbol>());
                }

                if (candidates.Count == 0) return null;

                int activeParam = 0;
                for (int i = 0; i < argList.Arguments.SeparatorCount; i++)
                {
                    if (argList.Arguments.GetSeparator(i).Span.Start < position)
                        activeParam++;
                    else
                        break;
                }

                var result = new SignatureHelpResult();
                foreach (var m in candidates.Distinct(SymbolEqualityComparer.Default).OfType<IMethodSymbol>())
                    result.Signatures.Add(FormatSignature(m, activeParam));

                return result;
            }
            catch
            {
                return null;
            }
        }

        private static string FormatSignature(IMethodSymbol m, int activeParamIndex)
        {
            var parts = new List<string>();
            for (int i = 0; i < m.Parameters.Length; i++)
            {
                var p = m.Parameters[i];
                var text = $"{p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {p.Name}";
                if (p.HasExplicitDefaultValue)
                    text += $" = {(p.ExplicitDefaultValue ?? "null")}";
                if (i == activeParamIndex)
                    text = "▶" + text;
                parts.Add(text);
            }
            return $"{m.Name}({string.Join(", ", parts)})";
        }        

        private Document MakeDocument(DocInfo docInfo)
        {
            var solution = workspace.CurrentSolution;
            var doc = FindDocByName(docInfo.full);

            if (doc == null)
                doc = AddDocument(docInfo, ref solution);

            project = solution.GetProject(project.Id);
            return doc;
        }

        private Document AddDocument(DocInfo docInfo, ref Solution solution)
        {
            Document doc;
            var source = SourceText.From(docInfo.code);
            var documentId = DocumentId.CreateNewId(project.Id);
            solution = solution.AddDocument(documentId, docInfo.full, source);
            doc = solution.GetDocument(documentId);
            return doc;
        }

        public void Build(List<DocInfo> docs)
        {
            WriteInfo("Start build...");

            UpdateDocuments(docs);

            compilation = Task.Run(() => GetCompilations(project.Documents.ToArray())).Result;

            assembly = GetAssembly(compilation);
        }

        private async Task<Compilation> GetCompilations(params Document[] documents)
        {
            WriteInfo("Compile...");

            var syntaxTrees = documents.Select(async (d) => await d.GetSyntaxTreeAsync());

            trees = await Task.WhenAll(syntaxTrees);

            string asmName = Path.GetRandomFileName();

            return CSharpCompilation.Create(asmName, trees, References, _options);
        }

        private Assembly GetAssembly(Compilation compilation)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                WriteInfo("Emit...");
                var emitResult = compilation.Emit(ms);
                WriteInfo("Emit done...");

                if (emitResult.Success)
                {
                    WriteInfo("Success!");
                    ms.Seek(0, SeekOrigin.Begin);
                    var buffer = ms.GetBuffer();
                    var assembly = Assembly.Load(buffer);

                    return assembly;
                }
                else
                {
                    WriteInfo("Errors in code...");

                    var failures = emitResult.Diagnostics
                        .Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    var errors = new List<Jump>();
                    foreach (Diagnostic fail in failures)
                    {
                        var filePath = "";
                        var loc = fail.Location;
                        if (loc.SourceTree != null)
                            filePath = loc.SourceTree.FilePath ?? "";                            

                        errors.Add(new Jump(
                            filePath, loc.SourceSpan.Start,
                            $"{fail.Id} {fail.GetMessage()}"));
                    }

                    var excp = new Exception("FAIL : (errors in code)\r\n");
                    excp.Data.Add("ErrorsInCode", errors);
                    throw excp;
                }
            }
        }

        private string RunMainMethod(string[] args)
        {
            _consoleOutput.Clear();

            if (assembly != null)
            {
                MainClassInfo main = Discoverer.FindStaticEntryMethod(assembly);

                object program = Activator.CreateInstance(main.MainClass);
                main.MainMethod.Invoke(program, new object[] { args });
            }

            if (DoCoverage)
            {                
                CCList = new HashSet<MarkInfo>();
                
                var dict = Discoverer.FindCCSpanList(assembly);
                if (dict != null)
                {
                    foreach (var l in dict)                                            
                        foreach (var v in l.Value)
                            CCList.Add(new MarkInfo(l.Key, v));                    
                }
            }

            return _consoleOutput.GetOutput();
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public string RunCodeThread(string[] args)
        {
            WriteInfo("Execute...");
            var vaittime = 15;
            var task = Task.Run(() =>
            {
                try
                {
                    return RunMainMethod(args);
                }
                catch (Exception e)
                {
                    var result = "FAIL: \n" + e.Message;

                    if (e.Message.Contains(TargetOfInvocation))
                       result += "\n\n" + e.InnerException.Message;

                    return result;
                }
            });

            bool onTime = task.Wait(TimeSpan.FromSeconds(vaittime));

            if (onTime)
                return (string)task.Result;
            else
                throw new TimeoutException(
                    $"The function lasted longer " +
                    $"than the maximum allowed time. [{vaittime} sec]");
        }

        static SyntaxNode GetNode(SyntaxTree tree, int lineNumber)
        {
            var lineSpan = tree.GetText().Lines[lineNumber - 1].Span;
            return tree.GetRoot().DescendantNodes(lineSpan)
                .First(n => lineSpan.Contains(n.Span));
        }

        public async Task<List<Jump>> FindSymbolDefinition(
                DocInfo docInfo, int position, int tag)
        {
            Document doc = MakeDocument(docInfo);
            var symbol = await SymbolFinder.FindSymbolAtPositionAsync(doc, position);
            if (symbol == null)
                return null;

            var result = new List<Jump>();            
            
            var syntaxReference = symbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (syntaxReference == null)
                return result;

            var declaration = syntaxReference.GetSyntax();
            var location = declaration.GetLocation();

            if (tag == 1)
            {
                result.Add(makeJump(location, ""));
            }
            else
            {
                var solution = doc.Project.Solution;
                var callers = await SymbolFinder.FindReferencesAsync(symbol, solution);

                foreach (var referenced in callers)
                {
                    foreach (var loc in referenced.Locations)
                    {
                        var text = "";
                        var tree = loc.Location.SourceTree;
                        if (tree != null)
                        {
                            var fullText = tree.GetText();
                            var linePos = loc.Location.GetLineSpan().StartLinePosition.Line;
                            text = fullText.Lines[linePos].ToString();                            
                        }                        

                        result.Add(makeJump(loc.Location, text));
                    }
                }
            }

            return result;
        }

        private static Jump makeJump(Location location, string text)
        {
            return new Jump(location.SourceTree.FilePath,
                location.SourceSpan.Start, text);
        }

        public async Task<List<DocInfo>> RenameSymbol(DocInfo docInfo,
            int position, string newName)
        {
            Document doc = MakeDocument(docInfo);
            var symbol = await SymbolFinder.FindSymbolAtPositionAsync(doc, position);
            if (symbol == null)
                return null;

            var result = new List<DocInfo>();
            var solution = doc.Project.Solution;

            // Roslyn 5.9.0
            solution = await Renamer.RenameSymbolAsync(solution, symbol,
                new SymbolRenameOptions(), newName);

            project = solution.GetProject(project.Id);

            foreach (var d in project.Documents)
            {
                var newRoot = await d.GetSyntaxRootAsync();
                result.Add(new DocInfo(d.Name, newRoot.ToFullString()));
            }

            return result;
        }         

        internal async Task<List<DocInfo>> GenerateMethod(
            Analyzer ana, DocInfo info, int position)
        {
            Document doc = FindDocByName(info.full);

            var found = await ana.AnalyzeDoc(doc, position, project.Documents);

            if (found == null)
                return null;

            return UpdateSolution(ana, found);
        }        

        private List<DocInfo> UpdateSolution(Analyzer ana, ClassData cd)
        {
            var result = new List<DocInfo>();

            var method = ana.GenerateMethod();

            if (!FMsgBox.Show(
                   $"Generate method in class {cd.name}?" +
                   $"\n\n{method.NormalizeWhitespace()}", true))
                return result;
            
            var solution = workspace.CurrentSolution;
            var newCls = cd.syClass.AddMembers(method);
            SyntaxNode newRoot = cd.syRoot.ReplaceNode(cd.syClass, newCls);
            newRoot = Formatter.Format(newRoot, workspace);
            solution = project.Solution.WithDocumentSyntaxRoot(cd.docId, newRoot);
            workspace.TryApplyChanges(solution);

            project = solution.GetProject(project.Id);
            foreach (var d in project.Documents)
            {
                var newr = Task.Run(() => d.GetSyntaxRootAsync()).Result;
                result.Add(new DocInfo(d.Name, newr.ToFullString()));
            }

            return result;
        }
    }
}
