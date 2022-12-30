using System;
using System.Collections.Generic;
using System.Linq;
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

        const string FILES_FILTER = "RapidTDD files list (*.rpd)" + "|*.rpd|" +
                "All files (*.*)" + "|*.*";

        public static string chk(bool state)
        {
            // https://www.compart.com/en/unicode
            return state ? "☒" : "☐"; // U+2612/U+2610
        }
                
        static int ShowColIndx = 1;

        List<EditForm> editors = null;
        string testFileName = "";
        private MainForm mainForm = null;

        public FilesForm(MainForm main, List<EditForm> editors, string testFileName = "")
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.listView1.View = View.Details;

            for(int i=0; i<2; i++)
                this.listView1.Columns[i].Tag = true;            
            
            SetColumnsCaptions();

            this.listView1.ColumnClick += ListView1_ColumnClick;
            this.listView1.MouseDown += ListView1_MouseDown;

            this.mainForm = main;

            SetTestFileName(testFileName);

            ShowFilesList(editors);
        }

        private void ShowFilesList(List<EditForm> editors)
        {
            this.listView1.Items.Clear();

            this.editors = editors;
            foreach (var e in editors)
            {
                var item = new ListViewItem(new[] {
                    chk(e.DoBuild), chk(!e.IsHidden), e.TabName, e.FileName as string });
                listView1.Items.Add(item);
            }
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

        private OpenFileDialog CreateOpenDialog()
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Multiselect = false;
            openDlg.Filter = FILES_FILTER;
            openDlg.DefaultExt = "rpd";
            openDlg.AddExtension = false;
            return openDlg;
        }

        private void btnLoadFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = CreateOpenDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                this.Text = openDlg.FileName;

                var xdoc = XDocument.Load(openDlg.FileName);

                SetTestFileName(xdoc.Root.Element("Test").Value as string);
                if (!string.IsNullOrEmpty(testFileName))
                    mainForm.LoadTestsFromFile(testFileName);

                List<FileData> files = xdoc.Root.Descendants("File")
                    .Select(o =>
                        new FileData
                        {
                            doBuild = Convert.ToBoolean(o.Element("make").Value),
                            isHidden = !Convert.ToBoolean(o.Element("show").Value),
                            fullName = o.Element("full").Value
                        }).ToList();

                foreach (var file in files)
                {
                    var editForm = mainForm.OpenFileInEditor(file.fullName);
                    editForm.DoBuild = file.doBuild;
                    editForm.IsHidden = file.isHidden;
                }

                ShowFilesList(mainForm.editors);
            }
        }

        private void btnSaveFiles_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = CreateSaveDialog();
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                this.mainForm.SaveAllFiles();

                if (mainForm.tstForm != null)
                    SetTestFileName(mainForm.tstForm.tstPanel.TestsFileName);

                var xdoc = new XDocument(
                    new XElement("RapidTDD",
                        new XElement("Test", this.testFileName),                            
                        new XElement("Files",
                            editors.Select(o =>
                                new XElement("File",
                                new XElement("make", o.DoBuild),
                                new XElement("show", !o.IsHidden),
                                new XElement("full", o.FileName as string)
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
    }
}
