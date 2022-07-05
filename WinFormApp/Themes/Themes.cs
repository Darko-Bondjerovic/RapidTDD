using FastColoredTextBoxNS;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace WinFormApp.Themes
{
    public class INI
	{
		/// <summary>
		/// Path and name to settings ini file.
		/// </summary>
		public string File = "";

		public bool NoComments = false;

		/// <summary>
		/// Data of settings ini file as Dictionary
		/// </summary>
		public Dictionary<string, Dictionary<string, string>> Dict = new Dictionary<string, Dictionary<string, string>>();

		/// <summary>
		/// Encoding of the ini file. Default is UTF8.
		/// </summary>
		public Encoding FileEncoding = Encoding.UTF8;

		/// <summary>
		/// Lines of the text file
		/// </summary>
		private List<string> lines = new List<string>();

		private static Regex RegexComment = new Regex(@"(#.*)", RegexOptions.Multiline | RegexOptions.Compiled);
		private static Regex RegexSection = new Regex(@"^\[(.*)\]\r?$", RegexOptions.Multiline | RegexOptions.Compiled);

		/// <summary>
		/// Data of settings ini file as text
		/// </summary>
		public string Text
		{
			get
			{
				string text = "";
				foreach (string line in lines) text += "\n" + line;
				return text.Substring(1);
			}
			set
			{
				lines.Clear();
				foreach (string line in value.Split('\n'))
					lines.Add(line);
				UpdateDictionaryData();
			}
		}

		// CONSTRUCTOR
		public INI(string fileName = "")
		{
			if (fileName.Length > 0)
			{
				File = fileName;
				Load();
			}
		}

		/// <summary>
		/// Load the data of ini file
		/// </summary>
		public void Load()
		{
			if (System.IO.File.Exists(this.File))
				Text = System.IO.File.ReadAllText(this.File, FileEncoding);
		}

		/// <summary>
		/// Save ini text data to file
		/// </summary>
		public void Save()
		{
			System.IO.File.WriteAllText(this.File, Text, FileEncoding);
		}

		/// <summary>
		/// Update the Dictionary of the settings data
		/// </summary>
		private void UpdateDictionaryData()
		{
			Dict.Clear();
			string section = "", key = "", val = "";
			for (int i = 0; i < lines.Count; i++)
			{
				string line;
				if (NoComments) line = lines[i];
				else line = RegexComment.Replace(lines[i], "");
				if (line.Trim().Length == 0) continue;   // Skip empty lines and comments
														 // if current line is section - get current section name and continue
				if (RegexSection.IsMatch(line))
				{
					section = RegexSection.Match(line).Groups[1].Value;
					if (!Dict.ContainsKey(section)) Dict[section] = new Dictionary<string, string>();
					continue;
				}
				int n = line.IndexOf('=');
				if (n < 1)
				{
					key = line.Trim();
					val = "";
				}
				else
				{
					key = line.Substring(0, n).Trim();
					val = line.Substring(n + 1).Trim();
				}
				if (!Dict.ContainsKey(section)) Dict[section] = new Dictionary<string, string>();
				Dict[section][key] = val;
			}
		}

		/// <summary>
		/// Sets the value of the settings
		/// </summary>
		/// <param name="name">Name of the settings</param>
		/// <param name="value">Value of the settings</param>
		/// <param name="section">Section name (optional)</param>
		public void Set(string name, bool value, string section = "")
		{
			Set(name, value ? "1" : "0", section);
		}

		public void Set(string name, object value, string section = "")
		{
			Set(name, value.ToString(), section);
		}

		/// <summary>
		/// Sets the value of the settings
		/// </summary>
		/// <param name="name">Name of the settings</param>
		/// <param name="value">Value of the settings</param>
		/// <param name="section">Section name (optional)</param>
		public void Set(string name, string value, string section = "")
		{
			bool isFoundSection = false;
			bool isFoundKey = false;
			string currentSection = "";
			int lastLineOfSection = 0;
			for (int i = 0; i < lines.Count; i++)
			{
				string line = RegexComment.Replace(lines[i], "");
				if (line.Trim().Length == 0) continue;   // Skip empty lines and comments
														 // if current line is section - get current section name and continue
				if (RegexSection.IsMatch(line)) { currentSection = RegexSection.Match(line).Groups[1].Value; continue; }
				if (currentSection != section) continue; // Skip is not our sections

				isFoundSection = true;
				lastLineOfSection = i;

				Match m = Regex.Match(line, @"^(.*?)=");
				if (!m.Success) continue;
				string key = m.Groups[1].Value.Trim();
				if (key == name)
				{
					isFoundKey = true;
					lines[i] = m.Value + " " + value;
					break;
				}
			}

			if (!isFoundSection)
			{
				if (section.Length > 0)
				{
					lines.Add("");
					lines.Add("[" + section + "]");
				}
				lastLineOfSection = lines.Count - 1;
			}

			if (!isFoundKey)
			{
				lines.Insert(lastLineOfSection + 1, name + " = " + value);
			}

			if (!Dict.ContainsKey(section)) Dict[section] = new Dictionary<string, string>();
			Dict[section][name] = value;
		}

		/// <summary>
		/// Get the value of the settings by name
		/// </summary>
		/// <param name="name">Name of the settings</param>
		/// <param name="section">Section name (optional)</param>
		/// <param name="defaultValue">Default value. Returned if there is no settings with such section and name</param>
		/// <returns>Returns value of the settings</returns>
		public string Get(string name, string section = "", string defaultValue = "")
		{
			string value = defaultValue;
			if (Dict.ContainsKey(section))
			{
				if (Dict[section].ContainsKey(name))
					value = Dict[section][name];
			}
			return value;
		}

		/// <summary>
		/// Check is set value to "1", "Yes" or "Y"
		/// </summary>
		/// <param name="name">Name of the settings</param>
		/// <param name="section">Section name (optional)</param>
		/// <param name="defaultValue">Default value. Returned if there is no settings with such section and name</param>
		/// <returns>Returns the true if the setting value is "1", "Yes" or "Y". Else returns false.</returns>
		public bool Get(string name, string section = "", bool defaultValue = false)
		{
			bool retValue = defaultValue;
			if (Dict.ContainsKey(section))
			{
				if (Dict[section].ContainsKey(name))
				{
					string value = Dict[section][name].Trim();
					retValue = ((value == "1") || value.ToLower().StartsWith("y"));
				}
			}
			return retValue;
		}

		/// <summary>
		/// Gets all lines of section as list of strings
		/// </summary>
		/// <param name="section">Section name (optional)</param>
		/// <param name="excludeComments">If true - exclude empty lines and comments</param>
		/// <returns>Return List of the lines</returns>
		public List<string> GetLines(string section = "", bool excludeComments = false)
		{
			List<string> sectLines = new List<string>();
			string currentSection = "", line;
			for (int i = 0; i < lines.Count; i++)
			{
				line = lines[i];
				if (excludeComments)
				{   // Skip empty lines and comments
					line = RegexComment.Replace(line, "");
					if (line.Trim().Length == 0) continue;
				}
				// if current line is section - get current section name and continue
				if (RegexSection.IsMatch(line)) { currentSection = RegexSection.Match(line).Groups[1].Value; continue; }
				if (currentSection != section) continue; // Skip is not our sections
				sectLines.Add(line);
			}
			return sectLines;
		}

		/// <summary>
		/// Get contents of the section
		/// </summary>
		/// <param name="section">Section name</param>
		/// <param name="excludeComments">If true - exclude empty lines and comments</param>
		/// <returns>Returns content of section as string</returns>
		public string GetSectionText(string section, bool excludeComments = false)
		{
			List<string> t = GetLines(section, excludeComments);
			string text = "";
			foreach (string line in t) text += line + "\n";
			return text.Substring(0, text.Length - 1);
		}
	}

	public class Theme
	{
		public string Author;
		public string Name;

		public Color Background = Color.White;
		public Color Caret = Color.Black;
		public Color Foreground = Color.Black;
		public Color Invisibles = Color.Gray;
		public Color LineHighlight = Color.FromArgb(100, 210, 210, 255);
		public Color ChangedLines = Color.FromArgb(255, 152, 251, 152);
		public Color Selection = Color.FromArgb(60, 0, 0, 255);

		public Color IndentBackColor;
		public Color LineNumberColor;
		public Color PaddingBackColor;
		public Color BreakpointLineColor = Color.FromArgb(255, 255, 128, 128);

		public Style StringStyle;
		public Style CommentStyle;
		public Style NumberStyle;
		public Style AttributeStyle;
		public Style ClassNameStyle;
		public Style KeywordStyle;
		public Style ConstantsStyle;
		public Style CommentTagStyle; // only csharp
		public Style DeclFunctionStyle;

		public Style TagBracketStyle; // only YAML HTML
		public Style TagNameStyle;        // HTML
		public Style AttributeValueStyle; // HTML
		public Style HtmlEntityStyle;     // HTML

		public Style XmlTagBracketStyle;     // XML
		public Style XmlTagNameStyle;        // XML
		public Style XmlAttributeStyle;      // XML
		public Style XmlAttributeValueStyle; // XML
		public Style XmlEntityStyle;         // XML
		public Style XmlCDataStyle;          // XML

		public Style FunctionsStyle;         // Lua SQL
		public Style VariableStyle;          // PHP SQL
		public Style StatementsStyle;        // SQL
		public Style TypesStyle;             // PHP SQL
		public Style KeywordStyle2;          // PHP
		public Style KeywordStyle3;          // PHP

		public Style InvisibleStyle;
	}

	static public class Themes
    {
		public static Dictionary<string, Theme> Dict = new Dictionary<string, Theme>();

		static public void Init()
        {			
			LoadThemesFromString(ReadThemesFromResources());
		}

		public static void SetTheme(FastColoredTextBox editor, string name)
		{
			if (Dict.ContainsKey(name))
			{
				Theme t = Dict[name];
				if ((t.ConstantsStyle == null) && (t.NumberStyle != null) && (name != "Standard"))
					t.ConstantsStyle = ((TextStyle)t.NumberStyle).Clone(30);

				editor.BackColor = t.Background;
				editor.CaretColor = t.Caret;
				editor.ForeColor = t.Foreground;

				editor.SelectionColor = t.Selection;
				editor.PaddingBackColor = t.Background;

				editor.IndentBackColor = (t.IndentBackColor.Name != "0") ? t.IndentBackColor : editor.BackColor;
				editor.LineNumberColor = (t.LineNumberColor.Name != "0") ? t.LineNumberColor : Color.FromArgb(150, editor.ForeColor);
				editor.PaddingBackColor = (t.PaddingBackColor.Name != "0") ? t.PaddingBackColor : Color.FromArgb(150, editor.BackColor);

				editor.RefreshTheme(t);
			}
		}

		private static Color ToColor(string val)
		{
			if (string.IsNullOrEmpty(val)) return Color.Transparent;

			if (val.Length == 9)
			{
				string aval = val.Substring(7, 2);
				val = "#" + aval + val.Substring(1, 6);
			}
			ColorConverter cc = new ColorConverter();
			Color c = (Color)cc.ConvertFromString(val);
			return c;
		}

		private static Style ToStyle(string foreVal, string backVal = "", bool bold = false, bool italic = false, bool underline = false)
		{
			FontStyle fs = FontStyle.Regular;
			if (italic) fs |= FontStyle.Italic;
			if (bold) fs |= FontStyle.Bold;
			if (underline) fs |= FontStyle.Underline;
			Brush backBrush = null;
			Brush foreBrush = null;
			if (foreVal != "")
				foreBrush = new SolidBrush(ToColor(foreVal));
			if (backVal != "")
				backBrush = new SolidBrush(ToColor(backVal));
			Style s = new TextStyle(foreBrush, backBrush, fs);
			return s;
		}

		private static Style ToStyle2(string styleString)
		{
			//{fore:#8ABBE7;fore:#FFFFFF;font:Bold, Italic;}
			string fore = Regex.Match(styleString, "fore:(.*?);").Groups[1].Value;
			string back = Regex.Match(styleString, "back:(.*?);").Groups[1].Value;
			bool bold = Regex.IsMatch(styleString, "font:.*?Bold", RegexOptions.IgnoreCase);
			bool italic = Regex.IsMatch(styleString, "font:.*?Italic", RegexOptions.IgnoreCase);
			bool underline = Regex.IsMatch(styleString, "font:.*?Underline", RegexOptions.IgnoreCase);
			return ToStyle(fore, back, bold, italic, underline);
		}

		public static void LoadThemesFromString(string data)
		{
			INI tini = new INI();
			tini.NoComments = true;
			tini.Text = data;
			Dict.Clear();
			foreach (string section in tini.Dict.Keys)
			{
				Theme tt = new Theme();
				tt.Name = section;
				tt.Background = ToColor(tini.Get("Background", section, ""));
				tt.Caret = ToColor(tini.Get("Caret", section, ""));
				tt.Foreground = ToColor(tini.Get("Foreground", section, ""));
				tt.Invisibles = ToColor(tini.Get("Invisibles", section, ""));
				tt.LineHighlight = ToColor(tini.Get("LineHighlight", section, ""));
				tt.ChangedLines = ToColor(tini.Get("ChangedLines", section, ""));
				tt.Selection = ToColor(tini.Get("Selection", section, ""));
				tt.LineNumberColor = ToColor(tini.Get("LineNumberColor", section, ""));
				tt.IndentBackColor = ToColor(tini.Get("IndentBackColor", section, ""));

				tt.StringStyle = ToStyle2(tini.Get("StringStyle", section, ""));
				tt.CommentStyle = ToStyle2(tini.Get("CommentStyle", section, ""));
				tt.NumberStyle = ToStyle2(tini.Get("NumberStyle", section, ""));
				tt.AttributeStyle = ToStyle2(tini.Get("AttributeStyle", section, ""));
				tt.ClassNameStyle = ToStyle2(tini.Get("ClassNameStyle", section, ""));
				tt.KeywordStyle = ToStyle2(tini.Get("KeywordStyle", section, ""));
				tt.CommentTagStyle = ToStyle2(tini.Get("CommentTagStyle", section, ""));
				tt.TagBracketStyle = ToStyle2(tini.Get("TagBracketStyle", section, ""));
				tt.FunctionsStyle = ToStyle2(tini.Get("FunctionsStyle", section, ""));
				tt.VariableStyle = ToStyle2(tini.Get("VariableStyle", section, ""));
				tt.DeclFunctionStyle = ToStyle2(tini.Get("DeclFunctionStyle", section, ""));
				tt.InvisibleStyle = ToStyle(tini.Get("Invisibles", section, ""));
				Dict.Add(section, tt);
			}
		}

		static public string ReadThemesFromResources()
		{
			// click on ColorThemes in Resources folder and set Build Action: Embeded Resources
			var filename = @"WinFormApp.Resources.ColorThemes.txt";
			Assembly assembly = Assembly.GetExecutingAssembly();

			// read resouces:
			//var str = "";
			//string[] names = assembly.GetManifestResourceNames();
			//ResourceSet set = new ResourceSet(assembly.GetManifestResourceStream(names[3]));
			//foreach (DictionaryEntry resource in set)	
			//	str += $"\n[{resource.Key}] \t{resource.Value}\n";

			var text = string.Empty;
			Stream stream = assembly.GetManifestResourceStream(filename);
			try
			{
				using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
				{
					stream = null;
					text = reader.ReadToEnd();
				}
			}
			finally
			{
				if (stream != null)
					stream.Dispose();
			}
			return text;
		}
	}
}
