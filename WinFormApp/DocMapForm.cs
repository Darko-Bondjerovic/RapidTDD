using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp
{
    public partial class DocMapForm : DockContent
    {
        public Action WhenClosingForm = () => { };

        private MainForm main = null;

        public DocMapForm(MainForm main)
        {
            InitializeComponent();

            this.CloseButtonVisible = false;            
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.FormClosing += DocMapForm_FormClosing;

            this.main = main;
        }

        private void DocMapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WhenClosingForm();
        }

        internal void SetTarget(EditForm edit)
        {
            if (edit != null)
                this.documentMap1.Target = edit.fctb;
        }
    }
}
