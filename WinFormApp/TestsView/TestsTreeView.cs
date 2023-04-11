using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DiffNamespace
{
    public class TestsTreeView : TreeView
    {
        private string oldTestText = "";

        public static Font FontReg = new Font("Consolas", 14);
        public static Font FontBold = new Font("Consolas", 14, FontStyle.Bold);

        public static readonly Color DARK_COLOR = ColorTranslator.FromHtml("#211E1E");

        public List<TestItem> currTests = new List<TestItem>();

        public bool KeepOldTests = false;

        public bool TestsAreChanged { get; set; } = false;

        public enum Filter { all, pass, fail };

        public Filter filter = Filter.all;
        
        public Action DoUpdateUI = () => { };

        int SelectedParent = -1;
        int SelectedIndex = -1;

        public TestsTreeView()
        {
            this.Font = FontReg;
            this.BackColor = DARK_COLOR;
            this.ForeColor = Color.White;

            this.ShowPlusMinus = true;

            // must set both (ShowLines & FullRowSelect)
            this.ShowLines = false;
            this.FullRowSelect = true;

            this.HideSelection = false;
            this.HotTracking = false;
            
            this.MouseDown += tree_MouseDown;

            // Images are added in imageList1 in TestsForm
            //this.ImageList = LoadImages();

            //this.CheckBoxes = true;
        }

        void InitSelectedIndx()
        {
            SelectedParent = -1;
            SelectedIndex = -1;
        }

        public void DisplayTests(List<TestItem> tests)
        {
            SaveSelIndx();
            UpdateTests(tests);
            RestoreSelIndx();
            
            DoUpdateUI();

            CheckIfTestsIsChanged();
        }

        private void CheckIfTestsIsChanged()
        {
            var currTestText = MakeTestFileContext().ToString();
            TestsAreChanged = oldTestText != currTestText;
            oldTestText = currTestText;
        }

        private void UpdateTests(List<TestItem> newTests)
        {
            var upd = new UpdateExpGroups(currTests, newTests, KeepOldTests);

            upd.Find(true);

            // do we have some different tests to rename or delete?
            if (upd.HaveDiffs && !KeepOldTests)
            {
                var frm = new UpdateDlg(upd);
                if (frm.ShowDialog() != DialogResult.OK)
                    return; // cancel, don't change anything
            }

            upd.PrevExpToNew();
            currTests = newTests;

            DisplayInUI();            
        }
        
        void DoUpdateIcons(TreeNode treeNode)
        {
            var item = treeNode.Tag;
            if (item == null)
                return;

            var tst = item as TestItem;
            var imgIndx = tst.Ignore ? 2 : tst.pass ? 1 : 0;
            treeNode.ImageIndex = imgIndx;
            treeNode.SelectedImageIndex = imgIndx;            
        
            foreach (TreeNode tn in treeNode.Nodes)
                DoUpdateIcons(tn);
        }
        
        public void UpdateIcons()
        {
            foreach (TreeNode tn in this.Nodes)
                DoUpdateIcons(tn);
        }
        
        public void DisplayInUI()
        {
            // remember expanded nodes 
            var expanded = new List<int>();
            foreach(TreeNode node in this.Nodes)
                if (node.IsExpanded)
                   expanded.Add(node.Index);
            
            this.BeginUpdate();

            this.Nodes.Clear();

            foreach (var item in currTests)
            {
                if (!IsInFilter(item) && !(item is GroupItem))
                    continue;
                
                var treeNode = MakeNode(item);

                if (item is GroupItem)
                {
                    foreach (var test in (item as GroupItem).Tests)
                    {
                        if (IsInFilter(test))
                            treeNode.Nodes.Add( MakeNode(test));
                    }
                }

                this.Nodes.Add(treeNode);
            }
            
            foreach(TreeNode node in this.Nodes)
                if (expanded.Contains(node.Index))
                    node.Expand();
            
            this.EndUpdate();
            
            UpdateIcons();
        }

        bool IsInFilter(TestItem test)
        {
            if (filter != Filter.all)
            {
                if ((test.pass && filter == Filter.fail)
                    || (!test.pass && filter == Filter.pass))
                    return false;
            }
                
            return true;
        }        
        
        TreeNode MakeNode(TestItem test)
        {
            var result = new TreeNode();
            result.Text = test.name;
            result.Tag = test;
            
            return result;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            //SB_HORZ = 0,  SB_VERT = 1, SB_CTL = 2, SB_BOTH = 3
            ShowScrollBar(this.Handle, 0, false); // hide horizontal scroll bar
            base.WndProc(ref m);
        }

        public bool HaveTests()
        {
            return this.currTests.Count > 0;
        }
        
        public TestItem GetSelectedTest()
        {
            if (this.SelectedNode == null)
            {
                //try to select 1. node in list if fileExists
                if (this.Nodes.Count > 0)
                    this.SelectedNode = this.Nodes[0];
            }

            if (this.SelectedNode == null)
                return null;
            
            var node = this.SelectedNode;
            return (TestItem)node.Tag ?? (TestItem)node.Tag;
        }
        
        //https://stackoverflow.com/questions/39420720/windows-form-treeview-afterclick-event-responding-differently-for-mouse-left-an
        private void tree_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = null;
            node = this.GetNodeAt(e.X, e.Y);
            var hti = this.HitTest(e.Location);

            if (e.Button != MouseButtons.Left ||
                hti.Location == TreeViewHitTestLocations.PlusMinus ||
                node == null)
            {
                return;
            }
            this.SelectedNode = node;
        }
        
        
        public ImageList LoadImages()
        {
            //var yes = @"WinFormApp.Resources.Yes.png";
            //Assembly assembly = Assembly.GetExecutingAssembly();

            var imageList = new ImageList();
            imageList.ColorDepth = ColorDepth.Depth32Bit;
        
            var folder = @"..\..\TestsView\images\";
            foreach (var path in Directory.GetFiles(folder))
                imageList.Images.Add(Image.FromFile(path));

            return imageList;
        }

        public void ReloadTests()
        {
            DisplayTests(this.currTests);
        }

        private void SaveSelIndx()
        {
            if (SelectedNode == null)
            {
                InitSelectedIndx();
            }
            else
            {
                SelectedParent = SelectedNode.Parent == null 
                    ? -1 : SelectedNode.Parent.Index;
                SelectedIndex = SelectedNode.Index;
            }
        }

        private void RestoreSelIndx()
        {
            if (this.Nodes.Count == 0)
                return;

            if (SelectedParent == -1)
            {
                foreach (TreeNode child in this.Nodes)
                    if (SelectedIndex == child.Index)
                    {
                        this.SelectedNode = child;
                        return;
                    }
            }
            else
            {
                foreach (TreeNode node in this.Nodes)
                    if (SelectedParent == node.Index)
                        foreach (TreeNode child in node.Nodes)
                            if (SelectedIndex == child.Index)
                            {
                                this.SelectedNode = child;
                                return;
                            }
            }

            // if nothing is selected, select 1. node            
            this.SelectedNode = this.Nodes[0];
            
        }
                    
        internal string Progress()
        {
            var passCount = 0;
            var allCount = 0;
            
            foreach(var item in currTests)
            {
                if (item is GroupItem)
                {
                    foreach(var test in (item as GroupItem).Tests)
                    {
                        allCount++;
                        if (test.pass)
                            passCount++;
                    }
                 }
                 else
                 {
                     allCount++;
                     if (item.pass)
                         passCount++;
                 }
            }
            
            return $"[{passCount}/{allCount}] ";
        }
        
        internal void CopyAllActToExp()
        {
            foreach(var item in currTests)
            {
                if (item is GroupItem)
                    foreach(var t in (item as GroupItem).Tests)
                        t.CopyActToExp();
                 else
                     item.CopyActToExp();
            }

            DoUpdateUI();
        }

        public void SetFilterState(CheckState state)
        {
            filter = Filter.all;
            
            if (state == CheckState.Checked)
                filter = Filter.pass;
            
            if (state == CheckState.Indeterminate)
                filter = Filter.fail;
            
            DisplayInUI();
        }
        
        static List<TestItem> LoadTests(IEnumerable<XElement> xmlTests)
        {
            return xmlTests.Select( o => new TestItem(o)).ToList();
        }
        
        List<TestItem> LoadTests(string fileName)
        {
            var xdoc = XDocument.Load(fileName);
            var gxml = xdoc.Root.Elements("GroupItem");

            var result = new List<TestItem>();

            if (gxml.Count() == 0)
                result = LoadTests(xdoc.Descendants("TestItem"));
            else                           
                result = LoadTests(xdoc.Root.Elements("TestItem"));                        

            var groups = gxml.Select( g => new GroupItem(g) { 
                  Tests = LoadTests(g.Descendants("TestItem")) 
                  }).ToList<TestItem>();

            result.AddRange(groups);

            return result;
        }
              
        public List<TestItem> LoadTestsFromFile(string fileName)
        {
            var tests = LoadTests(fileName);
            DisplayTests(tests);      
            return tests;
        }
        
        public void SaveTests(string fileName)
        {
            XDocument xdoc = MakeTestFileContext();
            xdoc.Save(fileName);
        }

        public XDocument MakeTestFileContext()
        {
            var xdoc = new XDocument();

            var root = new XElement("Tests");
            xdoc.Add(root);

            foreach (var item in currTests)
            {
                var el = item.ToXml();
                root.Add(el);

                if (item is GroupItem)
                    foreach (var t in (item as GroupItem).Tests)
                        el.Add(t.ToXml());
            }

            return xdoc;
        }

        public void UnloadTests()
        {
            this.Nodes.Clear();
            this.TestsAreChanged = false;
            this.currTests = new List<TestItem>();
        }
    }
}
