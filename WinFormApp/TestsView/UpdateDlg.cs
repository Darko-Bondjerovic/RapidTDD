using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace DiffNamespace
{
    public partial class UpdateDlg : Form
    {
        UpdateExp upd = null;

        public UpdateDlg(UpdateExp upd)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;

            this.upd = upd;

            UpdateControls();
        }

        private void UpdateControls()
        {
            ActiveControl = btnOk;
            rbRename.Checked = true;

            // we can only delete old tests
            if (upd.GetNewCount() == 0)
            {
                rbRename.Enabled = false;
                rbDelete.Checked = true;
            }            

            chkOnlyDiffs.Checked = true;

            DisplayResults();
        }

        private void NewOldTestsDlg_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Font = new Font("Consolas", 14);

            listView1.Columns.Add("Previous tests");
            listView1.Columns.Add("Current tests");
            listView1.Columns.Add("Expected values");
            
            ResizeListViewColumns(listView1);

            this.CancelButton = btnCancel;
            this.AcceptButton = btnOk;         

            this.rbRename.CheckedChanged += DoCheckedChanged;
            this.rbDelete.CheckedChanged += DoCheckedChanged;
            this.chkOnlyDiffs.CheckedChanged += DoCheckedChanged;
        }        

        private void DoCheckedChanged(object sender, EventArgs e)
        {
            DisplayResults();
        }

        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)
            {
                column.Width = (listView1.Width - 5) / listView1.Columns.Count;
            }
        }        

        void DisplayResults()
        {
            upd.Find(rbRename.Checked);

            var newListCount = upd.GetNewCount();
            
            listView1.Items.Clear();
            for (int i = 0; i < upd.result.Count; i++)
            {                
                var u = upd.result[i];

                if (u.IsSame && chkOnlyDiffs.Checked)
                    continue;                    

                var item = new ListViewItem(
                    new[] { u.oldName, u.newName, u.expected });

                if (i >= newListCount)
                    item.Font = new Font("Consolas", 14, FontStyle.Strikeout);
                
                listView1.Items.Add(item);
            }
        }
    }
}
