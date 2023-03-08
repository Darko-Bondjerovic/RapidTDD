using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp.TestsView
{
    public partial class OutputForm : DockContent
    {
        public OutputForm()
        {
            InitializeComponent();
            this.CloseButtonVisible = false;
        }

        internal void ShowResponseToUI(string response)
        {
            outTextBox.Text = response;
            outTextBox.PaintFailPassLines();
        }

        private void OutputForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
