
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
            this.ErrorsListView = new DiffNamespace.ColoredListView();
            this.SuspendLayout();
            // 
            // ReferecesListView
            // 
            this.ErrorsListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ErrorsListView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ErrorsList.BackgroundImage")));
            this.ErrorsListView.BackgroundImageTiled = true;
            this.ErrorsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorsListView.Font = new System.Drawing.Font("Consolas", 16F);
            this.ErrorsListView.ForeColor = System.Drawing.Color.White;
            this.ErrorsListView.FullRowSelect = true;
            this.ErrorsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ErrorsListView.HideSelection = false;
            this.ErrorsListView.Location = new System.Drawing.Point(0, 0);
            this.ErrorsListView.MultiSelect = false;
            this.ErrorsListView.Name = "ErrorsList";
            this.ErrorsListView.OwnerDraw = true;
            this.ErrorsListView.ShowGroups = false;
            this.ErrorsListView.Size = new System.Drawing.Size(904, 525);
            this.ErrorsListView.TabIndex = 0;
            this.ErrorsListView.UseCompatibleStateImageBehavior = false;
            this.ErrorsListView.View = System.Windows.Forms.View.Details;
            this.ErrorsListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ErrorsList_KeyDown);
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 525);
            this.Controls.Add(this.ErrorsListView);
            this.Name = "ErrorForm";
            this.Text = "Errors";
            this.ResumeLayout(false);

        }

        #endregion

        public DiffNamespace.ColoredListView ErrorsListView;
    }
}