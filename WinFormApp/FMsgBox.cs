using System;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class FMsgBox : Form
    {
        public FMsgBox()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {            
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }   

        public static bool Show(string message,
            bool question = false)
        {
            using (var frm = new FMsgBox())
            {
                frm.btnCancel.Visible = question;
                frm.ActiveControl = frm.btnOk;
                frm.btnOk.Text = question ? "Yes" : "Ok";
                frm.Text = question ? "Question" : "Info";
                frm.txt.Text = message;

                return frm.ShowDialog() == DialogResult.OK;
            }
        }

        private void FMsgBox_Shown(object sender, EventArgs e)
        {
            // when form is shown, Ok button does not have rectangle (focused) border.
            // https://stackoverflow.com/questions/38504389/on-windows-forms-set-tab-focus-to-a-button
          
            SendKeys.SendWait("{TAB}");
            SendKeys.SendWait("+{TAB}");
        }
    }
}
