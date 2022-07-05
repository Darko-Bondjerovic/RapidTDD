using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//https://docs.microsoft.com/en-us/answers/questions/530055/how-to-color-a-specific-strings-in-richtextbox-tex.html

namespace DiffNamespace
{
    //https://stackoverflow.com/questions/6547193/how-to-append-text-to-richtextbox-without-scrolling-and-losing-selection
    public class SynchronizedTextBox : RichTextBox
    {
        #region ScrollBoth

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event vScrollEventHandler vScroll;
        public delegate void vScrollEventHandler(Message message);
        public const int WM_VSCROLL = 0x115;
        public const int WM_HSCROLL = 0x114;
        public const int WM_MOUSEWHEEL = 0x020A;

        protected override void WndProc(ref Message m)
        {
            // Code courtesy of Mark Mihevc  
            // sometimes we want to eat the paint message so we don't have to see all the  
            // flicker from when we select the text to change the color.

            if (m.Msg == WM_VSCROLL || m.Msg == WM_HSCROLL || m.Msg == WM_MOUSEWHEEL)
                if (vScroll != null)
                    vScroll(m);

            base.WndProc(ref m);
        }

        public void PubWndProc(ref Message message)
        {
            base.WndProc(ref message);
        }

        #endregion ScrollBoth        
    }

    public class FixedTextBox : SynchronizedTextBox
    {
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, ref Point lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, IntPtr lParam);

        const int WM_USER = 0x400;
        const int WM_SETREDRAW = 0x000B;
        const int EM_GETEVENTMASK = WM_USER + 59;
        const int EM_SETEVENTMASK = WM_USER + 69;
        const int EM_GETSCROLLPOS = WM_USER + 221;
        const int EM_SETSCROLLPOS = WM_USER + 222;

        Point _ScrollPoint;
        bool _Painting = true;
        IntPtr _EventMask;
        int _SuspendIndex = 0;
        int _SuspendLength = 0;

        public void SuspendPainting()
        {
            if (_Painting)
            {
                _SuspendIndex = this.SelectionStart;
                _SuspendLength = this.SelectionLength;
                SendMessage(this.Handle, EM_GETSCROLLPOS, 0, ref _ScrollPoint);
                SendMessage(this.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                _EventMask = SendMessage(this.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
                _Painting = false;
            }
        }

        public void ResumePainting()
        {
            if (!_Painting)
            {
                this.Select(_SuspendIndex, _SuspendLength);
                SendMessage(this.Handle, EM_SETSCROLLPOS, 0, ref _ScrollPoint);
                SendMessage(this.Handle, EM_SETEVENTMASK, 0, _EventMask);
                SendMessage(this.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                _Painting = true;
                this.Invalidate();
            }
        }
    }

    public class ColoredTextBox : FixedTextBox
    {
        private Font FONT = new Font("Consolas", 16);

        public static readonly Color DARK_COLOR = ColorTranslator.FromHtml("#211E1E");

        public static SolidBrush DARK_BRUSH = new SolidBrush(DARK_COLOR);

        private bool darkTheme = true;

        public void ChangeTheme()
        {
            darkTheme = !darkTheme;
            UpdateColors();
        }

        private void UpdateColors()
        {
            if (darkTheme)
            {
                ForeColor = Color.White;
                BackColor = DARK_COLOR;             
            }
            else
            {
                ForeColor = Color.Black;
                BackColor = Color.White;             
            }
        }

        public ColoredTextBox()
        {
            this.Font = FONT;
            this.WordWrap = false;
            UpdateColors();            
        }        

        public void ClearColors(TestItem tst)
        {
            Text = tst.exp;
            Select(0, Text.Length);
            UpdateColors();            
        }

        public static diff_match_patch _diff = new diff_match_patch();

	private static int GetLength(string str)
        {
            return str.Replace("\n", "  ").Length;
        }

        internal void PaintDiffs(TestItem tst)
        {
            var diffs = _diff.diff_main(tst.act, tst.exp, false);
            _diff.diff_cleanupSemanticLossless(diffs);

            this.Text = "";

            foreach (Diff diff in diffs)
            {   
		        var length = TextLength;
                var newText = SetSpecialChars(diff.text);
                AppendText(newText);
                Select(length, GetLength(newText));

                var color = Color.White;
                switch (diff.operation)
                {
                    case Operation.DELETE: color = Color.Crimson; break;
                    case Operation.INSERT: color = Color.Plum; break;
                    case Operation.EQUAL: color = darkTheme ?
                            DARK_COLOR : Color.White; break;
                }
                SelectionBackColor = color;
            }
        }

        private static string SetSpecialChars(string txt)
        {
            //https://www.fileformat.info/info/unicode/font/consolas/grid.htm

            /*
                ⤶ U+2936 ARROW POINTING DOWNWARDS THEN CURVING LEFTWARDS
                ↵ U+21B5 DOWNWARDS ARROW WITH CORNER LEFTWARDS
                ⏎ U+23CE RETURN SYMBOL
                ↲ U+21B2 DOWNWARDS ARROW WITH TIP LEFTWARDS
                ↩ U+21A9 LEFTWARDS ARROW WITH HOOK
            */

            txt = txt.Replace("\r", "");
            txt = txt.Replace("\n", "↵\n");
            return txt;
        }

        public void PaintFailPassLines()
        {
            int LineCount = 0;
            foreach (string line in Lines)
            {
                this.Select(
                    GetFirstCharIndexFromLine(LineCount),
                    line.Length);

                Color color = darkTheme ?
                    Color.White : Color.Black;

                if (line.Contains("PASS"))
                    color = Color.Green;

                if (line.Contains("FAIL"))
                    color = Color.Red;

                this.SelectionColor = color;

                LineCount++;
            }
        }
    }
}
