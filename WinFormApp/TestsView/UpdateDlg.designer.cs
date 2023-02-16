namespace DiffNamespace
{
    partial class UpdateDlg
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.rbDelete = new System.Windows.Forms.RadioButton();
            this.rbRename = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkAllowDrop = new System.Windows.Forms.CheckBox();
            this.chkOnlyDiffs = new System.Windows.Forms.CheckBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.rbDelete);
            this.panel1.Controls.Add(this.rbRename);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(808, 117);
            this.panel1.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(692, 62);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 39);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.Location = new System.Drawing.Point(692, 16);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(105, 39);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // rbDelete
            // 
            this.rbDelete.AutoSize = true;
            this.rbDelete.Font = new System.Drawing.Font("Consolas", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDelete.Location = new System.Drawing.Point(24, 68);
            this.rbDelete.Margin = new System.Windows.Forms.Padding(2);
            this.rbDelete.Name = "rbDelete";
            this.rbDelete.Size = new System.Drawing.Size(514, 31);
            this.rbDelete.TabIndex = 1;
            this.rbDelete.Text = "Delete previous tests and exp. values";
            this.rbDelete.UseVisualStyleBackColor = true;
            // 
            // rbRename
            // 
            this.rbRename.AutoSize = true;
            this.rbRename.Checked = true;
            this.rbRename.Font = new System.Drawing.Font("Consolas", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRename.Location = new System.Drawing.Point(24, 22);
            this.rbRename.Margin = new System.Windows.Forms.Padding(2);
            this.rbRename.Name = "rbRename";
            this.rbRename.Size = new System.Drawing.Size(540, 31);
            this.rbRename.TabIndex = 0;
            this.rbRename.TabStop = true;
            this.rbRename.Text = "Rename tests and keep prev. exp. values";
            this.rbRename.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.listView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 117);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(808, 694);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkAllowDrop);
            this.panel3.Controls.Add(this.chkOnlyDiffs);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 654);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(808, 40);
            this.panel3.TabIndex = 2;
            // 
            // chkAllowDrop
            // 
            this.chkAllowDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllowDrop.AutoSize = true;
            this.chkAllowDrop.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold);
            this.chkAllowDrop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAllowDrop.Location = new System.Drawing.Point(552, 3);
            this.chkAllowDrop.Name = "chkAllowDrop";
            this.chkAllowDrop.Size = new System.Drawing.Size(229, 32);
            this.chkAllowDrop.TabIndex = 1;
            this.chkAllowDrop.Text = "Allow Drag Drop";
            this.chkAllowDrop.UseVisualStyleBackColor = true;
            // 
            // chkOnlyDiffs
            // 
            this.chkOnlyDiffs.AutoSize = true;
            this.chkOnlyDiffs.Checked = true;
            this.chkOnlyDiffs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOnlyDiffs.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOnlyDiffs.Location = new System.Drawing.Point(9, 3);
            this.chkOnlyDiffs.Margin = new System.Windows.Forms.Padding(2);
            this.chkOnlyDiffs.Name = "chkOnlyDiffs";
            this.chkOnlyDiffs.Size = new System.Drawing.Size(372, 32);
            this.chkOnlyDiffs.TabIndex = 0;
            this.chkOnlyDiffs.Text = "Show only the differences ";
            this.chkOnlyDiffs.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(808, 694);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // UpdateDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 811);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UpdateDlg";
            this.Text = "Tests lists";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.RadioButton rbDelete;
        private System.Windows.Forms.RadioButton rbRename;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chkOnlyDiffs;
        private System.Windows.Forms.CheckBox chkAllowDrop;
    }
}