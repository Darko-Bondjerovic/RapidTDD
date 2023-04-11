using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WinFormApp
{
    public class CodeCover
    {
        private SyntaxTree tree = null;
        private string source = "";
        
        public List<MarkInfo> markers = new List<MarkInfo>();
        private string tabName = "";

        public CodeCover(string source)
        {
            this.source = source;
            tree = CSharpSyntaxTree.ParseText(source);
        }
        
        public string MakeForCC(string tabName)
        {
            this.tabName = tabName;
            FindStatsInTree(false);
            return MarkCode();
        }

        private List<T> GetListForType<T>()
        {
            return tree.GetRoot().DescendantNodesAndSelf().OfType<T>().ToList();
        }

        internal int FindClassSpanStartInCode(string onlyClassName)
        {
            var list = GetListForType<ClassDeclarationSyntax>();

            foreach (var item in list)
                if (item.Identifier.ToString().Equals(onlyClassName))
                    return item.SpanStart;

            return 0;
        }

        internal void FindStatsInTree(bool print)
        {
            // constructors
            GetListForType<ConstructorDeclarationSyntax>()
                .ForEach( x => MakeMarkForStats(x.Body, print));
                
            // methods
            GetListForType<MethodDeclarationSyntax>()
                .ForEach( x => MakeMarkForStats(x.Body, print));
               
            // propertyes (getter/setter)
            GetListForType<AccessorDeclarationSyntax>()
                .ForEach( x => MakeMarkForStats(x.Body, print));
        }
        
        private void MakeMarkForStats(BlockSyntax body, bool print = false)
        {
            if (body == null)
                return;

            var className = GetClassNameFromSyntax(body);
            
            var list = body.DescendantNodes().OfType<StatementSyntax>()
               .ToList().Where( x => !(x is BlockSyntax)).Distinct();
            
            foreach(var sy in list)
            {                
                MarkSimpleStat(className, sy);

                if (print)
                {
                    Console.WriteLine($"Parent: {sy.Parent.Kind()}");
                    Console.WriteLine(StatToStr(sy));
                }
            }
        }
        
        private void MarkSimpleStat(string className,  StatementSyntax sy)
        {
            var inBlock = sy.Parent is BlockSyntax || sy.Parent is SwitchSectionSyntax;

            if (inBlock)
            {
                markers.Add( new MarkInfo(tabName, className, sy));
            }
            else
            {
                // we shold add 2 braces here
                markers.Add( new MarkInfo(tabName, className, sy, true)); // {+mark
                markers.Add( new MarkInfo(tabName, "", sy)); // }
            }
        }

        internal string MarkCode()
        {
            var result = source;
            
            foreach (var m in markers.OrderByDescending(x => x.place))
                result = result.Insert(m.place, m.MakeMark());

            return result;
        }
        
        public static string FormatCode(string input)
        {
            var tr = CSharpSyntaxTree.ParseText(input);

            var str = tr.GetRoot().NormalizeWhitespace()
                .SyntaxTree.GetText().ToString();

            return str;
        }
        
        //=======================================================================================

        private static string StatToStr(StatementSyntax sy)
        {
            var parentStr = sy.Kind().ToString();            
            var text = sy.ToString();                        
            return $"{parentStr}\n{text}\n----------------\n";
        }

        
        private static bool TryGetParentSyntax<T>(SyntaxNode syntaxNode, out T result) where T : SyntaxNode
        {
            //https://stackoverflow.com/questions/20458457/getting-class-fullname-including-namespace-from-roslyn-classdeclarationsyntax

            // set defaults
            result = null;
            if (syntaxNode == null)
                return false;

            try
            {
                syntaxNode = syntaxNode.Parent;
                if (syntaxNode == null)
                    return false;

                if (syntaxNode.GetType() == typeof(T))
                {
                    result = syntaxNode as T;
                    return true;
                }

                return TryGetParentSyntax<T>(syntaxNode, out result);
            }
            catch
            {
                return false;
            }
        }

        private string GetClassNameWithNamespace(ClassDeclarationSyntax cds)
        {
            var onlyClassName = cds.Identifier.ToString();

            NamespaceDeclarationSyntax nds = null;
            if (!TryGetParentSyntax(cds, out nds))
                return onlyClassName;

            var namespaceName = nds.Name.ToString();
            return $"{namespaceName}.{onlyClassName}";
        }

        private string GetClassNameFromSyntax(StatementSyntax stat)
        {
            ClassDeclarationSyntax cds = null;
            if (!TryGetParentSyntax(stat, out cds))
                return "";

            return GetClassNameWithNamespace(cds);
        }       
    }
}