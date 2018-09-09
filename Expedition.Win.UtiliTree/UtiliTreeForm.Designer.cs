namespace Expedition.Win.UtiliTree
{
	partial class UtiliTreeForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UtiliTreeForm));
			this.toolStrips = new System.Windows.Forms.ToolStripContainer();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.icon16 = new System.Windows.Forms.PictureBox();
			this.icon32 = new System.Windows.Forms.PictureBox();
			this.icon64 = new System.Windows.Forms.PictureBox();
			this.icon128 = new System.Windows.Forms.PictureBox();
			this.treeFolders = new System.Windows.Forms.TreeView();
			this.imageList16 = new System.Windows.Forms.ImageList(this.components);
			this.splitSub = new System.Windows.Forms.SplitContainer();
			this.listIcons = new System.Windows.Forms.ListView();
			this.imageList32 = new System.Windows.Forms.ImageList(this.components);
			this.listSelected = new System.Windows.Forms.ListView();
			this.toolBar = new System.Windows.Forms.ToolStrip();
			this.txtPath = new System.Windows.Forms.ToolStripTextBox();
			this.cmdOpen = new System.Windows.Forms.ToolStripButton();
			this.cmdClose = new System.Windows.Forms.ToolStripButton();
			this.cmdLoad = new System.Windows.Forms.ToolStripButton();
			this.cmdSave = new System.Windows.Forms.ToolStripButton();
			this.cmdCreate = new System.Windows.Forms.ToolStripButton();
			this.cmdVerify = new System.Windows.Forms.ToolStripButton();
			this.cmdReset = new System.Windows.Forms.ToolStripButton();
			this.toolStrips.BottomToolStripPanel.SuspendLayout();
			this.toolStrips.ContentPanel.SuspendLayout();
			this.toolStrips.TopToolStripPanel.SuspendLayout();
			this.toolStrips.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.icon16)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon32)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon64)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon128)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitSub)).BeginInit();
			this.splitSub.Panel1.SuspendLayout();
			this.splitSub.Panel2.SuspendLayout();
			this.splitSub.SuspendLayout();
			this.toolBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrips
			// 
			// 
			// toolStrips.BottomToolStripPanel
			// 
			this.toolStrips.BottomToolStripPanel.Controls.Add(this.statusBar);
			// 
			// toolStrips.ContentPanel
			// 
			this.toolStrips.ContentPanel.Controls.Add(this.splitMain);
			this.toolStrips.ContentPanel.Size = new System.Drawing.Size(1423, 875);
			this.toolStrips.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStrips.Location = new System.Drawing.Point(0, 0);
			this.toolStrips.Name = "toolStrips";
			this.toolStrips.Size = new System.Drawing.Size(1423, 936);
			this.toolStrips.TabIndex = 0;
			this.toolStrips.Text = "toolStripContainer1";
			// 
			// toolStrips.TopToolStripPanel
			// 
			this.toolStrips.TopToolStripPanel.Controls.Add(this.toolBar);
			// 
			// statusBar
			// 
			this.statusBar.Dock = System.Windows.Forms.DockStyle.None;
			this.statusBar.Location = new System.Drawing.Point(0, 0);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(1423, 22);
			this.statusBar.TabIndex = 0;
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.flowLayoutPanel1);
			this.splitMain.Panel1.Controls.Add(this.treeFolders);
			this.splitMain.Panel1MinSize = 10;
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.splitSub);
			this.splitMain.Panel2MinSize = 50;
			this.splitMain.Size = new System.Drawing.Size(1423, 875);
			this.splitMain.SplitterDistance = 200;
			this.splitMain.TabIndex = 0;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.icon16);
			this.flowLayoutPanel1.Controls.Add(this.icon32);
			this.flowLayoutPanel1.Controls.Add(this.icon64);
			this.flowLayoutPanel1.Controls.Add(this.icon128);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 200);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// icon16
			// 
			this.icon16.Location = new System.Drawing.Point(3, 3);
			this.icon16.Name = "icon16";
			this.icon16.Size = new System.Drawing.Size(100, 50);
			this.icon16.TabIndex = 2;
			this.icon16.TabStop = false;
			// 
			// icon32
			// 
			this.icon32.Location = new System.Drawing.Point(3, 59);
			this.icon32.Name = "icon32";
			this.icon32.Size = new System.Drawing.Size(100, 50);
			this.icon32.TabIndex = 1;
			this.icon32.TabStop = false;
			// 
			// icon64
			// 
			this.icon64.Location = new System.Drawing.Point(3, 115);
			this.icon64.Name = "icon64";
			this.icon64.Size = new System.Drawing.Size(100, 50);
			this.icon64.TabIndex = 0;
			this.icon64.TabStop = false;
			// 
			// icon128
			// 
			this.icon128.Location = new System.Drawing.Point(3, 171);
			this.icon128.Name = "icon128";
			this.icon128.Size = new System.Drawing.Size(100, 50);
			this.icon128.TabIndex = 3;
			this.icon128.TabStop = false;
			// 
			// treeFolders
			// 
			this.treeFolders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeFolders.ImageIndex = 0;
			this.treeFolders.ImageList = this.imageList16;
			this.treeFolders.Location = new System.Drawing.Point(0, 206);
			this.treeFolders.Name = "treeFolders";
			this.treeFolders.SelectedImageIndex = 0;
			this.treeFolders.Size = new System.Drawing.Size(200, 669);
			this.treeFolders.TabIndex = 0;
			// 
			// imageList16
			// 
			this.imageList16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList16.ImageStream")));
			this.imageList16.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList16.Images.SetKeyName(0, "Root");
			this.imageList16.Images.SetKeyName(1, "Folder");
			this.imageList16.Images.SetKeyName(2, "FolderTemp");
			this.imageList16.Images.SetKeyName(3, "FolderGood");
			this.imageList16.Images.SetKeyName(4, "FolderError");
			this.imageList16.Images.SetKeyName(5, "File");
			this.imageList16.Images.SetKeyName(6, "FileTemp");
			this.imageList16.Images.SetKeyName(7, "FileGood");
			this.imageList16.Images.SetKeyName(8, "FileError");
			// 
			// splitSub
			// 
			this.splitSub.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitSub.Location = new System.Drawing.Point(0, 0);
			this.splitSub.Name = "splitSub";
			this.splitSub.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitSub.Panel1
			// 
			this.splitSub.Panel1.Controls.Add(this.listIcons);
			// 
			// splitSub.Panel2
			// 
			this.splitSub.Panel2.Controls.Add(this.listSelected);
			this.splitSub.Size = new System.Drawing.Size(1219, 875);
			this.splitSub.SplitterDistance = 544;
			this.splitSub.TabIndex = 0;
			// 
			// listIcons
			// 
			this.listIcons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listIcons.LargeImageList = this.imageList32;
			this.listIcons.Location = new System.Drawing.Point(0, 0);
			this.listIcons.Name = "listIcons";
			this.listIcons.Size = new System.Drawing.Size(1219, 544);
			this.listIcons.SmallImageList = this.imageList16;
			this.listIcons.TabIndex = 0;
			this.listIcons.UseCompatibleStateImageBehavior = false;
			this.listIcons.View = System.Windows.Forms.View.List;
			// 
			// imageList32
			// 
			this.imageList32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList32.ImageStream")));
			this.imageList32.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList32.Images.SetKeyName(0, "Root");
			this.imageList32.Images.SetKeyName(1, "Folder");
			this.imageList32.Images.SetKeyName(2, "FolderTemp");
			this.imageList32.Images.SetKeyName(3, "FolderGood");
			this.imageList32.Images.SetKeyName(4, "FolderError");
			this.imageList32.Images.SetKeyName(5, "File");
			this.imageList32.Images.SetKeyName(6, "FileTemp");
			this.imageList32.Images.SetKeyName(7, "FileGood");
			this.imageList32.Images.SetKeyName(8, "FileError");
			// 
			// listSelected
			// 
			this.listSelected.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listSelected.LargeImageList = this.imageList32;
			this.listSelected.Location = new System.Drawing.Point(0, 0);
			this.listSelected.Name = "listSelected";
			this.listSelected.Size = new System.Drawing.Size(1219, 327);
			this.listSelected.SmallImageList = this.imageList16;
			this.listSelected.TabIndex = 0;
			this.listSelected.UseCompatibleStateImageBehavior = false;
			// 
			// toolBar
			// 
			this.toolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBar.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtPath,
            this.cmdOpen,
            this.cmdClose,
            this.cmdLoad,
            this.cmdSave,
            this.cmdCreate,
            this.cmdVerify,
            this.cmdReset});
			this.toolBar.Location = new System.Drawing.Point(3, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.Size = new System.Drawing.Size(915, 39);
			this.toolBar.TabIndex = 0;
			// 
			// txtPath
			// 
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(500, 39);
			// 
			// cmdOpen
			// 
			this.cmdOpen.Image = ((System.Drawing.Image)(resources.GetObject("cmdOpen.Image")));
			this.cmdOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdOpen.Name = "cmdOpen";
			this.cmdOpen.Size = new System.Drawing.Size(76, 36);
			this.cmdOpen.Text = "Open";
			this.cmdOpen.ToolTipText = "Open a directory";
			this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
			// 
			// cmdClose
			// 
			this.cmdClose.Image = ((System.Drawing.Image)(resources.GetObject("cmdClose.Image")));
			this.cmdClose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(76, 36);
			this.cmdClose.Text = "Close";
			this.cmdClose.ToolTipText = "Close a directory";
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// cmdLoad
			// 
			this.cmdLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdLoad.Name = "cmdLoad";
			this.cmdLoad.Size = new System.Drawing.Size(41, 36);
			this.cmdLoad.Text = "Load";
			this.cmdLoad.ToolTipText = "Load a checksum file";
			this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
			// 
			// cmdSave
			// 
			this.cmdSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(39, 36);
			this.cmdSave.Text = "Save";
			this.cmdSave.ToolTipText = "Save to checksum file";
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// cmdCreate
			// 
			this.cmdCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdCreate.Name = "cmdCreate";
			this.cmdCreate.Size = new System.Drawing.Size(50, 36);
			this.cmdCreate.Text = "Create";
			this.cmdCreate.ToolTipText = "Create a new checksum file";
			this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
			// 
			// cmdVerify
			// 
			this.cmdVerify.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdVerify.Name = "cmdVerify";
			this.cmdVerify.Size = new System.Drawing.Size(44, 36);
			this.cmdVerify.Text = "Verify";
			this.cmdVerify.ToolTipText = "Verify selected checksum file";
			// 
			// cmdReset
			// 
			this.cmdReset.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdReset.Name = "cmdReset";
			this.cmdReset.Size = new System.Drawing.Size(44, 36);
			this.cmdReset.Text = "Reset";
			this.cmdReset.ToolTipText = "Reset all tree view items";
			this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
			// 
			// UtiliTreeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1423, 936);
			this.Controls.Add(this.toolStrips);
			this.Name = "UtiliTreeForm";
			this.Text = "Expedition UtiliTree";
			this.Load += new System.EventHandler(this.UtiliTreeForm_Load);
			this.toolStrips.BottomToolStripPanel.ResumeLayout(false);
			this.toolStrips.BottomToolStripPanel.PerformLayout();
			this.toolStrips.ContentPanel.ResumeLayout(false);
			this.toolStrips.TopToolStripPanel.ResumeLayout(false);
			this.toolStrips.TopToolStripPanel.PerformLayout();
			this.toolStrips.ResumeLayout(false);
			this.toolStrips.PerformLayout();
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.icon16)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon32)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon64)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon128)).EndInit();
			this.splitSub.Panel1.ResumeLayout(false);
			this.splitSub.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitSub)).EndInit();
			this.splitSub.ResumeLayout(false);
			this.toolBar.ResumeLayout(false);
			this.toolBar.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripContainer toolStrips;
		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.ToolStrip toolBar;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.SplitContainer splitSub;
		private System.Windows.Forms.TreeView treeFolders;
		private System.Windows.Forms.ListView listIcons;
		private System.Windows.Forms.ListView listSelected;
		private System.Windows.Forms.ToolStripButton cmdOpen;
		private System.Windows.Forms.ToolStripButton cmdLoad;
		private System.Windows.Forms.ToolStripButton cmdCreate;
		private System.Windows.Forms.ToolStripButton cmdVerify;
		private System.Windows.Forms.ToolStripTextBox txtPath;
		private System.Windows.Forms.ToolStripButton cmdClose;
		private System.Windows.Forms.ToolStripButton cmdReset;
		private System.Windows.Forms.ToolStripButton cmdSave;
		private System.Windows.Forms.ImageList imageList16;
		private System.Windows.Forms.ImageList imageList32;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.PictureBox icon64;
		private System.Windows.Forms.PictureBox icon32;
		private System.Windows.Forms.PictureBox icon16;
		private System.Windows.Forms.PictureBox icon128;
	}
}

