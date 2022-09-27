using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
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
using Microsoft.CodeAnalysis.Formatting;
using System.Windows.Forms;

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

    public class Worker
    {
        // this will also install Workspaces + Microsoft.CodeAnalysis.CSharp:
        //Install-Package Microsoft.CodeAnalysis.CSharp.Features -Version 3.9.0        

        //private readonly CompositionHost _compositionContext;

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


        //Static Extension Methods are not returned by the Roslyn CompletionService
        ////https://stackoverflow.com/questions/59791893/static-extension-methods-are-not-returned-by-the-roslyn-completionservice
        //private static readonly ImmutableArray<Assembly> _defaultAssemblyes =
        //    _defaultTypes.Select(x => x.GetTypeInfo().Assembly).Distinct().Concat(new[]
        //    {
        //        Assembly.Load(new AssemblyName("System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")),
        //        typeof(Microsoft.CSharp.RuntimeBinder.Binder).GetTypeInfo().Assembly,
        //    })
        //    .ToImmutableArray();


        //private static readonly MetadataReference CorlibReference =
        //    MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

        //private static readonly MetadataReference SystemCoreReference =
        //    MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);

        //private static readonly MetadataReference CSharpRuntimeBinder =
        //    MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.Binder).Assembly.Location);

        private static readonly CSharpCompilationOptions _options =
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOverflowChecks(false)
                .WithOptimizationLevel(OptimizationLevel.Release);
        //.WithUsings(DefaultNamespaces);

        //private ImmutableArray<MetadataReference> references;

        private AdhocWorkspace workspace = null;
        private Project project = null;
        private SyntaxTree[] trees = null;
        private Compilation compilation = null;
        private Assembly assembly = null;
        private readonly ConsoleOutput _consoleOutput = new ConsoleOutput();

        public Action<string> WriteInfo = (s) => { };


        public Worker()
        {
            // System.NotSupportedException : The language 'C#' is not supported error:
            var _ = typeof(Microsoft.CodeAnalysis.CSharp.Formatting.CSharpFormattingOptions);

            _host = MefHostServices.Create(MefHostServices.DefaultAssemblies);

            //var partTypes = MefHostServices.DefaultAssemblies.Concat(_defaultReferenceAssemblies)
            //        .Distinct()
            //        .SelectMany(x => x.DefinedTypes)
            //        .Select(x => x.AsType())
            //        .ToArray();

            //_compositionContext = new ContainerConfiguration()
            //    .WithParts(partTypes)
            //    .CreateContainer();

            //_host = MefHostServices.Create(_compositionContext);

            workspace = new AdhocWorkspace(_host);

            //references =
            //    _defaultTypes.Select(t => MetadataReference
            //    .CreateFromFile(t.Assembly.Location) as MetadataReference)                
            //    .ToImmutableArray();

            AddNetFrameworkDefaultReferences();

            AddThirdPartyRefs();
        }


        public bool AddAssembly(string assemblyDll)
        {
            if (string.IsNullOrEmpty(assemblyDll)) return false;

            var file = Path.GetFullPath(assemblyDll);

            if (!File.Exists(file))
            {
                // check framework or dedicated runtime app folder
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
            AddAssembly("mscorlib.dll");
            AddAssembly("System.dll");
            AddAssembly("System.Core.dll");
            AddAssembly("Microsoft.CSharp.dll");
            AddAssembly("System.Net.Http.dll");

            //AddAssembly(typeof(Microsoft.CodeAnalysis.CSharpExtensions));

            // this library and CodeAnalysis libs
            //AddAssembly(typeof(ReferenceList)); // Scripting Library
        }

        public void AddThirdPartyRefs()
        {
            foreach (var full in RefsForm.GetRefsFilesList())
                AddAssembly(full);

            //references.Add(MetadataReference.CreateFromFile(full));
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
            //https://github.com/Vannevelj/RoslynTester/blob/master/RoslynTester/RoslynTester/Helpers/DiagnosticVerifier.cs#L433

            workspace.ClearSolution();
            MakeAndAddProject();

            var solution = workspace.CurrentSolution;

            foreach (var item in list)
                AddDocument(item, ref solution);

            //workspace.TryApplyChanges(solution);
            project = solution.GetProject(project.Id);
        }

        private Document FindDocByName(string full)
        {
            return project.Documents.FirstOrDefault(d => d.Name.Equals(full));
        }

        public async Task<List<string>> ReadCompletionItems(string docname, string word)
        {
            //Document doc = MakeDocument(docInfo);

            Document doc = FindDocByName(docname);
            var root = await doc.GetSyntaxRootAsync();
            var code = root.ToFullString();
            var position = code.LastIndexOf(word) + word.Length;

            var completionService = CompletionService.GetService(doc);
            var results = await completionService.GetCompletionsAsync(doc, position);

            var list = new List<string>();
            if (results == null)
                return list;

            foreach (var i in results.Items)
                list.Add(i.DisplayText);

            return list;
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

            compilation = GetCompilations(project.Documents.ToArray()).Result;

            assembly = GetAssembly(compilation);
        }

        private async Task<Compilation> GetCompilations(params Document[] documents)
        {
            WriteInfo("Compile...");

            var syntaxTrees = documents.Select(async (d) => await d.GetSyntaxTreeAsync());

            trees = await Task.WhenAll(syntaxTrees);

            string asmName = Path.GetRandomFileName(); //"MyCompilation" nbo45m3c.ap1

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

                    var errors = new List<Tuple<string, int, string>>();
                    foreach (Diagnostic fail in failures)
                    {
                        errors.Add(Tuple.Create(
                            fail.Location.SourceTree.FilePath,
                            fail.Location.SourceSpan.Start,
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

            return _consoleOutput.GetOutput();
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

        public async Task<List<DocInfo>> RenameSymbol(DocInfo docInfo,
            int position, string newName)
        {
            var result = new List<DocInfo>();

            Document doc = MakeDocument(docInfo);

            var symbol = await SymbolFinder.FindSymbolAtPositionAsync(doc, position);

            if (symbol == null)
                return result;

            //https://github.com/dotnet/roslyn/blob/main/src/Workspaces/Core/Portable/FindSymbols/SymbolFinder.cs

            var solution = doc.Project.Solution;

            solution = await Renamer.RenameSymbolAsync(solution, symbol,
                        newName, solution.Workspace.Options);

            //workspace.TryApplyChanges(solution);

            project = solution.GetProject(project.Id);

            foreach (var d in project.Documents)
            {
                var newRoot = d.GetSyntaxRootAsync().Result;
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
                var newr = d.GetSyntaxRootAsync().Result;
                result.Add(new DocInfo(d.Name, newr.ToFullString()));
            }

            return result;
        }
    }
}


