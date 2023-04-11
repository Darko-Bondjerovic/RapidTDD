using FastColoredTextBoxNS;
using Microsoft.CodeAnalysis.Differencing;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using WinFormApp.Themes;

namespace WinFormApp
{
    public partial class EditForm : DockContent
    {
        public static Font FONT_FOR_SOURCE =
            new Font("Consolas", 18, FontStyle.Regular);

        public Action<EditForm> WhenClosingEditor = edt => { };

        public Action<EditForm> WhenHiddingEditor = edt => { };

        public Action<EditForm> WhenActivated = edt => { };

        public object FileName { get; internal set; } = null;
        public string TabName
        {
            get { return Text; }
            set { Text = value; }
        }

        public bool DoBuild { get; internal set; } = true;        

        private bool SplitEdit = true;

        public EditForm()
        {
            InitializeComponent();

            SetStyleAndCSharp(fctb);
            SetStyleAndCSharp(second);
            SplitEditors();

            FormClosing += EditForm_FormClosing;
            VisibleChanged += EditForm_VisibleChanged;
            this.Activated += EditForm_Activated;


            // needed for code coverage
            fctb.AllowSeveralTextStyleDrawing = true;
        }

        private void EditForm_Activated(object sender, EventArgs e)
        {            
            WhenActivated(this);
        }

        Style redstyle = new TextStyle(Brushes.LightYellow, Brushes.Tomato, FontStyle.Regular);

        Style GreenStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);

       

        private void EditForm_VisibleChanged(object sender, EventArgs e)
        {
            WhenHiddingEditor(this);
        }

        private void ConnectTwoEditors()
        {
            second.SourceTextBox = fctb;
        }

        public void SplitEditors()
        {
            SplitEdit = !SplitEdit;

            if (SplitEdit)
            {
                ConnectTwoEditors();
                split.Panel2Collapsed = false;
                split.Panel2.Show();
            }
            else
            {
                split.Panel2Collapsed = true;
                split.Panel2.Hide();
            }
        }

        private void SetStyleAndCSharp(FastColoredTextBox box)
        {
            Style classNameStyle = new TextStyle(Brushes.Black,
                    null, FontStyle.Regular);
            
            Style sameWordsStyle = new MarkerStyle(
                new SolidBrush(Color.FromArgb(50, Color.Gray)));
                        
            box.BorderStyle = BorderStyle.Fixed3D;
            //box.LeftPadding = 17; // draw vertical line on letft side in edittextbox
            box.Language = Language.CSharp;
            box.AddStyle(sameWordsStyle);
            box.SyntaxHighlighter.ClassNameStyle = classNameStyle;
            try
            {
                box.Font = FONT_FOR_SOURCE;
            }
            catch
            {
                // brak with message Parameter is not valid
                // GC collect font and clear?! :( Set again:
                box.Font = new Font("Consolas", 16, FontStyle.Regular);
            }

            // special characters?
            //result.ImeMode = true;

            // this prevent text to put spaces after = e.g. var x = 3;
            box.AutoIndentChars = false;

            //box.KeyDown += textboxKeyDown;            
        }

        internal void InsertEmptyMain()
        {
            this.fctb.Text =
@"using System;

public class Program 
{ 
    static public void Main(string[] args) 
    { 
        Console.WriteLine(""Hi!"");
    } 
}";

            this.fctb.IsChanged = false;
        }

        //private void textboxKeyDown(object sender, KeyEventArgs e)
        //{
        //    // Alt + Enter
        //    //var fctbox = (sender as FastColoredTextBoxNS);
        //    //if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.Enter)                        
        //    //    fctbox.GenerateCode(ReadSources(),
        //    //        fctbox.SelectedText);

        //    //if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Enter)
        //    //    fctbox.GenerateVariable(ReadSources(),
        //    //        fctbox.SelectedText);
        //}

        

        private void EditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AskToSaveBeforeClose())
                WhenClosingEditor(this);
            else
                e.Cancel = true;
        }

        private bool AskToSaveBeforeClose()
        {
            if (fctb.Text == "")
                return true;

            if (fctb.IsChanged)
            {
                switch (MessageBox.Show(
                        $"Save changes to file: {TabName} ?\n\n" +
                        "Yes - save changes\n" +
                        "No - discard changes\n" +
                        "Cancel - cancel close", "Confirm",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Information))
                {
                    case DialogResult.Yes:
                        this.SaveFile();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return false;
                }
            }

            return true;
        }

        public void LoadFile(string fileName)
        {
            this.FileName = fileName;
            this.TabName = Path.GetFileName(fileName);
            if (!string.IsNullOrEmpty(fileName))
            {
                this.fctb.OpenFile(fileName);
                ConnectTwoEditors();
            }
        }

        internal void ReloadFile()
        {
            if (MessageBox.Show("Reloads the current file from disk, \n" 
                +"losing all changes since saving? \n\n"
                +$"{TabName}", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No) return;

            this.fctb.OpenFile(this.FileName as string);
            ConnectTwoEditors();
        }

        internal void SetTheme(string themeName)
        {
            Themes.Themes.SetTheme(fctb, themeName);
            Themes.Themes.SetTheme(second, themeName);            
        }

        public bool SaveFile(bool saveAs = false)
        {
            SaveFileDialog saveDlg = CreateSaveDialog();

            if (FileName == null || saveAs)
            {
                saveDlg.FileName = this.FileName == null ? TabName :
                        Path.GetFileName(FileName as string);

                if (saveDlg.ShowDialog() != DialogResult.OK)
                    return false;

                this.TabName = Path.GetFileName(saveDlg.FileName);
                this.FileName = saveDlg.FileName;
            }

            try
            {
                File.WriteAllText(FileName as string, fctb.Text);                
                fctb.IsChanged = false;                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private SaveFileDialog CreateSaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = 
                "C# source file (*.cs)" + "|*.cs|"
                + "Normal text file (*.txt)|*.txt|"
                + "All files (*.*)" + "|*.*";

            dialog.DefaultExt = "cs";
            dialog.AddExtension = true;

            return dialog;
        }

//        internal void InsertDemoCode()
//        {
//            this.fctb.Text =
//@"using System;

//public class Program
//{
//    static public void Main(string[] args)
//    {   
//        Print(""Press F5 to execute tests:\n"");

//        Assert(""text"", ""text"");
//        Assert(""when?"", ""what?"");
//    }

//    static void Assert(string exp, string act)
//    {
//        if (act.Equals(exp))
//            Print($""PASS [{exp}]==[{act}]"");
//        else
//            Print($""FAIL [{exp}]!=[{act}]"");
//    }

//    static void Print(string str = """")
//    {
//        Console.WriteLine(str);
//    }
//}";
//}

        internal void InsertTestCode()
        {
            this.fctb.Text =
@"using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
//using System.Windows.Forms;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        //Console.WriteLine($"" |{ ""Left"",-7}|{ ""Right"",7}| "");

        //var form = new Form();
        //form.ShowDialog();

        new Tests().Execute();
    }
}

public class Tests
{
    public void Execute()
    {
        Print($""[GROUP]"");

        Print(""[TEST] First test"");
        Print(""Result for first test"") ; //<-- actual result
        Print(""[EXPC]Result for first test"") ; //<-- expected result

        Print(""[TEST] Second test"");        

        Print($""[GROUP] {GetType().Name}"");

        for(int i=1; i<5; i++)
            RunTest(i);
    }
    
    void RunTest(int input)
    {
        Print($""[TEST] Test {input}"");
        
        Print($""{input}"");
    }

    static void Print(string str = """")
    {
        Console.WriteLine(str);
    }
}";

            this.fctb.IsChanged = false;

        }
    }
}
