using System.Drawing;
using System.Windows.Forms;

namespace DiffNamespace
{
    public class ColoredListView : ListView
    {
        protected Pen penBlue = new Pen(Color.Blue) { Width = 2 };
        protected private bool darkTheme = true;
        protected private Bitmap backImage = new Bitmap(1, 1);

        public ColoredListView() : base()
        {
            this.ShowGroups = false;
            this.Sorting = SortOrder.None;
            this.ListViewItemSorter = null;

            this.DoubleBuffered = true;
            this.Font = new Font("Consolas", 14);

            this.FullRowSelect = true;

            this.View = System.Windows.Forms.View.Details;
            this.Columns.Add(new ColumnHeader() { Width = 1900 });
            HeaderStyle = ColumnHeaderStyle.None;
            this.MultiSelect = false;

            this.GridLines = false;
            this.OwnerDraw = true;
            this.DrawItem += ListViewDrawItem;                      

            this.BackgroundImage = backImage;
            this.BackgroundImageTiled = true;

            UpdateBackground();
        }        

        protected virtual void ListViewDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.Item.ForeColor = Color.White;
            e.DrawText();
                        
            if (!(sender as ListView).Focused  &&  e.Item.Selected)            
                e.Graphics.DrawRectangle(penBlue, e.Bounds); 
        }

        private void UpdateBackground()
        {
            // when disabled, bacground is gray :(
            //https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled

            this.BackgroundImage = null;
            var color = darkTheme ? ColoredTextBox.DARK_COLOR : Color.White;
            backImage.SetPixel(0, 0, color);
            this.BackgroundImage = backImage;
            this.BackgroundImageTiled = true;
        }
    }
}