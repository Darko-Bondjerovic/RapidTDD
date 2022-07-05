using FastColoredTextBoxNS;
using System.Drawing;
using System;

namespace WinFormApp.Themes
{
    public static class ThemeExt
	{
		public static TextStyle Clone(this TextStyle textStyle, int val = 0)
		{
			if (textStyle.ForeBrush == null) return null;
			Color c = ((SolidBrush)textStyle.ForeBrush).Color;
			int r = Math.Max(0, c.R - val);
			int g = Math.Max(0, c.G - val);
			int b = Math.Max(0, c.B - val);
			Brush newForeBrush = new SolidBrush(Color.FromArgb(r, g, b));
			return new TextStyle(newForeBrush, textStyle.BackgroundBrush, textStyle.FontStyle);
		}

		public static void RefreshTheme(this FastColoredTextBox tb, Theme StyleTheme)			
		{
			tb.Range.ClearStyle(StyleIndex.All);
			for (int i = 0; i < tb.Styles.Length; i++)
				tb.Styles[i] = null;
			tb.DefaultStyle.ForeBrush = new SolidBrush(tb.ForeColor);
			((TextStyle)(tb.SyntaxHighlighter.BoldStyle)).ForeBrush = new SolidBrush(tb.ForeColor);
			//((TextStyle)(ftb.SyntaxHighlighter.BoldStyle2)).ForeBrush = new SolidBrush(ftb.ForeColor);
			tb.SyntaxHighlighter.InitStyleSchema(Language.CSharp);
			InitStyleTheme(tb.SyntaxHighlighter, StyleTheme);
			tb.OnSyntaxHighlight(new TextChangedEventArgs(tb.Range));
			tb.Refresh();			
		}

		private static void InitStyleTheme(SyntaxHighlighter sb, Theme StyleTheme)
		{
			if (StyleTheme == null) return;
			if (StyleTheme.StringStyle != null)   sb.StringStyle = StyleTheme.StringStyle;
			if (StyleTheme.CommentStyle != null)    sb.CommentStyle = StyleTheme.CommentStyle;
			if (StyleTheme.NumberStyle != null)     sb.NumberStyle = StyleTheme.NumberStyle;
			if (StyleTheme.ClassNameStyle != null)  sb.ClassNameStyle = StyleTheme.ClassNameStyle;
			if (StyleTheme.KeywordStyle != null)    sb.KeywordStyle = StyleTheme.KeywordStyle;
			if (StyleTheme.TagBracketStyle != null) sb.TagBracketStyle = StyleTheme.TagBracketStyle;
			if (StyleTheme.CommentTagStyle != null) sb.CommentTagStyle = StyleTheme.CommentTagStyle;
			if (StyleTheme.FunctionsStyle != null)  sb.FunctionsStyle = StyleTheme.FunctionsStyle;
			if (StyleTheme.VariableStyle != null)   sb.VariableStyle = StyleTheme.VariableStyle;
			//if (StyleTheme.ConstantsStyle != null)  sb.ConstantsStyle = StyleTheme.ConstantsStyle;
			//if (StyleTheme.DeclFunctionStyle != null) sb.DeclFunctionStyle = StyleTheme.DeclFunctionStyle;
		}
	}
}
