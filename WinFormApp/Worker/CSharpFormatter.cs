using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

namespace WinFormApp
{
    public class CSharpFormatter
    {
        private const string ResourceName = "WinFormApp.Resources.CSharpFormatter.editorconfig";
        private readonly string _editorConfigText;

        public CSharpFormatter()
        {
            _editorConfigText = LoadEditorConfig() ?? FallbackEditorConfig;
        }

        private static string FallbackEditorConfig => @"
root = true
[*]
insert_final_newline = true
indent_style = space
indent_size = 4
[*.cs]
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true
";

        public string Format(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return code;
            try
            {
                using (var workspace = new AdhocWorkspace())
                {
                    const string baseDir = "/virtual/CSharpFormatter";
                    string sourceFile = baseDir + "/FormattedCode.cs";
                    string editorConfigFile = baseDir + "/.editorconfig";
                    string projectFile = baseDir + "/CSharpFormatter.csproj";

                    var projectId = ProjectId.CreateNewId();
                    var documentId = DocumentId.CreateNewId(projectId);

                    // AnalyzerConfigDocumentId via reflection (da kompajlira na svim verzijama)
                    var analyzerConfigDocIdType = Type.GetType("Microsoft.CodeAnalysis.AnalyzerConfigDocumentId, Microsoft.CodeAnalysis.Workspaces.Common");
                    object analyzerConfigId;
                    if (analyzerConfigDocIdType != null)
                        analyzerConfigId = analyzerConfigDocIdType.GetMethod("CreateNewId", new[] { typeof(ProjectId) }).Invoke(null, new object[] { projectId });
                    else
                        analyzerConfigId = DocumentId.CreateNewId(projectId);

                    var projectInfo = ProjectInfo.Create(projectId, VersionStamp.Create(), "CSharpFormatter", "CSharpFormatter", LanguageNames.CSharp, filePath: projectFile);
                    var solution = workspace.CurrentSolution.AddProject(projectInfo);
                    solution = solution.AddDocument(documentId, "FormattedCode.cs", SourceText.From(code), filePath: sourceFile);

                    var addMethod = solution.GetType().GetMethods().FirstOrDefault(m => m.Name == "AddAnalyzerConfigDocument" && m.GetParameters().Length == 4);
                    if (addMethod != null)
                        solution = (Solution)addMethod.Invoke(solution, new object[] { analyzerConfigId, ".editorconfig", SourceText.From(_editorConfigText), editorConfigFile });
                    else
                        solution = solution.AddAnalyzerConfigDocument((DocumentId)analyzerConfigId, ".editorconfig", SourceText.From(_editorConfigText), filePath: editorConfigFile);

                    workspace.TryApplyChanges(solution);

                    var document = workspace.CurrentSolution.GetDocument(documentId);
                    if (document == null) return NormalizeFallback(code);

                    // Prvo probaj editorconfig formatter
                    try
                    {
                        var formattedDoc = Formatter.FormatAsync(document).GetAwaiter().GetResult();
                        var root = formattedDoc.GetSyntaxRootAsync().GetAwaiter().GetResult();
                        // UVEK Normalize da garantujes nove redove
                        var result = root.NormalizeWhitespace().ToFullString();
                        // Ako je Normalize vratio 1 liniju (nemoguce), vrati fallback
                        if (result.Contains("\n") || result.Contains("\r") || result.Count(c => c == '{') > 1)
                            return result;
                        return result;
                    }
                    catch
                    {
                        return NormalizeFallback(code);
                    }
                }
            }
            catch
            {
                return NormalizeFallback(code);
            }
        }

        private static string NormalizeFallback(string code)
        {
            try
            {
                var tree = CSharpSyntaxTree.ParseText(code);
                var root = tree.GetRoot();
                return root.NormalizeWhitespace().ToFullString();
            }
            catch { return code; }
        }

        private static string LoadEditorConfig()
        {
            try
            {
                var asm = typeof(CSharpFormatter).Assembly;
                var names = asm.GetManifestResourceNames();
                var match = names.FirstOrDefault(n => n.EndsWith("CSharpFormatter.editorconfig", StringComparison.OrdinalIgnoreCase));
                if (match != null)
                {
                    using (var stream = asm.GetManifestResourceStream(match))
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
                using (var stream = asm.GetManifestResourceStream(ResourceName))
                {
                    if (stream == null) return null;
                    using (var reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }
            catch { return null; }
        }
    }
}
