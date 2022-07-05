
namespace WinFormApp
{
    partial class ErrorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorForm));
            this.ErrorsList = new DiffNamespace.ColoredListView();
            this.SuspendLayout();
            // 
            // ErrorsList
            // 
            this.ErrorsList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ErrorsList.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ErrorsList.BackgroundImage")));
            this.ErrorsList.BackgroundImageTiled = true;
            this.ErrorsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorsList.Font = new System.Drawing.Font("Consolas", 16F);
            this.ErrorsList.ForeColor = System.Drawing.Color.White;
            this.ErrorsList.FullRowSelect = true;
            this.ErrorsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ErrorsList.HideSelection = false;
            this.ErrorsList.Location = new System.Drawing.Point(0, 0);
            this.ErrorsList.MultiSelect = false;
            this.ErrorsList.Name = "ErrorsList";
            this.ErrorsList.OwnerDraw = true;
            this.ErrorsList.ShowGroups = false;
            this.ErrorsList.Size = new System.Drawing.Size(800, 450);
            this.ErrorsList.TabIndex = 0;
            this.ErrorsList.UseCompatibleStateImageBehavior = false;
            this.ErrorsList.View = System.Windows.Forms.View.Details;
            this.ErrorsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ErrorsList_KeyDown);
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ErrorsList);
            this.Name = "ErrorForm";
            this.Text = "Errors";
            this.ResumeLayout(false);

        }

        #endregion

        public DiffNamespace.ColoredListView ErrorsList;
    }
}