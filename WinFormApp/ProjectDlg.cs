using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WinFormApp
{
    public partial class ProjectDlg : Form
    {
        const string recentProjects = @"..\..\RefsDir\RecentProjects.txt";

        const string FILES_FILTER = "RapidTDD files list (*.rpd)" + "|*.rpd";
        
        private ProjectForm proj = null;

        public ProjectDlg(ProjectForm proj)
        {
            InitializeComponent();
            this.proj = proj;
            FillRecentList();
            MakePopupMenu();
        }

        private void MakePopupMenu()
        {
            ContextMenu cntxMnu = new ContextMenu();
            var item = cntxMnu.MenuItems.Add("Copy full path");
            item.Click += CopyFullPathClick;

            var item2 = cntxMnu.MenuItems.Add("Remove item");
            item2.Click += RemoveItemFormListClick;

            listView1.ContextMenu = cntxMnu;            
        }

        private void CopyFullPathClick(object sender, EventArgs e)
        {
            var full = "";

            if (listView1.SelectedItems.Count > 0)
                full = listView1.SelectedItems[0].SubItems[1].Text;

            if (string.IsNullOrEmpty(full))
                MessageBox.Show("File path is empty.");
            else
                Clipboard.SetText(full);
        }

        private void RemoveItemFormListClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var indx = listView1.SelectedIndices[0];
                listView1.Items.RemoveAt(indx);
                SaveRecentProjects();
            }   
        }

        private void FillRecentList()
        {           
            listView1.Items.Clear();    

            var full = Path.GetFullPath(recentProjects);
            if (File.Exists(full))
            {
                var files = File.ReadAllLines(full);
                foreach (var file in files)
                {
                    if (string.IsNullOrEmpty(file)) continue;
                    AddFileToListView(file);
                }
            }            
        }

        private void AddFileToListView(string file)
        {
            var desc = new string[] { Path.GetFileName(file), file };
            var item = new ListViewItem(desc);
            listView1.Items.Add(item);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void rdbClose_CheckedChanged(object sender, System.EventArgs e)
        {
            rdbNew.Enabled = false;
            rdbOpen.Enabled = false;
            rdbRecent.Enabled = false;
            rdbAdd.Enabled = false;
            rdbRemove.Enabled = false;
            listView1.Enabled = false;
        }

        private SaveFileDialog CreateSaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "Create new project";
            dialog.Filter = FILES_FILTER;
            dialog.DefaultExt = "rpd";
            dialog.AddExtension = false;
            return dialog;
        }

        private OpenFileDialog CreateOpenDialog()
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Title = "Open project";
            openDlg.Multiselect = false;
            openDlg.Filter = FILES_FILTER;
            openDlg.DefaultExt = "rpd";
            openDlg.AddExtension = false;
            return openDlg;
        }        

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (rdbClose.Checked)            
                proj.Close();                                      
            else if (rdbNew.Checked)            
                CreateNew();            
            else if (rdbOpen.Checked)            
                OpenProject();            
            else if (rdbRecent.Checked)            
                LoadRecent();

            AddFileToListView(proj.RpdFull);
            SaveRecentProjects();
            this.Close();
        }

        private void SaveRecentProjects()
        {
            var full = Path.GetFullPath(recentProjects);

            HashSet<string> files = new HashSet<string>();

            foreach (ListViewItem item in listView1.Items)
                files.Add(item.SubItems[1].Text);
            
            File.WriteAllLines(full, files.ToArray());
        }

        private void LoadRecent()
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select one project");
            }
            else
            {
                var file = listView1.SelectedItems[0].SubItems[1].Text;
                proj.LoadExisting(file);
            }
        }

        private void OpenProject()
        {
            OpenFileDialog openDlg = CreateOpenDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
                proj.LoadExisting(openDlg.FileName);
        }

        private void CreateNew()
        {
            SaveFileDialog saveDlg = CreateSaveDialog();
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                if (rdbRemove.Checked)
                    proj.CloseOpenedDocs();

                proj.RpdFull = saveDlg.FileName;
                proj.UpdateFormUI();
                proj.SaveRpdFile();
            }
        }

        private void DisableOptions()
        {
            rdbAdd.Enabled = false;
            rdbRemove.Enabled = false;
            rdbRemove.Checked = true;
        }

        private void rdbNew_CheckedChanged(object sender, System.EventArgs e)
        {
            rdbAdd.Enabled = true;
            rdbRemove.Enabled = true;
            rdbRemove.Checked = true;
        }

        private void rdbOpen_CheckedChanged(object sender, EventArgs e)
        {
            DisableOptions();
        }

        private void rdbRecent_CheckedChanged(object sender, EventArgs e)
        {
            DisableOptions();
            
            // select first item in the list
            if (listView1.SelectedIndices.Count == 0)
                listView1.Items[0].Selected = true;
        }
    }
}
