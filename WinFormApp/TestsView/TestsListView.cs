using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

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
        private List<TestItem> tests = new List<TestItem>();
        
        private string SelectedTestName = "";        
        internal bool RenameTests = false;
        
        public Action DoUpdateUI = () => { };          
        
        public TestsListView() : base()
        {
            this.Columns[0].Width = 600;
        }
        public bool HaveTests()
        {
            return this.tests.Count > 0;
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

            if (RenameTests)
                DoRenameTests(newTests);
            else
                UpdateTests(newTests);

            RestoreSelIndx(); 
            DoUpdateUI();
        }

        private void DoRenameTests(List<TestItem> newTests)
        {
            for (int i = 0; i < newTests.Count(); i++)
            {
                if (i < tests.Count())
                {
                    var newName = newTests[i].name;
                    tests[i].name = newName;
                    Items[i].Text = newName;
                }
            }
        }

        private void UpdateTests(List<TestItem> newTests)
        {
            // try to awoid Items.Clear() - scrollbar jump like crazy! :(

            //var update = newTests.Intersect(tests).ToList();
            //before delete anything, update newTests with old exp.
            foreach (var n in newTests)
            {
                var o = tests
                    .Where(x => x.name.Equals(n.name))
                    .FirstOrDefault();

                if (o != null)
                {
                    if (n.exp == "")
                        n.exp = o.exp;
                }
            }

            var forDelete = tests.Except(newTests).ToList();

            if (forDelete.Count() > 0)
            {
                var lst = forDelete.Select(x => x.name).ToList();
                var str = string.Join("\n", lst);
                if (DialogResult.No == MessageBox.Show(
                    $"Delete tests:\n{str}?",                    
                    "Confirm", MessageBoxButtons.YesNo))
                   return;

                for(int i=Items.Count-1; i >= 0; i--)
                {
                    if (lst.Contains(Items[i].Text))
                        Items.RemoveAt(i);
                }

                tests.RemoveAll(x => forDelete.Contains(x));
            }            

            for (int i = 0; i < newTests.Count(); i++)
            {
                var newItem = MakeItem(newTests[i]);

                if (i < tests.Count())
                {
                    var indx = tests.IndexOf(newTests[i]);

                    if (indx == -1)
                    {
                        tests.Insert(i, newTests[i]);
                        Items.Insert(i, newItem);
                    }
                    else
                    {
                        if (indx == i)
                        {
                            Items[indx].Tag = newTests[i];
                        }
                        else
                        {
                            tests.RemoveAt(indx);
                            tests.Insert(i, newTests[i]);

                            Items.RemoveAt(indx);
                            Items.Insert(i, newItem);
                        }
                    }
                }
                else
                {                    
                    Items.Add(newItem);          
                }
            }

            tests = newTests;
        }

        internal string Progress()
        {
            return $"[{tests.Count(x => x.pass == true)}"+
                $"/{tests.Count()}] ";
        }

        private void SaveSelIndx()
        {
            SelectedTestName = "";

            if (Items.Count == 0)
                return;

            if (SelectedItems.Count > 0)
            {
                ListViewItem item = (ListViewItem)SelectedItems[0];
                SelectedTestName = ((TestItem)item.Tag).name;                
            }
        }
        private void RestoreSelIndx()
        {
            if (Items.Count == 0)
                return;

            var test = tests.FirstOrDefault(o => o.name.Equals(SelectedTestName));
            var indx = test == null ? 0 : tests.IndexOf(test);

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
            foreach (var t in tests)
                t.CopyActToExp();

            DoUpdateUI();
        }

        internal void SaveTests(string fileName)
        {
            var xdoc = new XDocument(
                new XElement("Tests",
                    tests.Select(o =>
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

            tests.Clear(); // upload all new tests
            DisplayTests(newTests);
        }
    }
}