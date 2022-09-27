
namespace WinFormApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dockpanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolNew = new System.Windows.Forms.ToolStripButton();
            this.toolOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolSave = new System.Windows.Forms.ToolStripButton();
            this.toolSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolInfo = new System.Windows.Forms.ToolStripLabel();
            this.toolActToExp = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.relaodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.fontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.insertDemoCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertTestsCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.generateMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.openTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.copyActToExpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllActToExpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayTestsViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayOutputViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.themesToolMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockpanel
            // 
            this.dockpanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dockpanel.BackColor = System.Drawing.Color.Gray;
            this.dockpanel.Location = new System.Drawing.Point(0, 66);
            this.dockpanel.Name = "dockpanel";
            this.dockpanel.Size = new System.Drawing.Size(800, 383);
            this.dockpanel.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolNew,
            this.toolOpen,
            this.toolStripSeparator3,
            this.toolSave,
            this.toolSaveAs,
            this.toolStripSeparator1,
            this.toolStripSeparator4,
            this.toolRun,
            this.toolStripSeparator5,
            this.toolInfo,
            this.toolActToExp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 39);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolNew
            // 
            this.toolNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolNew.Image = global::WinFormApp.Properties.Resources._new;
            this.toolNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNew.Name = "toolNew";
            this.toolNew.Size = new System.Drawing.Size(36, 36);
            this.toolNew.ToolTipText = "New";
            this.toolNew.Click += new System.EventHandler(this.toolNew_Click);
            // 
            // toolOpen
            // 
            this.toolOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolOpen.Image = global::WinFormApp.Properties.Resources.file_open;
            this.toolOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOpen.Name = "toolOpen";
            this.toolOpen.Size = new System.Drawing.Size(36, 36);
            this.toolOpen.ToolTipText = "Open";
            this.toolOpen.Click += new System.EventHandler(this.toolOpen_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // toolSave
            // 
            this.toolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSave.Image = global::WinFormApp.Properties.Resources.save_icon;
            this.toolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSave.Name = "toolSave";
            this.toolSave.Size = new System.Drawing.Size(36, 36);
            this.toolSave.ToolTipText = "Save";
            this.toolSave.Click += new System.EventHandler(this.toolSave_Click);
            // 
            // toolSaveAs
            // 
            this.toolSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolSaveAs.Image = global::WinFormApp.Properties.Resources.save_as_3;
            this.toolSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSaveAs.Name = "toolSaveAs";
            this.toolSaveAs.Size = new System.Drawing.Size(36, 36);
            this.toolSaveAs.ToolTipText = "Save as";
            this.toolSaveAs.Click += new System.EventHandler(this.toolSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 39);
            // 
            // toolRun
            // 
            this.toolRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolRun.Image = global::WinFormApp.Properties.Resources.Next;
            this.toolRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolRun.Name = "toolRun";
            this.toolRun.Size = new System.Drawing.Size(36, 36);
            this.toolRun.ToolTipText = "Execute";
            this.toolRun.Click += new System.EventHandler(this.toolRun_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 39);
            // 
            // toolInfo
            // 
            this.toolInfo.AutoSize = false;
            this.toolInfo.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolInfo.Name = "toolInfo";
            this.toolInfo.Size = new System.Drawing.Size(252, 36);
            this.toolInfo.Text = "   ";
            this.toolInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolActToExp
            // 
            this.toolActToExp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolActToExp.Image = global::WinFormApp.Properties.Resources.duplicate_48;
            this.toolActToExp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolActToExp.Name = "toolActToExp";
            this.toolActToExp.Size = new System.Drawing.Size(36, 36);
            this.toolActToExp.ToolTipText = "Copy act to exp";
            this.toolActToExp.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.testsToolStripMenuItem,
            this.viewsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.relaodToolStripMenuItem,
            this.toolStripMenuItem2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAllToolStripMenuItem,
            this.toolStripMenuItem1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::WinFormApp.Properties.Resources._new;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::WinFormApp.Properties.Resources.file_open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // relaodToolStripMenuItem
            // 
            this.relaodToolStripMenuItem.Name = "relaodToolStripMenuItem";
            this.relaodToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.relaodToolStripMenuItem.Text = "Relaod";
            this.relaodToolStripMenuItem.Click += new System.EventHandler(this.relaodToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(182, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::WinFormApp.Properties.Resources.save_icon;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::WinFormApp.Properties.Resources.save_as_3;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveAllToolStripMenuItem
            // 
            this.saveAllToolStripMenuItem.Image = global::WinFormApp.Properties.Resources.Save_icon_32;
            this.saveAllToolStripMenuItem.Name = "saveAllToolStripMenuItem";
            this.saveAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAllToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveAllToolStripMenuItem.Text = "Save all";
            this.saveAllToolStripMenuItem.Click += new System.EventHandler(this.saveAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.renameTestsToolStripMenuItem,
            this.toolStripMenuItem7,
            this.fontToolStripMenuItem,
            this.splitToolStripMenuItem,
            this.toolStripMenuItem3,
            this.insertDemoCodeToolStripMenuItem,
            this.insertTestsCodeToolStripMenuItem,
            this.toolStripMenuItem6,
            this.generateMethodToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = global::WinFormApp.Properties.Resources.Editing_Rename_icon;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.renameToolStripMenuItem.Text = "Rename symbol";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // renameTestsToolStripMenuItem
            // 
            this.renameTestsToolStripMenuItem.Name = "renameTestsToolStripMenuItem";
            this.renameTestsToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.renameTestsToolStripMenuItem.Text = "Rename tests";
            this.renameTestsToolStripMenuItem.Click += new System.EventHandler(this.renameTestsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(181, 6);
            // 
            // fontToolStripMenuItem
            // 
            this.fontToolStripMenuItem.Name = "fontToolStripMenuItem";
            this.fontToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.fontToolStripMenuItem.Text = "Font ...";
            this.fontToolStripMenuItem.Click += new System.EventHandler(this.fontToolStripMenuItem_Click);
            // 
            // splitToolStripMenuItem
            // 
            this.splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            this.splitToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.splitToolStripMenuItem.Text = "Split editor";
            this.splitToolStripMenuItem.Click += new System.EventHandler(this.splitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 6);
            // 
            // insertDemoCodeToolStripMenuItem
            // 
            this.insertDemoCodeToolStripMenuItem.Name = "insertDemoCodeToolStripMenuItem";
            this.insertDemoCodeToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.insertDemoCodeToolStripMenuItem.Text = "Insert demo code";
            this.insertDemoCodeToolStripMenuItem.Click += new System.EventHandler(this.insertDemoCodeToolStripMenuItem_Click);
            // 
            // insertTestsCodeToolStripMenuItem
            // 
            this.insertTestsCodeToolStripMenuItem.Name = "insertTestsCodeToolStripMenuItem";
            this.insertTestsCodeToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.insertTestsCodeToolStripMenuItem.Text = "Insert tests code";
            this.insertTestsCodeToolStripMenuItem.Click += new System.EventHandler(this.insertTestsCodeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(181, 6);
            // 
            // generateMethodToolStripMenuItem
            // 
            this.generateMethodToolStripMenuItem.Name = "generateMethodToolStripMenuItem";
            this.generateMethodToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.generateMethodToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.generateMethodToolStripMenuItem.Text = "Generate code";
            this.generateMethodToolStripMenuItem.Click += new System.EventHandler(this.generateMethodToolStripMenuItem_Click);
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executeCodeToolStripMenuItem,
            this.toolStripMenuItem4,
            this.openTestsToolStripMenuItem,
            this.saveTestsToolStripMenuItem,
            this.saveAsToolStripMenuItem1,
            this.toolStripMenuItem5,
            this.copyActToExpToolStripMenuItem,
            this.copyAllActToExpToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // executeCodeToolStripMenuItem
            // 
            this.executeCodeToolStripMenuItem.Image = global::WinFormApp.Properties.Resources.Next;
            this.executeCodeToolStripMenuItem.Name = "executeCodeToolStripMenuItem";
            this.executeCodeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.executeCodeToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.executeCodeToolStripMenuItem.Text = "Execute";
            this.executeCodeToolStripMenuItem.Click += new System.EventHandler(this.executeCodeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(190, 6);
            // 
            // openTestsToolStripMenuItem
            // 
            this.openTestsToolStripMenuItem.Name = "openTestsToolStripMenuItem";
            this.openTestsToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.openTestsToolStripMenuItem.Text = "Open tests";
            this.openTestsToolStripMenuItem.Click += new System.EventHandler(this.openTestsToolStripMenuItem_Click);
            // 
            // saveTestsToolStripMenuItem
            // 
            this.saveTestsToolStripMenuItem.Name = "saveTestsToolStripMenuItem";
            this.saveTestsToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.saveTestsToolStripMenuItem.Text = "Save tests";
            this.saveTestsToolStripMenuItem.Click += new System.EventHandler(this.saveTestsToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem1
            // 
            this.saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            this.saveAsToolStripMenuItem1.Size = new System.Drawing.Size(193, 26);
            this.saveAsToolStripMenuItem1.Text = "Save tests as";
            this.saveAsToolStripMenuItem1.Click += new System.EventHandler(this.saveAsToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(190, 6);
            // 
            // copyActToExpToolStripMenuItem
            // 
            this.copyActToExpToolStripMenuItem.Image = global::WinFormApp.Properties.Resources.duplicate_48;
            this.copyActToExpToolStripMenuItem.Name = "copyActToExpToolStripMenuItem";
            this.copyActToExpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.copyActToExpToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.copyActToExpToolStripMenuItem.Text = "Copy act to exp";
            this.copyActToExpToolStripMenuItem.Click += new System.EventHandler(this.copyActToExpToolStripMenuItem_Click);
            // 
            // copyAllActToExpToolStripMenuItem
            // 
            this.copyAllActToExpToolStripMenuItem.Name = "copyAllActToExpToolStripMenuItem";
            this.copyAllActToExpToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            this.copyAllActToExpToolStripMenuItem.Text = "Copy for all act to exp";
            this.copyAllActToExpToolStripMenuItem.Click += new System.EventHandler(this.copyAllActToExpToolStripMenuItem_Click);
            // 
            // viewsToolStripMenuItem
            // 
            this.viewsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayTestsViewToolStripMenuItem,
            this.displayOutputViewToolStripMenuItem,
            this.toolStripMenuItem9});
            this.viewsToolStripMenuItem.Name = "viewsToolStripMenuItem";
            this.viewsToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.viewsToolStripMenuItem.Text = "Views";
            // 
            // displayTestsViewToolStripMenuItem
            // 
            this.displayTestsViewToolStripMenuItem.Name = "displayTestsViewToolStripMenuItem";
            this.displayTestsViewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.displayTestsViewToolStripMenuItem.Text = "Display tests view";
            this.displayTestsViewToolStripMenuItem.Click += new System.EventHandler(this.displayTestsViewToolStripMenuItem_Click);
            // 
            // displayOutputViewToolStripMenuItem
            // 
            this.displayOutputViewToolStripMenuItem.Name = "displayOutputViewToolStripMenuItem";
            this.displayOutputViewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.displayOutputViewToolStripMenuItem.Text = "Display output view";
            this.displayOutputViewToolStripMenuItem.Click += new System.EventHandler(this.displayOutputViewToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(177, 6);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addReferencesToolStripMenuItem,
            this.toolStripSeparator2,
            this.themesToolMenu,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // addReferencesToolStripMenuItem
            // 
            this.addReferencesToolStripMenuItem.Name = "addReferencesToolStripMenuItem";
            this.addReferencesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addReferencesToolStripMenuItem.Text = "Add references";
            this.addReferencesToolStripMenuItem.Click += new System.EventHandler(this.addReferencesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // themesToolMenu
            // 
            this.themesToolMenu.Name = "themesToolMenu";
            this.themesToolMenu.Size = new System.Drawing.Size(180, 22);
            this.themesToolMenu.Text = "Themes";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.dockpanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Rapid TDD";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockpanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolNew;
        private System.Windows.Forms.ToolStripButton toolOpen;
        private System.Windows.Forms.ToolStripLabel toolInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fontToolStripMenuItem;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.ToolStripButton toolSave;
        private System.Windows.Forms.ToolStripButton toolSaveAs;
        private System.Windows.Forms.ToolStripButton toolRun;
        private System.Windows.Forms.ToolStripMenuItem insertDemoCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyActToExpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem saveAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addReferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolActToExp;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executeCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem copyAllActToExpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem relaodToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem themesToolMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem splitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renameTestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertTestsCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem generateMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayTestsViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayOutputViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
    }
}

