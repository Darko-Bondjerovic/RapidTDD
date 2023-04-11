using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp
{
    public partial class InvokerForm : DockContent
    {
        internal Action<Jump> WhenReferenceClick = (o) => { };

        public InvokerForm()
        {
            InitializeComponent();

            ReferecesListView.Columns.Add(new ColumnHeader() { Width = 1900 });
            ReferecesListView.HeaderStyle = ColumnHeaderStyle.None;
            ReferecesListView.Click += ReferecesListView_Click;
        }

        private void ReferecesListView_Click(object sender, EventArgs e)
        {
            var item = ReferecesListView.SelectedItems[0];
            if (item != null && item.Tag != null)            
                WhenReferenceClick((Jump)item.Tag);
        }

        public void DisplayReferences(List<Jump> list)
        {
            if (list == null)
                return;

            ReferecesListView.Items.Clear();
            foreach (var e in list)
            {
                var title = $"[{e.File}] [{e.Spot}] {e.Desc}";
                ReferecesListView.Items.Add(new ListViewItem(title) { Tag = e });
            }
        }
    }
}