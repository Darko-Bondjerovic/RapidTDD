using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;

namespace WinFormApp
{
    public partial class ErrorForm : DockContent
    {
        internal Action<Jump> WhenErrorClick = (o) => { };

        public ErrorForm()
        {
            InitializeComponent();           
            ErrorsListView.Click += ErrorsListView_Click;

            ErrorsListView.Columns.Add(new ColumnHeader() { Width = 1900 });
            ErrorsListView.HeaderStyle = ColumnHeaderStyle.None;

            ContextMenu cntxMnu = new ContextMenu();
            var item = cntxMnu.MenuItems.Add("Copy text");
            item.Click += CopyItemClick;
            ErrorsListView.ContextMenu = cntxMnu;
        }

        private void ErrorsListView_Click(object sender, EventArgs e)
        {
            var item = ErrorsListView.SelectedItems[0];
            if (item != null && item.Tag != null)
                WhenErrorClick((Jump)item.Tag);            
        }

        private void CopyItemClick(object sender, EventArgs e)
        {
            CopyToCliboard();
        }      

        public void ShowErrors(object errobj)
        {
            if (errobj == null)
                return;

            ErrorsListView.Items.Clear();

            var errors = errobj as List<Jump>;

            var max = 1900;
            foreach (var e in errors)
            {
                var title = $"[{e.File}] [{e.Spot}] {e.Desc}";
                using (Graphics graphics = Graphics.FromImage(new Bitmap(1, 1)))
                {
                    SizeF size = graphics.MeasureString(title, ErrorsListView.Font);
                    max = Math.Max(max, (int)size.Width);
                }
                
                ErrorsListView.Items.Add(new ListViewItem(title) { Tag = e });
            }

            ErrorsListView.Columns[0].Width = max;
        }

        private void ErrorsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)            
               CopyToCliboard();            
        }

        private void CopyToCliboard()
        {
            if (ErrorsListView.SelectedItems.Count > 0)
            {
                var item = ErrorsListView.SelectedItems[0];
                Clipboard.SetText(item.Text);
            }
        }
    }
}
