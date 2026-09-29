using DiffNamespace;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing;
using System.Reflection;

namespace WinFormApp.TestsView
{
    public partial class TestsForm : DockContent
    {
        public TestsPanel tstPanel = null;

        public TestsForm()
        {
            InitializeComponent();

	    /*string path = Path.Combine(Application.StartupPath, "TestsView", "images");

	    imageList1.Images.Clear();
            imageList1.Images.Add("No.png", Image.FromFile(Path.Combine(path, "No.png")));
            imageList1.Images.Add("Yes.png", Image.FromFile(Path.Combine(path, "Yes.png")));
            imageList1.Images.Add("exclamation 48.png", 
		Image.FromFile(Path.Combine(path, "exclamation 48.png")));
	    */

var assembly = Assembly.GetExecutingAssembly();

imageList1.Images.Clear();

using (var stream = assembly.GetManifestResourceStream("WinFormApp.TestsView.images.No.png"))
using (var image = Image.FromStream(stream))
    imageList1.Images.Add("No.png", new Bitmap(image));

using (var stream = assembly.GetManifestResourceStream("WinFormApp.TestsView.images.Yes.png"))
using (var image = Image.FromStream(stream))
    imageList1.Images.Add("Yes.png", new Bitmap(image));

using (var stream = assembly.GetManifestResourceStream("WinFormApp.TestsView.images.exclamation 48.png"))
using (var image = Image.FromStream(stream))
    imageList1.Images.Add("exclamation 48.png", new Bitmap(image));


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
