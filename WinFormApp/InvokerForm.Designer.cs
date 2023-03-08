namespace WinFormApp
{
    partial class InvokerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InvokerForm));
            this.ReferecesListView = new DiffNamespace.ColoredListView();
            this.SuspendLayout();
            // 
            // ReferecesListView
            // 
            this.ReferecesListView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ReferecesListView.BackgroundImage")));
            this.ReferecesListView.BackgroundImageTiled = true;
            this.ReferecesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReferecesListView.Font = new System.Drawing.Font("Consolas", 14F);
            this.ReferecesListView.FullRowSelect = true;
            this.ReferecesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ReferecesListView.HideSelection = false;
            this.ReferecesListView.Location = new System.Drawing.Point(0, 0);
            this.ReferecesListView.MultiSelect = false;
            this.ReferecesListView.Name = "ReferecesListView";
            this.ReferecesListView.OwnerDraw = true;
            this.ReferecesListView.ShowGroups = false;
            this.ReferecesListView.Size = new System.Drawing.Size(800, 450);
            this.ReferecesListView.TabIndex = 0;
            this.ReferecesListView.UseCompatibleStateImageBehavior = false;
            this.ReferecesListView.View = System.Windows.Forms.View.Details;            
            // 
            // InvokerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ReferecesListView);
            this.Name = "InvokerForm";
            this.Text = "References";
            this.ResumeLayout(false);

        }

        #endregion

        private DiffNamespace.ColoredListView ReferecesListView;
    }
}