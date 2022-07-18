using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace WinFormApp 
{
    public class ClassData
    {
        public SyntaxNode syRoot = null;

        public ClassDeclarationSyntax syClass = null;

        public DocumentId docId;

        public string name = "";
        public int start = -1;
        public int endof = -1;

        public ClassData() { }

        public ClassData(string name, int start, int endof)
        {
            this.name = name;
            this.start = start;
            this.endof = endof;
        }

        public override string ToString()
        {
            return $"{name} {start} {endof}";
        }
    }

    public class Analyzer
    {
        static public string BODYLINE = "throw new NotImplementedException();";

        private SyntaxTree tree = null;
        private string input = "";

        internal string MethodName { get; set; } = "";
        internal List<string> ParamTypes = new List<string>();
        private int StatClassLoc = 0;

        internal string ClassName { get; set; } = "";
        internal bool HaveNewKey { get; set; } = false;
        internal List<ISymbol> VarsNames { get; set; } = new List<ISymbol>();
        internal string ReturnType { get; set; } = "";
        internal bool IsStaticMethod { get; set; } = false;

        public override string ToString()
        {            
            var str = new StringBuilder();
            var isstat = IsStaticMethod ? "Is static" : "";
            str.AppendLine($"MethodName [{MethodName}] {isstat}");
            str.AppendLine($"ParamTypes [{string.Join(",", ParamTypes)}]");
            str.AppendLine($"HaveNewKey [{HaveNewKey}]");
            str.AppendLine($"ClassName [{ClassName}]");
            str.AppendLine($"VarName [{string.Join(",", VarsNames)}]");
            str.AppendLine($"ReturnType [{ReturnType}]");
            return str.ToString();
        }

        public void Run(string input)
        {
            this.input = input;
            ParseInput();
        }

        private void ParseInput()
        {
            tree = CSharpSyntaxTree.ParseText(input);

            FindMethod();
            FindParams();
            FindNewClass();

            HaveNewKey = !string.IsNullOrEmpty(ClassName);
            if (!HaveNewKey)
                FindStatClass();

            FindReturnType();

            FindIsStaticMethod();
        }

        private void FindIsStaticMethod()
        {
            IsStaticMethod = false;

            if (MethodName == "" && ClassName == "")
                return;

            var noSpaces = input.Replace(" ", "");
            IsStaticMethod = noSpaces.Contains(ClassName + ".");
        }

        private MetadataReference mscorlib = null;

        private MetadataReference Mscorlib
        {
            get
            {
                if (mscorlib == null)
                {
                    mscorlib = MetadataReference.CreateFromFile(
                        typeof(object).Assembly.Location);
                }
                return mscorlib;
            }
        }

        private SemanticModel GetSemanticModel(SyntaxTree syntaxTree)
        {
            CSharpCompilation compilation = CSharpCompilation.Create(new Guid().ToString(),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { syntaxTree }, references: new[] { Mscorlib });
            return compilation.GetSemanticModel(syntaxTree);
        }

        private void FindReturnType()
        {
            var vars = tree.GetRoot().DescendantNodes()
                .OfType<VariableDeclaratorSyntax>().ToList();

            var result = string.Empty;
            foreach (var vrb in vars)
            {
                try
                {
                    var model = GetSemanticModel(tree);
                    var decSymbol = model.GetDeclaredSymbol(vrb);
                    if (decSymbol != null)
                    {
                        VarsNames.Add(decSymbol); //add variable 

                        ITypeSymbol type = ((ILocalSymbol)decSymbol).Type;
                        result = type.ToString();
                        if (result.Equals("?"))
                            result = "object";
                    }
                }
                catch { }
            }

            ReturnType = result;

            if (VarsNames.Count() == 0)
                ReturnType = "void";

            if (MethodName == "" && ClassName != "")
                ReturnType = "";
        }

        private void FindStatClass()
        {
            var stats = tree.GetRoot().DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .Where(m => m.Kind() == SyntaxKind.SimpleMemberAccessExpression)
                .FirstOrDefault();

            if (stats != null)
            {
                StatClassLoc = stats.GetLocation().SourceSpan.Start;
                ClassName = stats.Expression.ToFullString();
            }
        }

        private void FindNewClass()
        {
            // https://stackoverflow.com/questions/43804765/roslyn-get-identifiername-in-objectcreationexpressionsyntax
            var cls = tree.GetRoot().DescendantNodes()
                .OfType<ObjectCreationExpressionSyntax>()
                .FirstOrDefault();

            if (cls != null)
            {
                //ClassName = cls.Type.ToFullString();
                try
                {
                    var ns = cls.Type as NameSyntax;
                    if (ns != null)
                        ClassName = ns.ToString();
                }
                catch { }

                try
                {
                    var pts = cls.Type as PredefinedTypeSyntax;
                    if (pts != null)
                        ClassName = pts.Keyword.ToString();
                }
                catch { }

            }
        }


        private void FindParams()
        {
            //https://stackoverflow.com/questions/66991517/source-generation-how-to-get-involved-types-from-invocationexpressionsyntax

            var arguments = tree.GetRoot().DescendantNodes()
                .OfType<LiteralExpressionSyntax>().ToList();

            //https://csharp.hotexamples.com/examples/-/LiteralExpressionSyntax/Kind/php-literalexpressionsyntax-kind-method-examples.html
            //Example #6

            var varType = "object";
            foreach (var arg in arguments)
            {
                switch (arg.Kind())
                {
                    case SyntaxKind.NumericLiteralExpression:
                        varType = "int"; break;
                    case SyntaxKind.StringLiteralExpression:
                        varType = "string"; break;
                    case SyntaxKind.CharacterLiteralExpression:
                        varType = "char"; break;
                    case SyntaxKind.TrueLiteralExpression:
                    case SyntaxKind.FalseLiteralExpression:
                        varType = "bool"; break;
                    case SyntaxKind.NullLiteralExpression:
                        varType = "object"; break;
                }

                ParamTypes.Add(varType);
            }
        }


        private void FindMethod()
        {
            //read from document: tree.GetRoot:
            //SyntaxNode root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            //https://stackoverflow.com/questions/55118805/extract-called-method-information-using-roslyn

            var invocation = tree.GetRoot().DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .FirstOrDefault();

            if (invocation != null)
            {
                // You need to check the Kind of the InvocationExpressionSyntax and 
                // figure out how you want to handle every possible expression.

                var expr = invocation.Expression;
                if (expr is IdentifierNameSyntax)
                {
                    IdentifierNameSyntax identifierName = expr as IdentifierNameSyntax; // identifierName is your method name
                    MethodName = identifierName.ToFullString();
                }

                if (expr is MemberAccessExpressionSyntax)
                {
                    MemberAccessExpressionSyntax memberAccessExpressionSyntax = expr as MemberAccessExpressionSyntax;
                    MethodName = memberAccessExpressionSyntax.Name.ToFullString();
                }
            }
        }

        public MethodDeclarationSyntax GenerateMethod()
        {
            // constructor 
            if (ClassName != "" && MethodName == "")
                MethodName = ClassName;

            return GetMethodDeclarationSyntax(BODYLINE);
        }

        static private IEnumerable<ParameterSyntax> GetParametersList(string[] parameterTypes, string[] paramterNames)
        {
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                yield return SyntaxFactory.Parameter(
                    attributeLists: SyntaxFactory.List<AttributeListSyntax>(),
                    modifiers: SyntaxFactory.TokenList(),
                    type: SyntaxFactory.ParseTypeName(parameterTypes[i]),
                    identifier: SyntaxFactory.Identifier(paramterNames[i]),
                    @default: null);
            }
        }

        private List<string> MakeVars()
        {
            var vars = new List<string>();
            for (int i = 0; i < ParamTypes.Count; i++)
                vars.Add($"a{i + 1}");
            return vars;
        }

        private SyntaxTokenList GetModifiers(bool isStatic = false)
        {
            SyntaxTokenList mlist = new SyntaxTokenList();
            if (isStatic) mlist = mlist.Add(Token(SyntaxKind.StaticKeyword));
            return mlist.Add(Token(SyntaxKind.PublicKeyword));
        }

        private MethodDeclarationSyntax GetMethodDeclarationSyntax(string methodCode = "")
        {
            //https://mattdufeu.co.uk/blog/roslyn-codefix-to-add-a-tostring-method/
            //https://stackoverflow.com/questions/37710714/roslyn-add-new-method-to-an-existing-class

            var parameterList = ParameterList(SeparatedList(
                GetParametersList(ParamTypes.ToArray(), MakeVars().ToArray())));

            var methodBody = ParseStatement(methodCode);

            return MethodDeclaration(attributeLists: List<AttributeListSyntax>(),
                modifiers: TokenList(GetModifiers(IsStaticMethod)),
                returnType: ParseTypeName(ReturnType),
                explicitInterfaceSpecifier: null,
                identifier: Identifier(MethodName),
                typeParameterList: null,
                parameterList: parameterList,
                constraintClauses: List<TypeParameterConstraintClauseSyntax>(),
                body: Block(methodBody),
                semicolonToken: Token(SyntaxKind.None));

            // Annotate that this node should be formatted
            //.WithAdditionalAnnotations(Formatter.Annotation);
        }

        private ClassDeclarationSyntax
            GetClassDeclarationSyntax(string name, bool isStatic = false)
        {
            var theClass = ClassDeclaration(name)
                .WithModifiers(GetModifiers(isStatic));
            //.WithParameterList(ParameterList(splist))
            //.WithBody(Block());

            return theClass;
        }

        internal string GenerateClass()
        {
            var newClass = GetClassDeclarationSyntax(ClassName);
            newClass = newClass.AddMembers(GenerateMethod())
                .NormalizeWhitespace();

            var newClassStr = "using System;\n\n" + newClass.ToFullString().Replace(BODYLINE, "//");

            if (DialogResult.No == MessageBox.Show(
               $"Generate class in new file?\n\n{newClassStr}", "Confirm",
               MessageBoxButtons.YesNo))
                return "";

            return newClassStr;
                
        }

        public async Task<List<ClassData>> FindClasses(Document doc)
        {
            var root = await doc.GetSyntaxRootAsync();
            var model = await doc.GetSemanticModelAsync();

            var classes = root.DescendantNodes()
                .OfType<ClassDeclarationSyntax>().ToList();

            var result = new List<ClassData>();
            foreach (var cds in classes)
            {
                var symb = model.GetDeclaredSymbol(cds) as ITypeSymbol;
                result.Add(new ClassData()
                {
                    syRoot = root,
                    syClass = cds,
                    name = symb.Name,
                    start = cds.FullSpan.Start,
                    endof = cds.FullSpan.End
                });
            }

            return result;
        }

        public static ITypeSymbol GetType(ISymbol symbol)
        {
            if (symbol is IFieldSymbol)
            {
                return ((IFieldSymbol)symbol).Type;
            }

            if (symbol is ILocalSymbol)
            {
                return ((ILocalSymbol)symbol).Type;
            }

            return null;
        }

        public async Task<ClassData> AnalyzeDoc(Document doc, 
               int position, IEnumerable<Document> allDocs)
        {
            ClassData found = null;

            // var m = new MyClass();
            // m.RunMethod(); <--- this is selected.

            if (MethodName != "" && IsStaticMethod)
            {
                var symbol = await SymbolFinder
                    .FindSymbolAtPositionAsync(doc,
                    position + StatClassLoc);

                if (symbol != null)
                {
                    try
                    {
                        //var parent = symbol.ContainingType.Name; // <-- Program

                        ITypeSymbol type = GetType(symbol);
                        
                        if (type != null && type.Name != "")
                        { 
                            ClassName = type.Name;
                            IsStaticMethod = false;
                        }
                    }
                    catch
                    {

                    }
                }
            }

            if (ClassName == "")
            {
                var cdlist = await FindClasses(doc);
                foreach (var c in cdlist)
                {
                    if (c.start <= position && position <= c.endof)
                    {
                        found = c;
                        found.docId = doc.Id;
                        break;
                    }
                }
            }
            else
            {
                // try find class in solution documents:
                foreach (var document in allDocs)
                {
                    var cdlist = await FindClasses(document);
                    foreach (var c in cdlist)
                    {
                        if (c.name.Equals(ClassName))
                        {
                            found = c;
                            found.docId = document.Id;
                            break;
                        }
                    }
                }
            }

            return found;
        }
    }
}