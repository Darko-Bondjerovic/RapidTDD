using System.Drawing;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class SplashForm : Form
    {
        delegate void StringParameterDelegate(string Text);
        delegate void StringParameterWithStatusDelegate(string Text);
        delegate void SplashShowCloseDelegate();

        /// <summary>
        /// To ensure splash screen is closed using the API and not by keyboard or any other things
        /// </summary>
        bool CloseSplashScreenFlag = false;

        public SplashForm()
        {
            InitializeComponent();
            this.label1.Parent = this;
            this.label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.Green;

            ProgressBar progressBar = new ProgressBar();
            progressBar.MarqueeAnimationSpeed = 50;                        
            progressBar.Style = ProgressBarStyle.Marquee;
            panel1.Controls.Add(progressBar);
            progressBar.Dock = DockStyle.Fill;

            progressBar.Show();
        }
        
        public void ShowSplashScreen()
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new SplashShowCloseDelegate(ShowSplashScreen));
                return;
            }
            this.Show();
            Application.Run(this);
        }
       
        public void CloseSplashScreen()
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new SplashShowCloseDelegate(CloseSplashScreen));
                return;
            }
            CloseSplashScreenFlag = true;
            this.Close();
        }

        public void UdpateStatusText(string Text)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(UdpateStatusText), new object[] { Text });
                return;
            }
            // Must be on the UI thread if we've got this far
            label1.ForeColor = Color.Green;
            label1.Text = Text;
        }
        
        public void UdpateStatusTextWithStatus(string Text)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterWithStatusDelegate(UdpateStatusTextWithStatus), new object[] { Text });
                return;
            }
            
            label1.Text = Text;
        }
        
        private void SplashForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseSplashScreenFlag == false)
                e.Cancel = true;
        }
    }

    public static class SplashScreen
    {
        static SplashForm sf = null;
        public static void ShowSplashScreen()
        {
            if (sf == null)
            {
                sf = new SplashForm();
                sf.ShowSplashScreen();
            }
        }

        public static void CloseSplashScreen()
        {
            if (sf != null)
            {
                sf.CloseSplashScreen();
                sf = null;
            }
        }

        public static void UdpateStatusText(string Text)
        {
            if (sf != null)
                sf.UdpateStatusText(Text);

        }

        public static void UdpateStatusTextWithStatus(string Text)
        {
            if (sf != null)
                sf.UdpateStatusTextWithStatus(Text);
        }
    }
}
