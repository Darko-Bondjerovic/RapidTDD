using DiffNamespace;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;
using WinFormApp.TestsView;

namespace WinFormApp
{
    public partial class ProjectForm : DockContent
    {
        public Action WhenClosingForm = () => { };

        class FileData
        {
            public bool doBuild = true;
            public bool isHidden = false;
            public string fileName = "";
        }        

        static int IsHidenColumnIndex = 1;
        
        private MainForm main = null;
        private TestsForm tstf = null;

        private string rpdFull = "";
        private string rpdPath = "";

        public string RpdFull
        {
            get
            {
                return rpdFull;
            }
            set
            {
                rpdFull = value;
                this.Text = Path.GetFileName(rpdFull);
                rpdPath = Path.GetDirectoryName(rpdFull);                
            }
        }        

        public ProjectForm(MainForm main, TestsForm tstf)
        {
            InitializeComponent();

            this.main = main;
            this.tstf = tstf;

            this.CloseButtonVisible = false;            
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;

            SetListViewUI();
            MakePopupMenu();
        }

        #region ListViewPart        


        private void SetListViewUI()
        {
            listView1.ColsNames = new string[] { "Build", "Show" };
            listView1.WhenColumnClick = DoWhenColumnClick;
            listView1.WhenMouseDown += DoWhenMouseDown;            
        }
        private void DoWhenColumnClick(ColumnClickEventArgs e, bool newValue)
        {
            if (rpdFull == "")
                return;

            foreach (var edt in main.editors)
            {
                if (e.Column == IsHidenColumnIndex)
                    edt.IsHidden = !newValue;
                else
                    edt.DoBuild = newValue;
            }
        }

        private void DoWhenMouseDown(int row, int col)
        {
            var edt = main.editors[row];

            if (col == IsHidenColumnIndex)
                edt.IsHidden = !edt.IsHidden;
            else
                edt.DoBuild = !edt.DoBuild;
        }

        private void MakePopupMenu()
        {
            ContextMenu cntxMnu = new ContextMenu();
            var item = cntxMnu.MenuItems.Add("Copy full path");
            item.Click += CopyFullPathClick;

            var item2 = cntxMnu.MenuItems.Add("Remove item");
            item2.Click += RemoveItemFormListClick;

            listView1.ContextMenu = cntxMnu;
            pnlTestFile.ContextMenu = cntxMnu;
        }

        private void RemoveItemFormListClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Remove item, are you sure?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            Control source = GetSourceControl(sender);

            if (source is ListView)
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    var indx = listView1.SelectedIndices[0];

                    if (main.editors.Count > indx)
                    {
                        var edt = main.editors[indx];
                        edt.HideOnClose = false;
                        edt.Close();
                        main.RemoveEditor(edt);

                        UpdateFormUI();
                    }
                }
            }

            if (source is TextBox)
            {
                tstf.UnloadTestFile();
                this.pnlTestFile.Text = "";
            }
        }

        private static Control GetSourceControl(object sender)
        {
            var menu = sender as MenuItem;
            var parent = menu.Parent as ContextMenu;
            var source = parent.SourceControl;
            return source;
        }        

        private void CopyFullPathClick(object sender, EventArgs e)
        {
            Control source = GetSourceControl(sender);

            var full = "";

            if (source is ListView)
                if (listView1.SelectedItems.Count > 0)
                    full = listView1.SelectedItems[0].SubItems[3].Text;

            if (source is TextBox)
                full = tstf.tstPanel.TestsFileName;

            if (string.IsNullOrEmpty(full))
                MessageBox.Show("File path is empty.");
            else
                Clipboard.SetText(full);
        }

        #endregion ListViewPart        

        private void OpenFilesFromXml(XDocument xdoc)
        {
            var result = xdoc.Root.Descendants("File")
                .Select(o =>
                    new FileData()
                    { 
                        doBuild = Convert.ToBoolean(o.Element("make").Value),
                        isHidden = !Convert.ToBoolean(o.Element("show").Value),
                        fileName = MakeFullPath(rpdPath, o.Element("full").Value)
                    }).ToList();

            foreach (var f in result)
            {
                var edt = main.OpenFileInEditor(f.fileName);

                if (edt != null)
                {
                    edt.DoBuild = f.doBuild;
                    edt.IsHidden = f.isHidden;
                }
            }
        }

        private void LoadTestFile(XDocument xdoc)
        {
            var file = xdoc.Root.Element("Test").Value as string;

            if (String.IsNullOrEmpty(file))
                return;

            if (!File.Exists(file))
                file = MakeFullPath(rpdPath, file);

            if (!string.IsNullOrEmpty(file))
                tstf.tstPanel.LoadTestsFromFile(file);
        }

        public static string MakeFullPath(string root, string relative)
        {
            return Path.GetFullPath(Path.Combine(root, relative));
        }

        public static String MakeRelativePath(String root, String full)
        {
            //https://stackoverflow.com/questions/275689/how-to-get-relative-path-from-absolute-path

            if (string.IsNullOrEmpty(full))
                return "";

            var ch = Path.DirectorySeparatorChar;
            root = root.TrimEnd(ch) + ch;

            Uri fromUri = new Uri(root);
            Uri toUri = new Uri(full);

            if (fromUri.Scheme != toUri.Scheme) 
                return full;  // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            return relativePath;
        }

        public void SaveRpdFile()
        {
            var xdoc = new XDocument(
                new XElement("RapidTDD"));

            var testFN = tstf.tstPanel.TestsFileName;
            var testXml = new XElement("Test", MakeRelativePath(rpdPath, testFN));
            xdoc.Root.Add(testXml);
            
            var filesXml = new XElement("Files");
            foreach (var o in main.editors)
            {                
                var fileXml = new XElement(
                    new XElement("File",
                    new XElement("make", o.DoBuild),
                    new XElement("show", !o.IsHidden),
                    new XElement("full", MakeRelativePath(
                            rpdPath, o.FileName as string))));

                 filesXml.Add(fileXml);
            }
            xdoc.Root.Add(filesXml);            

            xdoc.Save(RpdFull);
        }        

        private void ProjectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WhenClosingForm();
        }

        private void OpenRpdFileAndLoadData()
        {
            var xdoc = XDocument.Load(RpdFull);

            try
            {
                LoadTestFile(xdoc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load test file, error:\n" + ex.Message);
            }

            OpenFilesFromXml(xdoc);

            UpdateFormUI();
        }

        public void UpdateFormUI()
        {
            listView1.Items.Clear();

            foreach(var e in main.editors)
            {
                e.HideOnClose = true;

                var row = new ListViewItem(new[] {
                    TwoColsListView.chk(e.DoBuild),
                    TwoColsListView.chk(!e.IsHidden), 
                    e.TabName, e.FileName as string});

                listView1.Items.Add(row);
            }
                        
            this.pnlTestFile.Text = "Test file: " + 
                Path.GetFileName(tstf.tstPanel.TestsFileName);
        }

        public void CloseOpenedDocs()
        {
            main.CloseAllDocumentsAndTestFile();
        }

        internal void LoadExisting(string rpdfile)
        {
            if (!File.Exists(rpdfile))            
            {
                MessageBox.Show($"Project file does not exists:\n{rpdfile}");
                return;
            }

            this.RpdFull = rpdfile;            
            CloseOpenedDocs();     
            OpenRpdFileAndLoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveRpdFile();
        }
    }
}
