namespace WinFormApp
{
    partial class FilesForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtPaths = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSaveFiles = new System.Windows.Forms.Button();
            this.btnLoadFiles = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ColHeadBuild = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColHeadShow = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColHeadFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColHeadFull = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtPaths);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1116, 102);
            this.panel1.TabIndex = 8;
            // 
            // txtPaths
            // 
            this.txtPaths.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPaths.Font = new System.Drawing.Font("Consolas", 14.75F);
            this.txtPaths.Location = new System.Drawing.Point(0, 0);
            this.txtPaths.Multiline = true;
            this.txtPaths.Name = "txtPaths";
            this.txtPaths.ReadOnly = true;
            this.txtPaths.Size = new System.Drawing.Size(967, 102);
            this.txtPaths.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSaveFiles);
            this.panel3.Controls.Add(this.btnLoadFiles);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(967, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(149, 102);
            this.panel3.TabIndex = 0;
            // 
            // btnSaveFiles
            // 
            this.btnSaveFiles.Font = new System.Drawing.Font("Consolas", 12.75F);
            this.btnSaveFiles.Location = new System.Drawing.Point(16, 53);
            this.btnSaveFiles.Name = "btnSaveFiles";
            this.btnSaveFiles.Size = new System.Drawing.Size(121, 35);
            this.btnSaveFiles.TabIndex = 5;
            this.btnSaveFiles.Text = "Save files";
            this.btnSaveFiles.UseVisualStyleBackColor = true;
            this.btnSaveFiles.Click += new System.EventHandler(this.btnSaveFiles_Click);
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Font = new System.Drawing.Font("Consolas", 12.75F);
            this.btnLoadFiles.Location = new System.Drawing.Point(16, 12);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(121, 35);
            this.btnLoadFiles.TabIndex = 4;
            this.btnLoadFiles.Text = "Load files";
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            this.btnLoadFiles.Click += new System.EventHandler(this.btnLoadFiles_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 102);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1116, 633);
            this.panel2.TabIndex = 9;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColHeadBuild,
            this.ColHeadShow,
            this.ColHeadFile,
            this.ColHeadFull});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("Consolas", 13.75F);
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1116, 633);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ColHeadBuild
            // 
            this.ColHeadBuild.Tag = "1";
            this.ColHeadBuild.Text = "☐ Build";
            this.ColHeadBuild.Width = 110;
            // 
            // ColHeadShow
            // 
            this.ColHeadShow.Tag = "1";
            this.ColHeadShow.Text = "☐ Show";
            this.ColHeadShow.Width = 100;
            // 
            // ColHeadFile
            // 
            this.ColHeadFile.Tag = "0";
            this.ColHeadFile.Text = "Name";
            this.ColHeadFile.Width = 220;
            // 
            // ColHeadFull
            // 
            this.ColHeadFull.Text = "Full name";
            this.ColHeadFull.Width = 680;
            // 
            // FilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1116, 735);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FilesForm";
            this.Text = "Files (tabs)";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader ColHeadBuild;
        private System.Windows.Forms.ColumnHeader ColHeadShow;
        private System.Windows.Forms.ColumnHeader ColHeadFile;
        private System.Windows.Forms.ColumnHeader ColHeadFull;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSaveFiles;
        private System.Windows.Forms.Button btnLoadFiles;
        private System.Windows.Forms.TextBox txtPaths;
    }
}