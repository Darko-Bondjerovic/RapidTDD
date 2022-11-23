using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WinFormApp
{
    public partial class FilesForm : Form
    {
        public static string chk(bool state)
        {
            // https://www.compart.com/en/unicode
            return state ? "☒" : "☐"; // U+2612/U+2610
        }
                
        static int ShowColIndx = 1;

        List<EditForm> editors = null;

        public FilesForm(List<EditForm> editors)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.listView1.View = View.Details;

            for(int i=0; i<2; i++)
                this.listView1.Columns[i].Tag = true;            
            
            SetColumnsCaptions();

            this.listView1.ColumnClick += ListView1_ColumnClick;
            this.listView1.MouseDown += ListView1_MouseDown;

            this.editors = editors;

            foreach (var e in editors)
            {
                var item = new ListViewItem(new[] { 
                    chk(e.DoBuild), chk(!e.IsHidden), e.TabName });
                listView1.Items.Add(item);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // when user press ESC button, close this dialog
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ListView1_MouseDown(object sender, MouseEventArgs e)
        {
            var info = listView1.HitTest(e.X, e.Y);
            var row = info.Item.Index;
            var col = info.Item.SubItems.IndexOf(info.SubItem);
            var text = info.Item.SubItems[col].Text;

            if (col == 2)
                return;

            //this.Text = string.Format("R{0}:C{1} val '{2}'", row, col, text);

            text = (text == chk(true)) ? chk(false) : chk(true);
            listView1.Items[row].SubItems[col].Text = text;

            if (col == ShowColIndx)
                editors[row].IsHidden = !editors[row].IsHidden;
            else
                editors[row].DoBuild = !editors[row].DoBuild;
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 2)
                return;

            var newValue = ChangeColChecked(e);

            foreach (ListViewItem item in listView1.Items)            
                item.SubItems[e.Column].Text = chk(newValue);

            foreach (var edit in editors)
                if (e.Column == ShowColIndx)
                    edit.IsHidden = !newValue;
                else
                    edit.DoBuild = newValue;
        }

        private void SetColumnsCaptions()
        {
            var cols = new string[] { "Build", "Show" };

            for (int i = 0; i < 2; i++)
            {
                var value = Convert.ToBoolean(listView1.Columns[i].Tag);
                listView1.Columns[i].Text = $"{chk(value)} {cols[i]}";
            }
        }

        private bool ChangeColChecked(ColumnClickEventArgs e)
        {
            var column = listView1.Columns[e.Column];
            var newValue = !Convert.ToBoolean(column.Tag);
            column.Tag = newValue;

            SetColumnsCaptions();

            return newValue;
        }

        public static void ShowDialog(List<EditForm> editors)
        {
            if (editors.Count == 0)
                return;

            using (var frm = new FilesForm(editors))
            {
                frm.ShowDialog();
            }
        }
    }
}
