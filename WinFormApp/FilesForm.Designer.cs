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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.txtPaths);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1488, 126);
            this.panel1.TabIndex = 8;
            // 
            // txtPaths
            // 
            this.txtPaths.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPaths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPaths.Font = new System.Drawing.Font("Consolas", 14.75F);
            this.txtPaths.Location = new System.Drawing.Point(0, 0);
            this.txtPaths.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPaths.Multiline = true;
            this.txtPaths.Name = "txtPaths";
            this.txtPaths.ReadOnly = true;
            this.txtPaths.Size = new System.Drawing.Size(1289, 126);
            this.txtPaths.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSaveFiles);
            this.panel3.Controls.Add(this.btnLoadFiles);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(1289, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(199, 126);
            this.panel3.TabIndex = 0;
            // 
            // btnSaveFiles
            // 
            this.btnSaveFiles.Font = new System.Drawing.Font("Consolas", 12.75F);
            this.btnSaveFiles.Location = new System.Drawing.Point(21, 65);
            this.btnSaveFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveFiles.Name = "btnSaveFiles";
            this.btnSaveFiles.Size = new System.Drawing.Size(161, 43);
            this.btnSaveFiles.TabIndex = 5;
            this.btnSaveFiles.Text = "Save files";
            this.btnSaveFiles.UseVisualStyleBackColor = true;
            this.btnSaveFiles.Click += new System.EventHandler(this.btnSaveFiles_Click);
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Font = new System.Drawing.Font("Consolas", 12.75F);
            this.btnLoadFiles.Location = new System.Drawing.Point(21, 15);
            this.btnLoadFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(161, 43);
            this.btnLoadFiles.TabIndex = 4;
            this.btnLoadFiles.Text = "Load files";
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            this.btnLoadFiles.Click += new System.EventHandler(this.btnLoadFiles_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 126);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1488, 779);
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
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1488, 779);
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
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 91);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 28);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(106, 91);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(88, 28);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // FilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1488, 905);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
    }
}