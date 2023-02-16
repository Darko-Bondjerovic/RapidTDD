using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WinFormApp
{
    public partial class FilesForm : Form
    {
        class FileData
        {
            public bool doBuild = true;
            public bool isHidden = false;
            public string fullName = "";
        }

        const string FILES_FILTER = "RapidTDD files list (*.rpd)" + "|*.rpd";
        
        const string SOURCES_FILTER = "C# file (*.cs)" + "|*.cs";


        public static string chk(bool state)
        {
            // https://www.compart.com/en/unicode
            return state ? "☒" : "☐"; // U+2612/U+2610
        }
                
        static int ShowColIndx = 1;

        List<EditForm> editors = null;
        string testFileName = "";
        private MainForm mainForm = null;

        public string root { get; private set; }

        public FilesForm(MainForm main, List<EditForm> editors, string testFileName = "")
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.listView1.View = View.Details;
            this.listView1.FullRowSelect = true;

            for(int i=0; i<2; i++)
                this.listView1.Columns[i].Tag = true;            
            
            SetColumnsCaptions();

            this.listView1.ColumnClick += ListView1_ColumnClick;
            this.listView1.MouseDown += ListView1_MouseDown;

            this.mainForm = main;

            SetTestFileName(testFileName);

            ShowFilesInUI(editors);
        }

        private void ShowFilesInUI(List<EditForm> editors)
        {
            this.listView1.Items.Clear();

            this.editors = editors;
            foreach (var e in editors)            
                AddEditorToUI(e);            
        }

        private void AddEditorToUI(EditForm e)
        {
            var filename = e.FileName as string;
            var row = new ListViewItem(new[] {
                chk(e.DoBuild), chk(!e.IsHidden), e.TabName, filename});

            var list = new List<string>();
            foreach (ListViewItem item in listView1.Items)
                list.Add(item.SubItems[3].Text);

            if (!list.Contains(filename))        
                listView1.Items.Add(row);
        }

        private void SetTestFileName(string testFileName)
        {
            this.testFileName = testFileName;
            this.txtPaths.Text = "TEST FILE: " + testFileName;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // when user press ESC button, close this dialog
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ListView1_MouseDown(object sender, MouseEventArgs e)
        {
            var info = listView1.HitTest(e.X, e.Y);
            var row = info.Item.Index;
            var col = info.Item.SubItems.IndexOf(info.SubItem);
            var text = info.Item.SubItems[col].Text;
            
            if (col >= 2) // other columns
                return;            

            text = (text == chk(true)) ? chk(false) : chk(true);
            listView1.Items[row].SubItems[col].Text = text;

            if (col == ShowColIndx)
                editors[row].IsHidden = !editors[row].IsHidden;
            else
                editors[row].DoBuild = !editors[row].DoBuild;
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column >= 2)
                return;

            var newValue = ChangeColChecked(e);

            foreach (ListViewItem item in listView1.Items)            
                item.SubItems[e.Column].Text = chk(newValue);

            foreach (var edit in editors)
                if (e.Column == ShowColIndx)
                    edit.IsHidden = !newValue;
                else
                    edit.DoBuild = newValue;
        }

        private void SetColumnsCaptions()
        {
            var cols = new string[] { "Build", "Show" };

            for (int i = 0; i < 2; i++)
            {
                var value = Convert.ToBoolean(listView1.Columns[i].Tag);
                listView1.Columns[i].Text = $"{chk(value)} {cols[i]}";
            }
        }

        private bool ChangeColChecked(ColumnClickEventArgs e)
        {
            var column = listView1.Columns[e.Column];
            var newValue = !Convert.ToBoolean(column.Tag);
            column.Tag = newValue;

            SetColumnsCaptions();

            return newValue;
        }

        public static void ShowDialog(MainForm main, List<EditForm> editors, string tsfn = "")
        {
            using (var frm = new FilesForm(main, editors, tsfn))            
               frm.ShowDialog();            
        }

        private OpenFileDialog CreateOpenDialog(bool rpd = true)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Multiselect = false;
            if (rpd)
            {
                openDlg.Filter = FILES_FILTER;
                openDlg.DefaultExt = "rpd";
            }   
            else
            {
                openDlg.Filter = SOURCES_FILTER;
                openDlg.DefaultExt = "cs";
            }

            openDlg.Filter += "|All files (*.*)" + "|*.*";

            openDlg.AddExtension = false;
            return openDlg;
        }

        private void btnLoadFiles_Click(object sender, EventArgs e)
        {
            if (mainForm.IsAnythingToSave())
            {
                switch (MessageBox.Show(
                            $"Close all documents?\n\n" +
                            "Yes - close all open documents\n" +
                            "No - don't close documents, load rpd file\n" +
                            "Cancel - just close this dialog", "Confirm",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Information))
                {
                    case DialogResult.Yes:
                        mainForm.CloseAllDocumentsAndTestFile();                        
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }

            mainForm.CloseEmptyDocuments();

            OpenFileDialog openDlg = CreateOpenDialog();

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                this.Text = openDlg.FileName;

                this.root = Path.GetDirectoryName(openDlg.FileName); //rpd file
                var xdoc = XDocument.Load(openDlg.FileName);

                LoadTestFile(xdoc);
                OpenFilesInEditor(GetFilesList(xdoc));
                ShowFilesInUI(mainForm.editors);
            }
        }

        public static string MakeFullPath(string root, string relative)
        {
            return Path.GetFullPath(Path.Combine(root, relative));
        }

        private void OpenFilesInEditor(List<FileData> files)
        {
            foreach (var file in files)
            {
                var full = file.fullName;
                if (!File.Exists(full))
                    full = MakeFullPath(root, file.fullName);

                var editForm = mainForm.OpenFileInEditor(full);
                editForm.DoBuild = file.doBuild;
                editForm.IsHidden = file.isHidden;
            }
        }

        private List<FileData> GetFilesList(XDocument xdoc)
        {
            return xdoc.Root.Descendants("File")
                .Select(o =>
                    new FileData
                    {
                        doBuild = Convert.ToBoolean(o.Element("make").Value),
                        isHidden = !Convert.ToBoolean(o.Element("show").Value),
                        fullName = MakeFullPath(root, o.Element("full").Value),
                    }).ToList();
        }

        private void LoadTestFile(XDocument xdoc)
        {
            var testFull = xdoc.Root.Element("Test").Value as string;

            if (String.IsNullOrEmpty(testFull))
                return;

            if (!File.Exists(testFull))
                testFull = MakeFullPath(root, testFull);

            SetTestFileName(testFull);

            if (!string.IsNullOrEmpty(testFileName))
                mainForm.LoadTestsFromFile(testFileName);
        }


        public static String MakeRelativePath(String root, String full)
        {
            //https://stackoverflow.com/questions/275689/how-to-get-relative-path-from-absolute-path

            if (String.IsNullOrEmpty(root)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(full)) return "";

            var ch = Path.DirectorySeparatorChar;
            root = root.TrimEnd(ch) + ch;

            Uri fromUri = new Uri(root);
            Uri toUri = new Uri(full);

            if (fromUri.Scheme != toUri.Scheme) { return full; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            return relativePath;
        }

        private void btnSaveFiles_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = CreateSaveDialog();
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                root = Path.GetDirectoryName(saveDlg.FileName);

                this.mainForm.SaveAllFiles();

                if (mainForm.tstForm != null)
                    SetTestFileName(mainForm.tstForm.tstPanel.TestsFileName);

                var xdoc = new XDocument(
                    new XElement("RapidTDD",
                        new XElement("Test", MakeRelativePath(root, testFileName)),                            
                        new XElement("Files",
                            editors.Select(o =>
                                new XElement("File",
                                new XElement("make", o.DoBuild),
                                new XElement("show", !o.IsHidden),
                                new XElement("full", 
                                    MakeRelativePath(root, 
                                    o.FileName as string))
                            )))));

                xdoc.Save(saveDlg.FileName);
            }
        }

        private SaveFileDialog CreateSaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = FILES_FILTER; 
            dialog.DefaultExt = "rpd";
            dialog.AddExtension = false;

            return dialog;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = CreateOpenDialog(false);
            if (openDlg.ShowDialog() == DialogResult.OK)
                AddEditorToUI(mainForm.OpenFileInEditor(openDlg.FileName));            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                if (DialogResult.Yes == MessageBox.Show("Remove selected item(s)?", 
                    "Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    for (int i = listView1.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        var indx = listView1.SelectedIndices[i];
                        mainForm.editors[indx].Close();
                        listView1.Items.RemoveAt(indx);
                    }
                }
            }
         }
    }
}
