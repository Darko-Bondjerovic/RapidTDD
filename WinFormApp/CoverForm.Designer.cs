namespace WinFormApp
{
    partial class CoverForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoverForm));
            this.pnlListView = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnRunCC = new System.Windows.Forms.Button();
            this.chkColorOn = new System.Windows.Forms.CheckBox();
            this.chkFullClassName = new System.Windows.Forms.CheckBox();
            this.listView1 = new DiffNamespace.ColsListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlListView.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlListView
            // 
            this.pnlListView.Controls.Add(this.panel5);
            this.pnlListView.Controls.Add(this.panel4);
            this.pnlListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlListView.Location = new System.Drawing.Point(0, 0);
            this.pnlListView.Name = "pnlListView";
            this.pnlListView.Size = new System.Drawing.Size(800, 450);
            this.pnlListView.TabIndex = 10;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.listView1);
            this.panel5.Controls.Add(this.pnlBottom);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 32);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(800, 418);
            this.panel5.TabIndex = 10;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Font = new System.Drawing.Font("Consolas", 14.75F);
            this.pnlBottom.Location = new System.Drawing.Point(0, 388);
            this.pnlBottom.Multiline = true;
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.ReadOnly = true;
            this.pnlBottom.Size = new System.Drawing.Size(800, 30);
            this.pnlBottom.TabIndex = 1;
            this.pnlBottom.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.chkFullClassName);
            this.panel4.Controls.Add(this.chkColorOn);
            this.panel4.Controls.Add(this.btnRunCC);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(800, 32);
            this.panel4.TabIndex = 9;
            // 
            // btnRunCC
            // 
            this.btnRunCC.Location = new System.Drawing.Point(6, 6);
            this.btnRunCC.Margin = new System.Windows.Forms.Padding(2);
            this.btnRunCC.Name = "btnRunCC";
            this.btnRunCC.Size = new System.Drawing.Size(116, 22);
            this.btnRunCC.TabIndex = 6;
            this.btnRunCC.Text = "Run code cover.";
            this.btnRunCC.UseVisualStyleBackColor = true;
            this.btnRunCC.Click += new System.EventHandler(this.btnRunCC_Click);
            // 
            // chkColorOn
            // 
            this.chkColorOn.AutoSize = true;
            this.chkColorOn.Checked = true;
            this.chkColorOn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColorOn.Location = new System.Drawing.Point(140, 10);
            this.chkColorOn.Name = "chkColorOn";
            this.chkColorOn.Size = new System.Drawing.Size(77, 17);
            this.chkColorOn.TabIndex = 7;
            this.chkColorOn.Text = "Paint code";
            this.chkColorOn.UseVisualStyleBackColor = true;
            this.chkColorOn.CheckedChanged += new System.EventHandler(this.chkColorOn_CheckedChanged);
            // 
            // chkFullClassName
            // 
            this.chkFullClassName.AutoSize = true;
            this.chkFullClassName.Location = new System.Drawing.Point(238, 10);
            this.chkFullClassName.Name = "chkFullClassName";
            this.chkFullClassName.Size = new System.Drawing.Size(98, 17);
            this.chkFullClassName.TabIndex = 8;
            this.chkFullClassName.Text = "Full class name";
            this.chkFullClassName.UseVisualStyleBackColor = true;
            this.chkFullClassName.CheckedChanged += new System.EventHandler(this.chkFullClassName_CheckedChanged);
            // 
            // listView1
            // 
            this.listView1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("listView1.BackgroundImage")));
            this.listView1.BackgroundImageTiled = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Consolas", 14F);
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(800, 388);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Percent";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Class name";
            this.columnHeader4.Width = 240;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "TabName";
            this.columnHeader5.Width = 1800;
            // 
            // CoverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlListView);
            this.Name = "CoverForm";
            this.Text = "Code coverage";
            this.pnlListView.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlListView;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox pnlBottom;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnRunCC;
        private DiffNamespace.ColsListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.CheckBox chkFullClassName;
        private System.Windows.Forms.CheckBox chkColorOn;
    }
}