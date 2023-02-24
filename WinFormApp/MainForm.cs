using FastColoredTextBoxNS;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using WinFormApp.TestsView;

namespace WinFormApp
{
    //Install-Package FCTB -Version 2.16.24
    //Install-Package DockPanelSuite -Version 3.0.6
    //Install-Package DockPanelSuite.ThemeVS2015 -Version 3.0.6

    public partial class MainForm : Form
    {
        const string EXECUTE_INFO = "Press F5 to execute...";

        Font font = EditForm.FONT_FOR_SOURCE;
        int NewFilesCount = 1;
        public TestsForm tstForm = null;
        OutputForm outForm = null;
        ErrorForm errForm = null;
        FInvoker invForm = null;

        public List<EditForm> editors = new List<EditForm>();
        private string ThemeName = "VSDark"; //"Afterglow";

        Worker worker = null;// new Worker();
        private bool run_in_progress = false;
        private bool HadCompileErorrs = false;        

        public MainForm()
        {
            InitializeComponent();

            findF2.Click += FindSymbol_Click;
            findShiftF2.Click += FindSymbol_Click;

            WindowState = FormWindowState.Maximized;
            this.FormClosing += MainForm_FormClosing;

            this.IsMdiContainer = true;
            menuStrip1.MdiWindowListItem = viewsToolStripMenuItem;
            
            RunSplashScreen();
        }

        private void FindSymbol_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem temp = (ToolStripMenuItem)sender;
            if (temp == null)
                return;

            var tag = Convert.ToInt32(temp.Tag);
            FindSimbol(tag);            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // if we close all files, check for test file!
            if (tstForm != null)
                if (editors.Count == 0)
                    tstForm.tstPanel.AskToSaveTestFile();
        }

        private void RunSplashScreen()
        {
            StartSplashInThread();

            SplashScreen.UdpateStatusText("Load themes...");
            FillThemes();

            dockpanel.Theme = new VS2015BlueTheme(); // new VS2015DarkTheme();
            dockpanel.DocumentStyle = DocumentStyle.DockingMdi;

            //In properties or LoadForm, set all 4 anchors;
            //dockpanel.Anchor = Top, Left, Bottom, Right
            SplashScreen.UdpateStatusText("Load views...");
            MakeErrorForm();
            MakeInvokerForm();

            MakeOutputForm();
            MakeTestForm();

            SplashScreen.UdpateStatusText("Load compiler...");
            worker = new Worker();
            worker.WriteInfo += WriteInfo;
                        
            MakeNewEditor();//.InsertEmptyMain();            
            //ExecuteCode();

            // without ExecuteCode this, will crash:
            //SplashScreen.UdpateStatusText("Load completions...");
            //_ = worker.ReadCompletionItems(editors[0].TabName, "word").Result;

            SplashScreen.UdpateStatusText("Done!");
            
            
            this.Show();
            SplashScreen.CloseSplashScreen();

            this.Activate();
        }

        private void StartSplashInThread()
        {
            this.Hide();
            Thread splashthread = new Thread(
                new ThreadStart(SplashScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();
        }

        private void MakeErrorForm()
        {
            if (errForm == null)
            {
                errForm = new ErrorForm();
                errForm.FormClosing += ErrForm_FormClosing;
                errForm.WhenErrorClick = FindPosInSource;
            }
            
            errForm.Show(this.dockpanel, DockState.DockBottomAutoHide);
        }

        private void MakeInvokerForm()
        {
            if (invForm == null)
            {
                invForm = new FInvoker();
                invForm.FormClosing += InvForm_FormClosing;
                invForm.WhenReferenceClick = FindPosInSource;
            }

            invForm.Show(this.dockpanel, DockState.DockBottomAutoHide);
        }

        private void InvForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.invForm = null;
        }

        private void FindPosInSource(string tabname, int location)
        {
            foreach(var edit in editors)
            {
                if (edit.TabName == tabname)
                {
                    edit.Activate();
                    edit.fctb.SelectionStart = location;
                    edit.fctb.DoCaretVisible();
                    edit.fctb.Focus();
                    break;
                }
            }
        }

        private void ErrForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.errForm = null;
        }

        private void FillThemes()
        {
            Themes.Themes.Init();

            themesToolMenu.DropDownItems.Clear();
            foreach (var name in Themes.Themes.Dict.Keys)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)
                    themesToolMenu.DropDownItems.Add(name);

                if (item.Text == this.ThemeName)
                    item.Checked = true;

                item.Click += (o, a) =>
                {
                    this.ThemeName = item.Text;
                    SetThemeToAllOpenEditors();

                    foreach (ToolStripMenuItem i in themesToolMenu.DropDownItems)
                        i.Checked = i.Text == name;
                };
            }
        }
        private void SetThemeToAllOpenEditors()
        {
            foreach (var edt in editors)
                edt.SetTheme(this.ThemeName);
        }

        public EditForm CurrentEditForm
        {
            get
            {
                Form activeMdi = ActiveMdiChild;

                if (activeMdi != null && activeMdi is EditForm)
                {
                    return activeMdi as EditForm;
                }
                return null;
            }
        }

        public EditForm OpenFileInEditor(string fileName)
        {
            var edt = IsFileAlreadyOpen(fileName);

            if (edt == null)
            {
                edt = MakeNewEditor();
                edt.LoadFile(fileName);
            }
            else
                edt.ReloadFile();

            return edt;
        }

        private EditForm IsFileAlreadyOpen(string fileName)
        {
            foreach (EditForm edt in editors)
            {
                if (edt.FileName as string == fileName)
                {
                    this.ActivateMdiChild(edt);
                    return edt;
                }
            }
            return null;
        }

        private void quitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Quit application?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.Close();
        }        

        private void MakeOutputForm()
        {
            if (outForm == null)
            {
                outForm = new OutputForm();
                outForm.FormClosing += OutForm_FormClosing;
            }

            outForm.Show(this.dockpanel, DockState.DockLeft);
        }

        private void OutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            outForm = null;
        }

        private void MakeTestForm()
        {
            if (tstForm == null)
            {
                tstForm = new TestsForm();
                tstForm.WhenFormClosing += TestFormClosing;
            }
            
            tstForm.Show(this.dockpanel, DockState.DockLeft);
        }

        public void TestFormClosing()
        {            
            tstForm = null;
        }        

        private void newToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            MakeNewEditor();
        }

        private EditForm MakeNewEditor()
        {
            var edt = new EditForm();            
            edt.TabName = $"new {NewFilesCount++}";
            editors.Add(edt);
            edt.Show(this.dockpanel, DockState.Document);
            edt.WhenClosingEditor += DoClosingEditor;
            edt.SetTheme(this.ThemeName);            
            MakeCompletonPopup(edt);
            return edt;
        }

        private void DoClosingEditor(EditForm edt)
        {
            if (edt == null)
                return;

            editors.Remove(edt);
            dockpanel.Controls.Remove(edt);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            var openDlg = new OpenFileDialog();
            openDlg.Multiselect = true;
            openDlg.Filter = "C# files (*.cs)|*.cs|All files (*.*)|*.*";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                WriteInfo(EXECUTE_INFO);
                foreach (var file in openDlg.FileNames)
                    OpenFileInEditor(file);
            }
        }

        private void WriteInfo(string info)
        {            
            toolInfo.Text = info;
            toolStrip1.Refresh();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile(bool saveAs = false)
        {
            if (editors.Count == 0)
            {
                ShowInfoMsgBox("Nothing to save...");
                return;
            }

            var curr = CurrentEditForm;
            if (curr != null)
                curr.SaveFile(saveAs);
        }

        private static void ShowInfoMsgBox(string msg)
        {
            MessageBox.Show(msg);
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog == null)
            {
                fontDialog = new FontDialog();
                fontDialog.Font = font;
            }
            if (CurrentEditForm != null && fontDialog.ShowDialog() == DialogResult.OK)
            {
                ChangeFont(fontDialog.Font);
                font = fontDialog.Font;
            }
        }

        private void ChangeFont(Font font)
        {
            if (font.Size <= 4)
                return;

            this.font = font;

            foreach (EditForm tab in editors)
                tab.fctb.Font = font;
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAllFiles();
        }

        public void SaveAllFiles()
        {
            foreach (EditForm frm in editors)
                frm.SaveFile();

            if (tstForm != null)
                SaveTestFile();
        }

        private void toolNew_Click(object sender, EventArgs e)
        {
            MakeNewEditor();
        }

        private void toolOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void toolSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void toolSaveAs_Click(object sender, EventArgs e)
        {
            SaveFile(true);
        }

        private void insertDemoCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            //MakeNewEditor().InsertDemoCode();

            WriteInfo(EXECUTE_INFO);

            if (editors.Count > 0)
                CurrentEditForm.InsertTestCode();
            else
                MakeNewEditor().InsertTestCode();
        }

        private void relaodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editors.Count == 0 || CurrentEditForm.FileName == null)
            {
                ShowInfoMsgBox("No file to reload...");
                return;
            }

            CurrentEditForm.ReloadFile();
        }                

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            new AboutForm().ShowDialog();
        }

        private void toolRun_Click(object sender, EventArgs e)
        {
            ExecuteCode();
        }
        private void executeCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteCode();
        }

        private void SetRunInProgress(bool yes)
        {
            if (yes)
                WriteInfo("Wait...");
            else
                WriteInfo("");
            
            Cursor.Current = yes ? Cursors.WaitCursor : Cursors.Default;
            run_in_progress = yes;
            toolRun.Enabled = !yes;
            executeCodeToolStripMenuItem.Enabled = !yes;
        }

        private void ExecuteCode()
        {
            if (run_in_progress)
                return;

            if (editors.Count == 0)
            {
                ShowInfoMsgBox("Nothing to run, no source code...");
                return;
            }

            if (tstForm == null)
            {
                ShowInfoMsgBox("Tests view should be visible");
                return;
            }
            else
                tstForm.tstPanel.TestsAreChanged = false;


            SetRunInProgress(true);
            try
            {               
                var response = ExecuteSource(GetSources());

                if (response.Contains(Worker.TargetOfInvocation))
                {
                    ShowInfoMsgBox(response);
                    SetRunInProgress(false);
                    return;
                }    

                ShowResponseToUI(response, HadCompileErorrs);
                DisplayErrors(null);
            }
            finally
            {
                SetRunInProgress(false);
            }
        }

        public string ExecuteSource(List<DocInfo> docs)
        {
            WriteInfo("Wait...");
            HadCompileErorrs = false;
            try
            {
                worker.Build(docs);                
                return worker.RunCodeThread(new string[] { });
            }
            catch (Exception e)
            {
                HadCompileErorrs = true;
                var result = e.Message;
                if (e.Data != null)
                {
                    var errors = e.Data["ErrorsInCode"];
                    DisplayErrors(errors);
                }

                return result;
            }
        }

        private void DisplayErrors(object errobj)
        {
            if (errForm != null)
                if (!HadCompileErorrs)
                {
                    errForm.DockState = DockState.DockBottomAutoHide;
                    //errForm.textBox.Text = "";
                }

            if (errobj != null)
            {
                MakeErrorForm();
                errForm.DockState = DockState.DockBottom;
                errForm.ShowErrors(errobj);
            }
        }

        private void ShowResponseToUI(string response, bool err)
        {
            if (err) return;

            if (tstForm != null)
                tstForm.tstPanel.ShowResponseInUI(response);

            if (outForm != null)
                outForm.ShowResponseToUI(response);
        }

        public List<DocInfo> GetSources()
        {
            var list = new List<DocInfo>();
            foreach (var edit in editors)
                if (edit.DoBuild)
                    list.Add(MakeDocInfo(edit));

            return list;
        }

        private static DocInfo MakeDocInfo(EditForm edit)
        {
            return new DocInfo(edit.TabName, edit.fctb.Text);
        }

        private void copyActToExpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tstForm != null)
                tstForm.tstPanel.CopyTextActToExp();
        }

        private void copyAllActToExpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tstForm != null)
                tstForm.tstPanel.CopyAllActToExp();
        }

        private void MakeCompletonPopup(EditForm edt)
        {
            var pop = new AutocompleteMenu(edt.fctb);
            pop.ForeColor = Color.Black;
            pop.BackColor = Color.AntiqueWhite;
            pop.SelectedColor = Color.LightSteelBlue;
            pop.SearchPattern = @"[\w\.]";
            pop.AllowTabKey = false;
            pop.AlwaysShowTooltip = false;
            // must change both values to setup width:
            var width = 240;
            pop.Items.MinimumSize = new Size(width, 250);
            pop.Items.Width = width;
            pop.Font = new Font("Consolas", 14, FontStyle.Regular);

            pop.Items.SetAutocompleteItems(
                new Completions(pop, worker, edt)
                { RunUpdateDocs = UpdateAllDocs });
        }

        public void UpdateAllDocs()
        {
            worker.UpdateDocuments(GetSources());
        }

        private async void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editors.Count == 0)
            {
                ShowInfoMsgBox("Nothing to rename...");
                return;
            }

            var edit = CurrentEditForm;
            if (edit == null)
                return;

            var position = edit.fctb.SelectionStart;

            var word = edit.fctb.SelectedText;
            if (string.IsNullOrEmpty(word))
            {
                ShowInfoMsgBox("Select text/item to rename...");
                return;
            }

            var newName = word;
            if (InputBox.ShowDialog("Confirm", "Rename symbol:",
                ref newName) != DialogResult.OK) return;

            Cursor.Current = Cursors.WaitCursor;

            var vertical = edit.fctb.VerticalScroll.Value;

            UpdateAllDocs();
            var docInfo = MakeDocInfo(edit);
            try
            {
                var news = await worker.RenameSymbol(docInfo, position, newName);
                foreach (var edt in editors)
                {
                    var info = news.FirstOrDefault(x => x.full.Equals(edt.TabName));
                    if (info != null)
                        edt.fctb.Text = info.code;
                }
            }
            finally
            {
                edit.fctb.SelectionStart = position;
                UpdateScroll(edit.fctb, vertical);
                edit.fctb.DoCaretVisible();
                Cursor.Current = Cursors.Default;
            }
        }

        private void UpdateScroll(FastColoredTextBox tb, int vPos)
        {            
            if (vPos <= tb.VerticalScroll.Maximum)
            {
                tb.VerticalScroll.Value = vPos;
                tb.UpdateScrollbars();
            }
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            var edt = CurrentEditForm;
            if (edt != null)
                edt.SplitEditors();            
        }

        private void openTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tstForm != null)
                tstForm.tstPanel.LoadTests();
        }

        public void LoadTestsFromFile(string filename)
        {
            if (tstForm != null)
                tstForm.tstPanel.LoadTestsFromFile(filename);
        }

        private void saveTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTestFile();
        }

        void SaveTestFile()
        {
            if (tstForm != null)
                tstForm.tstPanel.SaveTests();
        }

        private void addReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new RefsForm().ShowDialog() == DialogResult.OK)            
                worker.AddThirdPartyRefs();
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tstForm != null)
                tstForm.tstPanel.SaveTestsAs();
        }

        private void generateMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateCode();
        }

        public async void GenerateCode()
        { 
            if (editors.Count == 0)
            {
                ShowInfoMsgBox("Nothing to generate...");
                return;
            }

            var edit = CurrentEditForm;
            if (edit == null)
                return;

            var input = edit.fctb.SelectedText;
            if (string.IsNullOrEmpty(input) || !input.Contains("("))
            {
                ShowInfoMsgBox($"Select some text with brackets: {input}()");
                return;
            }

            var ana = new Analyzer();
            ana.Run(input);

            UpdateAllDocs();
            
            var position = edit.fctb.SelectionStart;
            try
            {
                var docInfo = MakeDocInfo(edit);

                var news = await worker.GenerateMethod(ana, docInfo, position);
                
                if (news == null)
                {
                    var code = ana.GenerateClass();
                    if (code != "")                    
                        MakeNewEditor().fctb.Text = code;

                    return;
                }

                foreach (var edt in editors)
                {
                    var info = news.FirstOrDefault(n => n.full.Equals(edt.TabName));
                    if (info != null)
                        edt.fctb.Text = info.code;
                }
            }
            finally
            {
                edit.fctb.SelectionStart = position;
                edit.fctb.DoCaretVisible();
                Cursor.Current = Cursors.Default;
            }         
        }

        private void displayTestsViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeTestForm();
        }

        private void displayOutputViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeOutputForm();
        }

        private void displayFilesViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tsfn = tstForm == null ? "" : tstForm.tstPanel.TestsFileName;
            FilesForm.ShowDialog(this, editors, tsfn);                
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editors.Count == 0)
            {
                MessageBox.Show("Nothing to close...");
                return;
            }

            if (MessageBox.Show("Close all documents?", "Confirm",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            CloseAllDocumentsAndTestFile();
        }

        public bool IsAnythingToSave()
        {
            if (editors.Count == 0) 
                return false;

            foreach (var e in editors)            
                if (e.fctb.Text != "")                
                    return true;

            return false;
        }

        public void CloseAllDocumentsAndTestFile()
        {
            for (int i = editors.Count-1; i >= 0; i--)                
                editors[i].Close();           

            editors.Clear();

            if (tstForm != null)
            {
                tstForm.tstPanel.AskToSaveTestFile();
                tstForm.tstPanel.UnloadTests();
            }
        }

        public void CloseEmptyDocuments()
        {
            for (int i = editors.Count-1; i >= 0; i--)
            {
                if (editors[i].fctb.Text == "")                
                    editors[i].Close();                    
            }
        }

        private void linesNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var edt = CurrentEditForm;
            if (edt != null)
                edt.fctb.ShowLineNumbers = !edt.fctb.ShowLineNumbers;            
        }

        private void unloadTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tstForm != null)
                tstForm.tstPanel.UnloadTests();
        }

        private void keepOLDTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tstForm != null)
                tstForm.tstPanel.treeView.KeepOldTests =
                    keepOLDTestsToolStripMenuItem.Checked;
        }


        private async void FindSimbol(int tag)
        {
            if (editors.Count == 0)
            {
                ShowInfoMsgBox("No source code...");
                return;
            }

            var edit = CurrentEditForm;
            if (edit == null)
                return;

            var position = edit.fctb.SelectionStart;

            var word = edit.fctb.SelectedText;
            if (string.IsNullOrEmpty(word))
            {
                ShowInfoMsgBox("Select symbol first...");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            UpdateAllDocs();            

            var docInfo = MakeDocInfo(edit);
            try
            {
                var list = await worker.FindSymbolDefinition(docInfo, position, tag);

                Cursor.Current = Cursors.Default;

                if (list == null)
                    return;

                if (tag == 1)
                {
                    if (list.Count > 0)
                        FindPosInSource(list[0].Item1, list[0].Item2);
                }
                else
                {
                    DisplayInvokerForm(list, word);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DisplayInvokerForm(List<Tuple<string, int, string>> list, string name)
        {
            if (list != null)
            {
                MakeInvokerForm();
                invForm.Text = $"[{name} references]";                
                invForm.DockState = DockState.DockBottom;
                invForm.DisplayReferences(list);
            }
        }
    }
}