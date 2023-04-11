using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace WinFormApp
{
    public partial class CoverForm : DockContent
    {
        public static string CODECVG = 
            @"using System;
                using System.Collections.Generic;

                public static class CODECVG
                {
                    // usage : CODECVG.ADD(name, 8293);
    
                    static public Dictionary<string, HashSet<int>>  CCSpanList { get; set; } = 
                        new Dictionary<string, HashSet<int>>();
    
                    static public void ADD(string key, int value)
                    {
                        if (CCSpanList.ContainsKey(key))
                            CCSpanList[key].Add(value);
                        else
                            CCSpanList[key] = new HashSet<int>() {value};
                    }
                }";

        public class CoverItem
        {            
            public bool Check = true;
            public bool Color = true;
            public string ClassName = "";
            public int Percent = 0;
            public string TabName = "";

            public string OnlyClassName()
            {
                var indx = ClassName.LastIndexOf('.');
                return ClassName.Substring(indx + 1);
            }

            public ListViewItem MakeListViewItem()
            {
                var result = new ListViewItem( new[] 
                {                   
                    $"{Percent, 4} %", OnlyClassName(), TabName
                });

                result.Tag = this;

                return result;
            }
        }

        TextStyle redstyle = new TextStyle(Brushes.LightYellow, Brushes.LightCoral, FontStyle.Regular);

        public Action WhenClosingForm = () => { };

        private MainForm main = null;

        private List<DocInfo> docs = null;
        private List<MarkInfo> marks = null;

        public CoverForm(MainForm main)
        {
            InitializeComponent();

            this.main = main;

            this.CloseButtonVisible = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.FormClosing += CoverForm_FormClosing;
                        
            MakePopupMenu();
        }

        private void CoverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WhenClosingForm();
        }

        private void MakePopupMenu()
        {
            ContextMenu cntxMnu = new ContextMenu();
            var item = cntxMnu.MenuItems.Add("Jump to class code");
            item.Click += JumpToClassCode;
            listView1.ContextMenu = cntxMnu;
        }

        private void JumpToClassCode(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var indx = listView1.SelectedIndices[0];

                CoverItem ci = (CoverItem)listView1.Items[indx].Tag;

                var edit = main.editors.FirstOrDefault( x => x.TabName.Equals(ci.TabName));
                if (edit == null)
                    return;

                var start = new CodeCover(edit.fctb.Text)
                    .FindClassSpanStartInCode(ci.OnlyClassName());

                main.FindPosInSource(new Jump(ci.TabName, start));
            }
        }        

        private void btnRunCC_Click(object sender, EventArgs e)
        {
            RunCodeCover();
        }

        internal void RunCodeCover()
        {
            PrepareCoverForRun();
            main.ExecuteCoverSource(docs);
            GetCoverResults();
        }

        private void PrepareCoverForRun()
        {
            docs = new List<DocInfo>();
            marks = new List<MarkInfo>();

            var exists = docs.Find(x => x.full.Equals("CODECVG"));
            if (exists == null)
                docs.Add(new DocInfo("CODECVG", CODECVG));

            foreach (var edit in main.editors)
            {
                if (edit.DoBuild)
                {
                    string tabName = edit.TabName;

                    var cc = new CodeCover(edit.fctb.Text);
                    var ccode = cc.MakeForCC(tabName);
                    marks.AddRange(cc.markers);

                    docs.Add(new DocInfo(tabName, ccode));
                }
            }
        }

        private void GetCoverResults()
        {
            //var dict = marks.Except(ccList);
            marks.RemoveAll(m => m.className == "");

            foreach (var w in main.worker.CCList)
            {
                var m = marks.FirstOrDefault(x => x.Equals(w));
                if (m != null) m.IsHit = true;
            }

            DisplayMarksInListView();
            ClearPaintInEditors();
            PaintCoverInEditors();
        }

        private void DisplayMarksInListView()
        {
            listView1.Items.Clear();

            var result = marks.GroupBy(x => x.className)
               .Select(m => new CoverItem
               {
                   ClassName = m.First().className,
                   TabName = m.First().tabName,
                   Percent = (100 * m.Count(x => x.IsHit)) / m.Count()
               }).ToList();

            result.ForEach(r => listView1.Items.Add(r.MakeListViewItem()));
        }

        private void PaintCoverInEditors()
        {
            if (!chkColorOn.Checked)
                return;                

            foreach (var group in marks.Where(x => !x.IsHit).GroupBy(x => x.tabName))
            {
                var edit = main.editors.FirstOrDefault(x => x.TabName.Equals(group.Key));

                if (edit != null)
                {
                    foreach (var m in group)
                    {
                        var fctb = edit.fctb;
                        Range rng = new Range(fctb,
                            fctb.PositionToPlace(m.start),
                            fctb.PositionToPlace(m.ends));

                        rng.SetStyle(redstyle);
                    }
                }
            }
        }

        private void ClearPaintInEditors()
        {
            foreach (var edit in main.editors)
            {
                // clear all highlighted parts in text
                edit.fctb.ClearStylesBuffer();
                edit.fctb.Range.ClearStyle(StyleIndex.All);
                edit.fctb.OnSyntaxHighlight(new TextChangedEventArgs(edit.fctb.Range));
            }
        }       

        private void chkColorOn_CheckedChanged(object sender, EventArgs e)
        {
            ClearPaintInEditors();
            PaintCoverInEditors();
        }

        private void chkFullClassName_CheckedChanged(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listView1.Items)
            {
                var ci = (CoverItem)item.Tag;
                if (chkFullClassName.Checked)
                    item.SubItems[1].Text = ci.ClassName;
                else
                    item.SubItems[1].Text = ci.OnlyClassName();
            }
        }
    }
}
