
namespace WinFormApp.TestsView
{
    partial class OutputForm
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
            this.outTextBox = new DiffNamespace.ColoredTextBox();
            this.SuspendLayout();
            // 
            // outTextBox
            // 
            this.outTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.outTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outTextBox.ForeColor = System.Drawing.Color.White;
            this.outTextBox.Location = new System.Drawing.Point(0, 0);
            this.outTextBox.Name = "outTextBox";
            this.outTextBox.Size = new System.Drawing.Size(800, 450);
            this.outTextBox.TabIndex = 0;
            this.outTextBox.Text = "";
            // 
            // OutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.outTextBox);
            this.Name = "OutputForm";
            this.Text = "Output";
            this.ResumeLayout(false);

        }

        #endregion

        private DiffNamespace.ColoredTextBox outTextBox;
    }
}