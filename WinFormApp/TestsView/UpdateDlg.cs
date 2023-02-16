using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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

            this.Load += UpdateDlg_Load;

            UpdateControls();
        }

        private void UpdateDlg_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Font = new Font("Consolas", 14);

            listView1.Columns.Add("Previous tests");
            listView1.Columns.Add("Current tests");
            listView1.Columns.Add("Expected values");

            listView1.AllowDrop = false;

            listView1.ItemDrag += listView1_ItemDrag;
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragDrop += listView1_DragDrop;

            ResizeListViewColumns(listView1);

            this.CancelButton = btnCancel;
            this.AcceptButton = btnOk;

            this.rbRename.CheckedChanged += DoCheckedChanged;
            this.rbDelete.CheckedChanged += DoCheckedChanged;
            this.chkOnlyDiffs.CheckedChanged += DoCheckedChanged;
            this.chkAllowDrop.CheckedChanged += ChkAllowDrop_CheckedChanged;
        }

        private void UpdateControls()
        {
            ActiveControl = btnOk;
            rbRename.Checked = true;

            // we can only delete old tests
            if (upd.NewListIsEmpty())
            {
                rbRename.Enabled = false;
                rbDelete.Checked = true;
            }            

            chkOnlyDiffs.Checked = true;
      
            DisplayResults();
        }

        
        private void ChkAllowDrop_CheckedChanged(object sender, EventArgs e)
        {
            var chk = chkAllowDrop.Checked;
            chkOnlyDiffs.Checked = false;
            chkOnlyDiffs.Enabled = !chk;
            listView1.AllowDrop = chk;
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            var wrong = false;
            foreach(ListViewItem item in listView1.SelectedItems)
            {
                if (item.Text == "") // no previous test!
                {
                    wrong = true;
                    listView1.SelectedIndices.Remove(item.Index);
                }
            }

            if (wrong)
            {
                MessageBox.Show("Only previous tests can be selected for moving...");
                return;
            }

            listView1.DoDragDrop(listView1.SelectedItems, DragDropEffects.Move);
        }
        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
                e.Effect = DragDropEffects.Move;
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection).ToString(), false))
            {
                ListView.SelectedListViewItemCollection dragItems =
                    (ListView.SelectedListViewItemCollection)e.Data.GetData(
                    typeof(ListView.SelectedListViewItemCollection));

                var indexes = new List<int>();
                foreach (ListViewItem item in dragItems)
                    indexes.Add(item.Index);

                var source = sender as ListView;

                // Get the current mouse location relative to the control being dropped on.
                Point location = source.PointToClient(new Point(e.X, e.Y));
                int dropIndex = -1;

                // Find the item over which the mouse was released.
                for (int index = 0; index < source.Items.Count; index++)
                {
                    if (source.GetItemRect(index).Contains(location))
                    {
                        dropIndex = index;
                        break;
                    }
                }

                if (upd.MoveItems(indexes, dropIndex))
                    DisplayResults();
            }
        }

        private void DoCheckedChanged(object sender, EventArgs e)
        {
            upd.Find(rbRename.Checked);
            DisplayResults();
        }

        private void ResizeListViewColumns(ListView lv)
        {
            foreach (ColumnHeader column in lv.Columns)                
                column.Width = (listView1.Width - 5) / listView1.Columns.Count;            
        }        

        void DisplayResults()
        {
            listView1.Items.Clear();
            for (int i = 0; i < upd.Result.Count; i++)
            {                
                var u = upd.Result[i];

                if (u.IsSameNames() && chkOnlyDiffs.Checked)
                    continue;

                var item = new ListViewItem(u.ToListViewStr());                
                item.Font = u.ForDelete ?
                        new Font("Consolas", 14, FontStyle.Strikeout) :
                        new Font("Consolas", 14, FontStyle.Regular);
                
                listView1.Items.Add(item);
            }
        }
    }
}
