using System;
using System.Drawing;
using System.Windows.Forms;

namespace DiffNamespace
{
    public class TestsPanel : TableLayoutPanel
    {
        string TestsFileName = "";

        Panel testsTopPanel = null;
        SplitContainer split1 = null;
        SplitContainer diffSplit = null;        
        Button btnCopyAll = null;
        Button btnCopy = null;        
        Label lblSelectedTest = null;
        CheckBox chkFilter = null;

        public TestsListView TestsListView = null;
        public ColoredTextBox ActualTextBox = null;
        public ColoredTextBox ExpectedTextBox = null;

        private string TESTS_FILTER = 
                "Tests file (*.tst)" + "|*.tst|" +
                "All files (*.*)" + "|*.*";

        public TestsPanel()
        {
            this.Dock = DockStyle.Fill;

            this.RowStyles.Clear();
            this.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            this.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            this.ColumnStyles.Clear();
            this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            testsTopPanel = new Panel();
            testsTopPanel.Dock = DockStyle.Fill;
            this.Controls.Add(testsTopPanel, 0, 0);

            lblSelectedTest = new Label();
            lblSelectedTest.Font = new Font("Consolas", 14F);
            testsTopPanel.Controls.Add(lblSelectedTest);
            lblSelectedTest.Dock = DockStyle.Fill;
            lblSelectedTest.TextAlign = ContentAlignment.MiddleLeft;

            chkFilter = new CheckBox();
            chkFilter.ThreeState = true;            
            chkFilter.CheckState = CheckState.Unchecked;
            chkFilter.Text = "all tests";
            chkFilter.Dock = DockStyle.Right;
            chkFilter.CheckStateChanged += ChkFilter_CheckStateChanged;
            testsTopPanel.Controls.Add(chkFilter);

            btnCopyAll = new Button();
            btnCopyAll.Text = "Copy ALL";
            testsTopPanel.Controls.Add(btnCopyAll);
            btnCopyAll.Click += ClickCopyAllBtn;
            btnCopyAll.Dock = DockStyle.Left;
            btnCopyAll.Visible = false;

            btnCopy = new Button();
            btnCopy.Text = "Copy [F4]";
            testsTopPanel.Controls.Add(btnCopy);
            btnCopy.Click += ClickCopyBtn;
            btnCopy.Dock = DockStyle.Left;
            btnCopy.Visible = false;

            var btnOrnt1 = new Button();
            btnOrnt1.Text = "tst";
            btnOrnt1.Width = 35;
            testsTopPanel.Controls.Add(btnOrnt1);
            btnOrnt1.Click += ClickOrnt1;
            btnOrnt1.Dock = DockStyle.Right;            

            var btnOrnt2 = new Button();
            btnOrnt2.Text = "dfs";
            btnOrnt2.Width = 35;
            testsTopPanel.Controls.Add(btnOrnt2);
            btnOrnt2.Click += ClickOrnt2;
            btnOrnt2.Dock = DockStyle.Right;

            split1 = new SplitContainer();
            split1.Dock = DockStyle.Fill;
            this.Controls.Add(split1, 0, 1);
            split1.Orientation = Orientation.Vertical;
            split1.SplitterDistance = (2 * split1.Parent.Width) / 5;

            TestsListView = new TestsListView();
            TestsListView.Dock = DockStyle.Fill;
            split1.Panel1.Controls.Add(TestsListView);
            TestsListView.SelectedIndexChanged += ListView1_SelectedIndexChanged;
            TestsListView.DoUpdateUI = UpdateSelectedTestToUI;            

            diffSplit = new SplitContainer();
            diffSplit.Dock = DockStyle.Fill;
            split1.Panel2.Controls.Add(diffSplit);
            diffSplit.Orientation = Orientation.Horizontal;
            diffSplit.SplitterDistance = diffSplit.Parent.Height / 2;

            ActualTextBox = new ColoredTextBox()
            {
                Dock = DockStyle.Fill,                             
                ReadOnly = true,
            };

            ActualTextBox.vScroll += txtSource_vScroll;
            ActualTextBox.KeyUp += txtSource_KeyUp;
            diffSplit.Panel1.Controls.Add(ActualTextBox);

            ExpectedTextBox = new ColoredTextBox()
            {
                Dock = DockStyle.Fill,                
                ReadOnly = false,                
                ShortcutsEnabled = true
            };

            ExpectedTextBox.vScroll += txtTarget_vScroll;
            ExpectedTextBox.KeyUp += txtTarget_KeyUp;
            ExpectedTextBox.TextChanged += WhenTextChanged;
            ExpectedTextBox.KeyDown += WhenPasteText;
            diffSplit.Panel2.Controls.Add(ExpectedTextBox);

            ChangeOrientation(split1);
        }

        private void ChkFilter_CheckStateChanged(object sender, EventArgs e)
        {
            switch (chkFilter.CheckState)
            {
                case CheckState.Checked:
                    chkFilter.Text = "pass tests";
                    break;

                case CheckState.Unchecked:
                    chkFilter.Text = "all tests";
                    break;

                case CheckState.Indeterminate:
                    chkFilter.Text = "fail tests";                    
                    break;
            }

            this.TestsListView.SetFilterState(chkFilter.CheckState);
        }

        private void WhenPasteText(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string lPastingText = Clipboard.GetData(DataFormats.Text) as string;
                ExpectedTextBox.SelectedText = lPastingText; // (string)Clipboard.GetData("Text");
                e.Handled = true;
            }
        }

        private void ClickCopyAllBtn(object sender, EventArgs e)
        {
            CopyAllActToExp();
        }

        public void CopyAllActToExp()
        {
            if (MessageBox.Show("Copy actual result into expected for all tests?", "Confirm",
                               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TestsListView.CopyAllActToExp();
                TestsListView.DisplayInUI();

            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // this method called 2 times: I for unselected
            // and II for selected item in list. :(
            if (TestsListView.SelectedItems.Count == 0)
                return;
            
            UpdateSelectedTestToUI();
        }
        
        private void UpdateSelectedTestToUI()
        {
            lblSelectedTest.Text =
                TestsListView.Progress();

            var tst = GetSelectedTest();
            if (tst != null)
            {
                //lblSelectedTest.Text += tst.name;

                Action action = () => 
                {
                    ActualTextBox.Text= tst.act;
                    ExpectedTextBox.Text = tst.exp;
                };
                
                RunUpdateUI(action);                
                ScrollPosToTop();
            }
        }
        
        private void WhenTextChanged(object sender, EventArgs e)
        {
            var tst = GetSelectedTest();
            if (tst == null) 
                return;

            Action action = () => { tst.exp = ExpectedTextBox.Text; };

            // TestItem run diff when exp is changed,
            // then we should repaint text boxes:
            RunUpdateUI(action);

            lblSelectedTest.Text = TestsListView.Progress();
        }
        
        void RunUpdateUI(Action action)
        {
            ActualTextBox.TextChanged -= WhenTextChanged;
            ExpectedTextBox.TextChanged -= WhenTextChanged;

            try
            {
                if (action != null)
                    action();

                TestsListView.Refresh(); // if test pass/fail :)

                PaintTestDiffsInActBox();
            }
            finally
            {
                ActualTextBox.TextChanged += WhenTextChanged;
                ExpectedTextBox.TextChanged += WhenTextChanged;
            }
        }
        
        public void CopyTextActToExp()
        {
            var tst = GetSelectedTest();
            if (tst != null)
                tst.CopyActToExp();

            UpdateSelectedTestToUI();

            TestsListView.DisplayInUI();
        }        

        private SaveFileDialog CreateSaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = TESTS_FILTER;
            dialog.DefaultExt = "tst";
            dialog.AddExtension = false;
            return dialog;
        }

        internal void SaveTests()
        {
            if (!TestsListView.HaveTests())
                return;

            if (TestsFileName == "")
            {
                SaveFileDialog saveDlg = CreateSaveDialog();
                if (saveDlg.ShowDialog() == DialogResult.OK)
                    TestsFileName = saveDlg.FileName;
            }
            if (TestsFileName != "")
                TestsListView.SaveTests(TestsFileName);
        }

        private OpenFileDialog CreateOpenDialog()
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Multiselect = false;
            openDlg.Filter = TESTS_FILTER;
            openDlg.DefaultExt = "tst";
            return openDlg;
        }

        internal void LoadTests()
        {
            OpenFileDialog openDlg = CreateOpenDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                TestsListView.Items.Clear();
                TestsListView.LoadTests(openDlg.FileName);
                TestsFileName = openDlg.FileName;
            }
        }

        internal void ShowResponseInUI(string response)
        {
            var newTests = Parser.Find(response);
            TestsListView.DisplayTests(newTests);
        }
        
        private TestItem GetSelectedTest()
        {
            if (TestsListView.SelectedItems.Count == 0)
                return null;
            
            var lvi = TestsListView.SelectedItems[0];
            return (TestItem)lvi.Tag ?? (TestItem)lvi.Tag;
        }

        internal void ChangeTheme()
        {
            ActualTextBox.ChangeTheme();
            ExpectedTextBox.ChangeTheme();
            TestsListView.ChangeTheme();
        }

        private void txtSource_vScroll(Message message)
        {
            message.HWnd = ExpectedTextBox.Handle;
            ExpectedTextBox.PubWndProc(ref message);
        }

        private void txtSource_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageDown ||
                e.KeyCode == Keys.PageUp ||
                (e.Modifiers == Keys.Control && e.KeyCode == Keys.End) ||
                (e.Modifiers == Keys.Control && e.KeyCode == Keys.Home)
               )
            {
                ExpectedTextBox.SelectionStart = ActualTextBox.SelectionStart;
                ExpectedTextBox.ScrollToCaret();
            }
        }

        private void txtTarget_vScroll(Message message)
        {
            message.HWnd = ActualTextBox.Handle;
            ActualTextBox.PubWndProc(ref message);
        }

        private void txtTarget_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.PageDown ||
                e.KeyCode == Keys.PageUp ||
                (e.Modifiers == Keys.Control && e.KeyCode == Keys.End) ||
                (e.Modifiers == Keys.Control && e.KeyCode == Keys.Home)
               )
            {
                ActualTextBox.SelectionStart = ExpectedTextBox.SelectionStart;
                ActualTextBox.ScrollToCaret();
            }
        }
        
        public void ChangeOrientation(SplitContainer split)
        {
            if (split.Orientation == Orientation.Horizontal)
            {
                split.SplitterDistance = split.Parent.Width / 2;
                split.Orientation = Orientation.Vertical;
            }
            else
            {
                split.SplitterDistance =  split.Parent.Height / 2;
                split.Orientation = Orientation.Horizontal;
            }
        }
        
        private void ClickOrnt1(object sender, EventArgs e)
        {
            ChangeOrientation(split1);
        }
        
        private void ClickOrnt2(object sender, EventArgs e)
        {
            ChangeOrientation(diffSplit);
        }               
        
        private void ClickCopyBtn(object sender, EventArgs e)
        {
            CopyTextActToExp();
        }
        
        private void PaintTestDiffsInActBox()
        {
            ActualTextBox.SuspendPainting();

            var tst = GetSelectedTest();
            if (tst == null)
                return;

            tst.PaintBoxUI(ActualTextBox);

            ActualTextBox.ResumePainting();
        }

        public void SetLstViewColumnWidth()
        {
            TestsListView.Columns[0].Width = TestsListView.ClientRectangle.Width;
        }

        private void ScrollPosToTop()
        {
            if (ActualTextBox.Text != "")
            {
                ActualTextBox.SelectionStart = 0;
                ActualTextBox.SelectionLength = 0;
                ActualTextBox.ScrollToCaret();
            }
            if (ExpectedTextBox.Text != "")
            {
                ExpectedTextBox.SelectionStart = 0;
                ExpectedTextBox.SelectionLength = 0;
                ExpectedTextBox.ScrollToCaret();
            }
        }        

        internal void SaveTestsAs()
        {
            SaveFileDialog saveDlg = CreateSaveDialog();
            saveDlg.FileName = TestsFileName;
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                TestsFileName = saveDlg.FileName;
                TestsListView.SaveTests(TestsFileName);
            }
        }
    }
}
