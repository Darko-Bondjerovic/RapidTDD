using System;
using System.Drawing;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DiffNamespace
{
    public class TwoColsListView : BackgroundListView
    {
        TextFormatFlags flags = TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.VerticalCenter;

        public Action<ColumnClickEventArgs, bool> WhenColumnClick = (e, v) => { };

        public Action<int, int> WhenMouseDown = (r, c) => { };

        public string[] ColsNames = new string[2];
        
        public static string chk(bool state)
        {
            // https://www.compart.com/en/unicode
            return state ? "☒" : "☐"; // U+2612/U+2610
        }

        public TwoColsListView()
        {
            HeaderStyle = ColumnHeaderStyle.Clickable;

            this.Columns.Add("x", 85);
            this.Columns.Add("y", 85);

            // set this Tag before SetColumnCaption
            foreach (ColumnHeader col in this.Columns)
                col.Tag = true;
            
            this.MultiSelect = false;
            this.GridLines = false;
            this.MouseDown += ListView1_MouseDown;
            this.ColumnClick += ListView1_ColumnClick;
            this.OwnerDraw = true;
            this.DrawColumnHeader += ListView1_DrawColumnHeader;
            this.DrawSubItem += ListView1_DrawSubItem;
            this.VisibleChanged += ListView1_VisibleChanged;
        }

        private void ListView1_VisibleChanged(object sender, EventArgs e)
        {
            try { SetColumnsCaptions(); } catch { }
        }
        private void SetColumnsCaptions()
        {
            for (int i = 0; i < 2; i++)
            {
                var value = Convert.ToBoolean(this.Columns[i].Tag);
                this.Columns[i].Text = $"{chk(value)} {ColsNames[i]}";
            }
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

        
        private void ListView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;

            if (this.Items.Count == 0)
                return;

            try
            {
                var info = this.HitTest(e.X, e.Y);
                var row = info.Item.Index;
                var col = info.Item.SubItems.IndexOf(info.SubItem);
                var text = info.Item.SubItems[col].Text;

                if (col >= 2) // other columns
                    return;

                text = (text == chk(true)) ? chk(false) : chk(true);
                this.Items[row].SubItems[col].Text = text;

                WhenMouseDown(row, col);
            }
            catch { }
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column >= 2)
                return;

            var newValue = ChangeColChecked(e);

            foreach (ListViewItem item in this.Items)
                item.SubItems[e.Column].Text = chk(newValue);

            WhenColumnClick(e, newValue);
        }

        private bool ChangeColChecked(ColumnClickEventArgs e)
        {
            var column = this.Columns[e.Column];
            var newValue = !Convert.ToBoolean(column.Tag);
            column.Tag = newValue;

            SetColumnsCaptions();

            return newValue;
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