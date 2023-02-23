using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp
{
    public partial class FInvoker : DockContent
    {
        internal Action<string, int> WhenReferenceClick = (s, e) => { };

        public FInvoker()
        {
            InitializeComponent();
            ReferecesListView.Click += ReferecesListView_Click;
        }

        private void ReferecesListView_Click(object sender, EventArgs e)
        {
            var item = ReferecesListView.SelectedItems[0];
            if (item != null && item.Tag != null)
            {
                var data = (Tuple<string, int>)item.Tag;
                WhenReferenceClick(data.Item1, data.Item2);
            }
        }

        public void DisplayReferences(List<Tuple<string, int>> list)
        {
            if (list == null)
                return;

            ReferecesListView.Items.Clear();
            foreach (var item in list)
            {
                var title = $"[{item.Item1}] [{item.Item2}]\n";
                ReferecesListView.Items.Add(new ListViewItem(title) { Tag = item });
            }
        }
    }
}