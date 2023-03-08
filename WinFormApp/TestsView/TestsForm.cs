using DiffNamespace;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp.TestsView
{
    public partial class TestsForm : DockContent
    {
        public TestsPanel tstPanel = null;

        public TestsForm()
        {
            InitializeComponent();

            this.CloseButtonVisible = false;

            this.Text = "Tests view";            

            tstPanel = new TestsPanel();
            tstPanel.UpdateTitle = DoUpdateTitle;
            this.Controls.Add(tstPanel);

            tstPanel.treeView.ImageList = this.imageList1;

            this.FormClosing += TestsForm_FormClosing;
        }

        private void TestsForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            tstPanel.AskToSaveTestFile();
            e.Cancel = true;
        }

        void DoUpdateTitle(string tsfn)
        {
            this.Text = $"Test view [{Path.GetFileName(tsfn)}]";
        }

        internal void UnloadTestFile()
        {
            tstPanel.AskToSaveTestFile();
            tstPanel.UnloadTests();            
        }
    }
}
