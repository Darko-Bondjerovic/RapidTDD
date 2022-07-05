using DiffNamespace;
using System;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp.TestsView
{
    public partial class TestsForm : DockContent
    {
        public TestsPanel tstPanel = null;

        public TestsForm()
        {
            InitializeComponent();
            this.Text = "Tests view";            

            tstPanel = new TestsPanel();
            this.Controls.Add(tstPanel);            
        }

        internal void RenameTests(bool state)
        {
            tstPanel.TestsListView.RenameTests = state;
        }
    }
}
