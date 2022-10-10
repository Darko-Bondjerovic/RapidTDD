using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;

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
            this.Font = new Font("Consolas", 16);

            this.FullRowSelect = true;

            this.View = View.Details;
            this.Columns.Add(new ColumnHeader() { Width = 1900 });
            HeaderStyle = ColumnHeaderStyle.None;
            this.MultiSelect = false;

            this.OwnerDraw = true;
            this.DrawItem += ListViewDrawItem;

            this.BackgroundImage = backImage;
            this.BackgroundImageTiled = true;

            //this.ForeColor = Color.White;
            //this.BackColor = ColoredTextBox.DARK_COLOR;

            UpdateBackground();
        }

        protected virtual void ListViewDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.Item.ForeColor = Color.White;
            e.DrawText();

            if (e.Item.Selected)
                e.Graphics.DrawRectangle(penBlue, e.Bounds);
        }

        public void ChangeTheme()
        {
            darkTheme = !darkTheme;
            UpdateBackground();
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

    public class TestsListView : ColoredListView
    { 
        private List<TestItem> currTests = new List<TestItem>();
        
        private string SelectedTestName = "";        
        
        public Action DoUpdateUI = () => { };
        
        private CheckState state = CheckState.Unchecked; // all tests

        public TestsListView() : base()
        {
            this.Columns[0].Width = 600;
        }
        public bool HaveTests()
        {
            return this.currTests.Count > 0;
        }

        protected override void ListViewDrawItem(object sender, DrawListViewItemEventArgs e)
        {
            TestItem tst = (TestItem)e.Item.Tag ?? (TestItem)e.Item.Tag;

            if (e.Item.Selected)
            {
                if (tst == null)
                {
                    e.Item.ForeColor = SystemColors.HighlightText;
                    e.Item.BackColor = SystemColors.Highlight;
                }
                else
                {
                    e.Item.ForeColor = Color.White;
                    e.Item.BackColor = tst.GetSelectColor();
                }
            }
            else
            {
                if (tst == null)
                {
                    e.Item.ForeColor = (sender as ListView).ForeColor;
                    e.Item.BackColor = (sender as ListView).BackColor;
                }
                else
                {
                    e.Item.ForeColor = Color.Black;
                    e.Item.BackColor = tst.GetDefaultColor();
                }
            }

            e.DrawBackground();
            e.DrawText();            

            if (!Enabled)
                return;

            if (e.Item.Selected)
                e.Graphics.DrawRectangle(penBlue, e.Bounds);
        }

        internal void DisplayTests(List<TestItem> newTests)
        {
            SaveSelIndx();
            UpdateTests(newTests);
            RestoreSelIndx(); 
            DoUpdateUI();
        }        

        private void UpdateTests(List<TestItem> newTests)
        {
            // try to awoid Items.Clear() - scrollbar jump like crazy! :(

            var upd = new UpdateExp(currTests, newTests);

            upd.Find(true);

            // do we have some different tests to rename or delete?
            if (upd.HaveDiffs())
            {
                var frm = new UpdateDlg(upd);                
                if (frm.ShowDialog() != DialogResult.OK)                    
                    return; // cancel, don't change anything
            }

            upd.PrevExpToNew();

            currTests = newTests;

            DisplayInUI();
        }        

        public void DisplayInUI()
        {
            var indx = 0;
            foreach (TestItem test in currTests)
            {
                if (state != CheckState.Unchecked) // != all tests
                {
                    if ((test.pass && state == CheckState.Indeterminate)
                        || (!test.pass && state == CheckState.Checked))
                        continue;
                }

                if (indx < this.Items.Count)
                {
                    Items[indx].Text = test.name;
                    Items[indx].Tag = test;                    
                }
                else
                {
                    Items.Add(MakeItem(test));
                }

                indx++;
            }

            for (int i = Items.Count-1; i >= indx; i--)
                Items.RemoveAt(i);
        }

        internal void SetFilterState(CheckState state)
        {
            this.state = state;
            DisplayInUI();
        }

        internal string Progress()
        {
            return $"[{currTests.Count(x => x.pass == true)}"+
                $"/{currTests.Count()}] ";
        }

        private void SaveSelIndx()
        {
            SelectedTestName = "";

            if (Items.Count == 0)
                return;

            if (SelectedItems.Count > 0)
            {
                ListViewItem item = (ListViewItem)SelectedItems[0];
                SelectedTestName = item.Text;              
            }
        }
        private void RestoreSelIndx()
        {
            if (Items.Count == 0)
                return;

            var indx = -1;
            var item = this.FindItemWithText(SelectedTestName);
            if (item != null)
                indx = item.Index;

            // try to select first item in list
            if (indx == -1 && Items.Count > 0)
                indx = 0;

            if (indx < Items.Count)
                Items[indx].Selected = true;
            
            // text editor will lose focus with this call:
            //this.Select();
        }

        private ListViewItem MakeItem(TestItem newTest)
        {
            return new ListViewItem(newTest.name) { Tag = newTest };
        }

        internal void CopyAllActToExp()
        {
            foreach (var t in currTests)
                t.CopyActToExp();

            DoUpdateUI();
        }

        internal void SaveTests(string fileName)
        {
            var xdoc = new XDocument(
                new XElement("Tests",
                    currTests.Select(o =>
                        new XElement("TestItem",
                            new XElement("name", o.name),
                            //new XElement("pass", x.pass),
                            new XElement("act", o.act),
                            new XElement("exp", o.exp)
                        ))));

            xdoc.Save(fileName);
        }

        internal void LoadTests(string fileName)
        {
            var xdoc = XDocument.Load(fileName);
            List<TestItem> newTests =
                xdoc.Root.Elements("TestItem")
                .Select(e =>
                    new TestItem()
                    {
                        name = e.Element("name").Value,
                        //pass = bool.Parse(e.Element("pass").Value),
                        act = e.Element("act").Value,
                        exp = e.Element("exp").Value
                    }).ToList();

            currTests.Clear(); // upload all new tests
            DisplayTests(newTests);
        }
    }
}