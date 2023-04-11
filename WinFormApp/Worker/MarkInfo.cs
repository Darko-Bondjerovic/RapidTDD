using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinFormApp
{
    public class MarkInfo
    {
        public int start = -1;
        public int ends = -1;
        private bool before = false;
        
        public string tabName = "";
        public string className = "";
        public int place = -1;
        
        public bool IsHit = false;

        public MarkInfo(string className, int place)
        {            
            this.className = className;
            this.place = place;
        }   

        public MarkInfo(string tabName, string className, StatementSyntax sy,
           bool before = false)
        {
            this.tabName = tabName;
            this.className = className;
            this.before = before;

            start = sy.Span.Start;
            ends = sy.Span.End;
            place = start;

            if (className == "")
                place = ends;
        }

        public string MakeMark()
        {
            if (className == "")
                return "}";

            var str = before ? "{" : "";
            
            return str + $"CODECVG.ADD(\"{className}\",{place});";
        }

        public override string ToString()
        {
            return $"{className} {place}: {start} {ends} ";
        }

        public string GetTextCode(string input)
        {
            if (className == "") return "";
            return input.Substring(start, ends - start);
        }

        public override bool Equals(object obj)
        {
            return obj is MarkInfo info &&
                   className == info.className &&
                   place == info.place;
        }

        public override int GetHashCode()
        {
            int hashCode = 847039544;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(className);
            hashCode = hashCode * -1521134295 + place.GetHashCode();
            return hashCode;
        }
    }
}