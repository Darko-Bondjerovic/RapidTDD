using System.Reflection;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, System.EventArgs e)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            var add =
@"-------------------------------------------------------------------------
Many thanks for Pavel Torgashov, creator of FastColoredTextBox!
	
	The texbox control supports following hotkeys:

	* Ctrl+F, Ctrl+H - shows Find and Replace dialogs
	* F3 - find next
	* Ctrl+G - shows GoTo dialog
	* Ctrl+(C, V, X) - standard clipboard operations
	* Ctrl+Z, Alt+Backspace, Ctrl+R - Undo/Redo
	* Alt+Up, Alt+Down - moves selected lines up/down
	* Ctrl+B, Ctrl+Shift-B - add, removes bookmark
	* Ctrl+N, Ctrl+Shift+N - navigates to bookmark
	* Ctrl+Wheel - zooming";

            label1.Text =
                "                       Rapid TDD application\n\n\n" +
                "Compile, run c# code in memory and execute tests\n\n\n" +
                "           Copyleft © 2022 by Darko Bondjerovic\n\n\n\n" + 
                $"Version: {version} \n\n\n" + add;
        }
    }
}
