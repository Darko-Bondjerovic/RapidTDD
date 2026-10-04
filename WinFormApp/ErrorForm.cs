using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;
using System.Text;
using System.IO;

namespace WinFormApp
{
    public partial class ErrorForm : DockContent
    {
        internal Action<Jump> WhenErrorClick = (o) => { };

        public ErrorForm()
        {
            InitializeComponent();
            // Klik i dupli klik - oba rade Jump
            ErrorsListView.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) JumpToSelected(); };
            ErrorsListView.DoubleClick += (s, e) => JumpToSelected();
            ErrorsListView.Columns.Clear();
            ErrorsListView.Columns.Add(new ColumnHeader() { Width = 2000, Text = "Error" });
            ErrorsListView.HeaderStyle = ColumnHeaderStyle.None;
            ErrorsListView.View = View.Details;
            ErrorsListView.FullRowSelect = true;
            ErrorsListView.HideSelection = false;
            var cntxMnu = new ContextMenuStrip();
            var jumpItem = cntxMnu.Items.Add("Jump to error");
            jumpItem.Font = new Font(cntxMnu.Font, FontStyle.Bold);
            jumpItem.Click += (s, e) => JumpToSelected();
            cntxMnu.Items.Add(new ToolStripSeparator());
            cntxMnu.Items.Add("Copy text", null, (s, e) => CopyToCliboard());
            cntxMnu.Items.Add("Copy all text", null, (s, e) => CopyAllToClipboard());
            ErrorsListView.ContextMenuStrip = cntxMnu;
        }
        private void JumpToSelected()
        {
            if (ErrorsListView.SelectedItems.Count == 0) return;
            var item = ErrorsListView.SelectedItems[0];
            if (item?.Tag is Jump j) WhenErrorClick(j);
        }
        public void ClearErrors() { ErrorsListView.Items.Clear(); }
        public void ShowErrors(object errobj)
        {
            if (errobj == null) { ClearErrors(); return; }
            ErrorsListView.Items.Clear();
            var errors = errobj as List<Jump>;
            if (errors == null) return;
            int max = 1900;
            foreach (var e in errors)
            {
                string fn = e.File;
                if (string.IsNullOrEmpty(fn)) fn = "?";
                else fn = Path.GetFileName(fn); // prikazi samo TabName u listi
                if (string.IsNullOrEmpty(fn)) fn = e.File;
                var title = $"[{fn}] [{e.Spot}] {e.Desc}";
                var sz = TextRenderer.MeasureText(title, ErrorsListView.Font);
                max = Math.Max(max, sz.Width + 100);
                ErrorsListView.Items.Add(new ListViewItem(title) { Tag = e });
            }
            ErrorsListView.Columns[0].Width = max;
            ErrorsListView.Invalidate();
        }
        private void ErrorsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { JumpToSelected(); e.Handled = true; }
            else if (e.KeyCode == Keys.C && e.Control)
            {
                if (e.Shift) CopyAllToClipboard(); else CopyToCliboard();
            }
        }
        private void CopyToCliboard()
        {
            if (ErrorsListView.SelectedItems.Count > 0) Clipboard.SetText(ErrorsListView.SelectedItems[0].Text);
        }
        private void CopyAllToClipboard()
        {
            if (ErrorsListView.Items.Count == 0) return;
            var sb = new StringBuilder();
            foreach (ListViewItem li in ErrorsListView.Items) sb.AppendLine(li.Text);
            Clipboard.SetText(sb.ToString());
        }
    }
}
