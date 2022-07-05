using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp.TestsView
{
    public partial class OutputForm : DockContent
    {
        public OutputForm()
        {
            InitializeComponent();            
        }

        internal void ShowResponseToUI(string response)
        {
            outTextBox.Text = response;
            outTextBox.PaintFailPassLines();
        }
    }
}
