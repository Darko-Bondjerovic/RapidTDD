namespace WinFormApp
{
    partial class DocMapForm
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
            this.documentMap1 = new FastColoredTextBoxNS.DocumentMap();
            this.SuspendLayout();
            // 
            // documentMap1
            // 
            this.documentMap1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.documentMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentMap1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.documentMap1.Location = new System.Drawing.Point(0, 0);
            this.documentMap1.Name = "documentMap1";
            this.documentMap1.Size = new System.Drawing.Size(318, 554);
            this.documentMap1.TabIndex = 0;
            this.documentMap1.Target = null;
            this.documentMap1.Text = "documentMap1";
            // 
            // DocMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 554);
            this.Controls.Add(this.documentMap1);
            this.Name = "DocMapForm";
            this.Text = "Document map";
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.DocumentMap documentMap1;
    }
}