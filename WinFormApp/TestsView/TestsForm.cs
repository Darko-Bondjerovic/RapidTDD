using DiffNamespace;
using System;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp.TestsView
{
    public partial class TestsForm : DockContent
    {
        public TestsPanel tstPanel = null;

        internal Action WhenFormClosing = () => { };

        public TestsForm()
        {
            InitializeComponent();
            this.Text = "Tests view";            

            tstPanel = new TestsPanel();
            tstPanel.UpdateTitle = DoUpdateTitle;
            this.Controls.Add(tstPanel);

            this.FormClosing += TestsForm_FormClosing;
        }

        private void TestsForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            tstPanel.AskToSaveTestFile();
            WhenFormClosing();
        }

        void DoUpdateTitle(string tsfn)
        {
            this.Text = $"Test view [{Path.GetFileName(tsfn)}]";
        }
    }
}
