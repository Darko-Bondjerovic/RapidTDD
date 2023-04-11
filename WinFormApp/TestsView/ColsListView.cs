using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DiffNamespace
{
    public class ColsListView : BackgroundListView
    {
        TextFormatFlags flags = TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.VerticalCenter;

        public Action<ColumnClickEventArgs, bool> WhenColumnClick = (e, v) => { };

        public Action<int, int> WhenMouseDown = (r, c) => { };
        
        

        public ColsListView()
        {   
            this.MultiSelect = false;
            this.GridLines = false;
            this.OwnerDraw = true;
            this.DrawColumnHeader += ListView1_DrawColumnHeader;
            this.DrawSubItem += ListView1_DrawSubItem;        
        }
        

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            //private enum ScrollBarDirection
            //{
            //    SB_HORZ = 0,  SB_VERT = 1, SB_CTL = 2, SB_BOTH = 3
            //}
            // Call to unmanaged WinAPI:
            //ShowScrollBar(this.Handle, (int)ScrollBarDirection.SB_HORZ, false);

            ShowScrollBar(this.Handle, 0, false); //<-- SB_HORZ
            base.WndProc(ref m);
        }

        private void ListView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // we already have background image:
            //e.Graphics.FillRectangle(ColoredTextBox.DARK_BRUSH, e.Bounds);

            bool selected = e.Item.Selected;
            Color color = selected ? Color.YellowGreen : Color.White;

            if (selected)
            {
                e.SubItem.BackColor = Color.DarkSlateBlue;
                e.DrawBackground();
            }

            TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.Item.Font, e.Bounds, color, flags);
        }

        private void ListView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.DimGray, e.Bounds);

            // draw lines between columns:
            Rectangle bounds = e.Bounds; bounds.Width -= 1; bounds.Height -= 1;
            e.Graphics.DrawLine(SystemPens.ControlLightLight, bounds.X, bounds.Y, bounds.Right, bounds.Y);
            e.Graphics.DrawLine(SystemPens.ControlLightLight, bounds.X, bounds.Y, bounds.X, bounds.Bottom);
            e.Graphics.DrawLine(SystemPens.ControlDark, bounds.X + 1, bounds.Bottom, bounds.Right, bounds.Bottom);
            e.Graphics.DrawLine(SystemPens.ControlDark, bounds.Right, bounds.Y + 1, bounds.Right, bounds.Bottom);

            TextRenderer.DrawText(e.Graphics, e.Header.Text, e.Font, e.Bounds, Color.White, flags);
        }

    }
}