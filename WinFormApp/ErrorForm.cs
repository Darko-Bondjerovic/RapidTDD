using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;

namespace WinFormApp
{
    public partial class ErrorForm : DockContent
    {
        internal Action<string, int> WhenErrorClick = (s, e) => { };

        public ErrorForm()
        {
            InitializeComponent();
            ErrorsList.MouseDoubleClick += ListView1_MouseDoubleClick;

            ContextMenu cntxMnu = new ContextMenu();
            var item = cntxMnu.MenuItems.Add("Copy text");
            item.Click += CopyItemClick;
            ErrorsList.ContextMenu = cntxMnu;
        }

        private void CopyItemClick(object sender, EventArgs e)
        {
            CopyToCliboard();
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var item = ErrorsList.SelectedItems[0];
            if (item != null && item.Tag != null)
            {
                var data = (Tuple<string, int, string>)item.Tag;
                WhenErrorClick(data.Item1, data.Item2);
            }
        }         

        public void ShowErrors(object errobj)
        {
            if (errobj == null)
                return;

            ErrorsList.Items.Clear();

            var errors = errobj as List<Tuple<string, int, string>>;

            var max = 1900;
            foreach (var e in errors)
            {
                var title = $"[{e.Item1}] [{e.Item2}] {e.Item3}\n";
                using (Graphics graphics = Graphics.FromImage(new Bitmap(1, 1)))
                {
                    SizeF size = graphics.MeasureString(title, ErrorsList.Font);
                    max = Math.Max(max, (int)size.Width);
                }
                
                ErrorsList.Items.Add(new ListViewItem(title) { Tag = e });
            }

            ErrorsList.Columns[0].Width = max;
        }

        private void ErrorsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)            
               CopyToCliboard();            
        }

        private void CopyToCliboard()
        {
            if (ErrorsList.SelectedItems.Count > 0)
            {
                var item = ErrorsList.SelectedItems[0];
                Clipboard.SetText(item.Text);
            }
        }
    }
}
