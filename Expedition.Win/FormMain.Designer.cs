namespace Expedition.Win
{
	partial class FormMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.toolStrips = new System.Windows.Forms.ToolStripContainer();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.treeFolders = new System.Windows.Forms.TreeView();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.splitSub = new System.Windows.Forms.SplitContainer();
			this.listFiles = new System.Windows.Forms.ListView();
			this.colFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFileStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.listLog = new System.Windows.Forms.ListView();
			this.toolBar = new System.Windows.Forms.ToolStrip();
			this.cmdOpen = new System.Windows.Forms.ToolStripButton();
			this.cmdClose = new System.Windows.Forms.ToolStripButton();
			this.cmdLoad = new System.Windows.Forms.ToolStripButton();
			this.cmdSave = new System.Windows.Forms.ToolStripButton();
			this.cmdCreate = new System.Windows.Forms.ToolStripButton();
			this.cmdVerify = new System.Windows.Forms.ToolStripButton();
			this.cmdReset = new System.Windows.Forms.ToolStripButton();
			this.txtPath = new System.Windows.Forms.ToolStripTextBox();
			this.toolStrips.BottomToolStripPanel.SuspendLayout();
			this.toolStrips.ContentPanel.SuspendLayout();
			this.toolStrips.TopToolStripPanel.SuspendLayout();
			this.toolStrips.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
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
			this.splitMain.Panel1.Controls.Add(this.treeFolders);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.splitSub);
			this.splitMain.Size = new System.Drawing.Size(1423, 875);
			this.splitMain.SplitterDistance = 474;
			this.splitMain.TabIndex = 0;
			// 
			// treeFolders
			// 
			this.treeFolders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeFolders.ImageIndex = 0;
			this.treeFolders.ImageList = this.imageList;
			this.treeFolders.Location = new System.Drawing.Point(0, 0);
			this.treeFolders.Name = "treeFolders";
			this.treeFolders.SelectedImageIndex = 0;
			this.treeFolders.Size = new System.Drawing.Size(474, 875);
			this.treeFolders.TabIndex = 0;
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "Root");
			this.imageList.Images.SetKeyName(1, "Folder");
			this.imageList.Images.SetKeyName(2, "FolderTemp");
			this.imageList.Images.SetKeyName(3, "FolderGood");
			this.imageList.Images.SetKeyName(4, "FolderError");
			this.imageList.Images.SetKeyName(5, "File");
			this.imageList.Images.SetKeyName(6, "FileTemp");
			this.imageList.Images.SetKeyName(7, "FileGood");
			this.imageList.Images.SetKeyName(8, "FileError");
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
			this.splitSub.Panel1.Controls.Add(this.listFiles);
			// 
			// splitSub.Panel2
			// 
			this.splitSub.Panel2.Controls.Add(this.listLog);
			this.splitSub.Size = new System.Drawing.Size(945, 875);
			this.splitSub.SplitterDistance = 544;
			this.splitSub.TabIndex = 0;
			// 
			// listFIles
			// 
			this.listFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName,
            this.colFileSize,
            this.colFileStatus});
			this.listFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listFiles.FullRowSelect = true;
			this.listFiles.GridLines = true;
			this.listFiles.Location = new System.Drawing.Point(0, 0);
			this.listFiles.Name = "listFIles";
			this.listFiles.Size = new System.Drawing.Size(945, 544);
			this.listFiles.SmallImageList = this.imageList;
			this.listFiles.TabIndex = 0;
			this.listFiles.UseCompatibleStateImageBehavior = false;
			this.listFiles.View = System.Windows.Forms.View.Details;
			// 
			// colFileName
			// 
			this.colFileName.Text = "File";
			this.colFileName.Width = 400;
			// 
			// colFileSize
			// 
			this.colFileSize.Text = "Size";
			this.colFileSize.Width = 100;
			// 
			// colFileStatus
			// 
			this.colFileStatus.Text = "Status";
			this.colFileStatus.Width = 100;
			// 
			// listLog
			// 
			this.listLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listLog.Location = new System.Drawing.Point(0, 0);
			this.listLog.Name = "listLog";
			this.listLog.Size = new System.Drawing.Size(945, 327);
			this.listLog.TabIndex = 0;
			this.listLog.UseCompatibleStateImageBehavior = false;
			// 
			// toolBar
			// 
			this.toolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.toolBar.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdOpen,
            this.cmdClose,
            this.cmdLoad,
            this.cmdSave,
            this.cmdCreate,
            this.cmdVerify,
            this.cmdReset,
            this.txtPath});
			this.toolBar.Location = new System.Drawing.Point(3, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.Size = new System.Drawing.Size(1075, 39);
			this.toolBar.TabIndex = 0;
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
			// 
			// cmdLoad
			// 
			this.cmdLoad.Image = global::Expedition.Win.Properties.Resources.Load;
			this.cmdLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdLoad.Name = "cmdLoad";
			this.cmdLoad.Size = new System.Drawing.Size(73, 36);
			this.cmdLoad.Text = "Load";
			this.cmdLoad.ToolTipText = "Load a checksum file";
			this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
			// 
			// cmdSave
			// 
			this.cmdSave.Image = global::Expedition.Win.Properties.Resources.Save;
			this.cmdSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(71, 36);
			this.cmdSave.Text = "Save";
			this.cmdSave.ToolTipText = "Save to checksum file";
			// 
			// cmdCreate
			// 
			this.cmdCreate.Image = global::Expedition.Win.Properties.Resources.Create;
			this.cmdCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdCreate.Name = "cmdCreate";
			this.cmdCreate.Size = new System.Drawing.Size(82, 36);
			this.cmdCreate.Text = "Create";
			this.cmdCreate.ToolTipText = "Create a new checksum file";
			// 
			// cmdVerify
			// 
			this.cmdVerify.Image = global::Expedition.Win.Properties.Resources.Verify;
			this.cmdVerify.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdVerify.Name = "cmdVerify";
			this.cmdVerify.Size = new System.Drawing.Size(76, 36);
			this.cmdVerify.Text = "Verify";
			this.cmdVerify.ToolTipText = "Verify selected checksum file";
			// 
			// cmdReset
			// 
			this.cmdReset.Image = global::Expedition.Win.Properties.Resources.Reset;
			this.cmdReset.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdReset.Name = "cmdReset";
			this.cmdReset.Size = new System.Drawing.Size(76, 36);
			this.cmdReset.Text = "Reset";
			this.cmdReset.ToolTipText = "Reset all tree view items";
			this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
			// 
			// txtPath
			// 
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(500, 39);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1423, 936);
			this.Controls.Add(this.toolStrips);
			this.Name = "FormMain";
			this.Text = "Expedition Lookout";
			this.Load += new System.EventHandler(this.FormMain_Load);
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
		private System.Windows.Forms.ListView listFiles;
		private System.Windows.Forms.ListView listLog;
		private System.Windows.Forms.ToolStripButton cmdOpen;
		private System.Windows.Forms.ToolStripButton cmdLoad;
		private System.Windows.Forms.ToolStripButton cmdCreate;
		private System.Windows.Forms.ToolStripButton cmdVerify;
		private System.Windows.Forms.ToolStripTextBox txtPath;
		private System.Windows.Forms.ToolStripButton cmdClose;
		private System.Windows.Forms.ToolStripButton cmdReset;
		private System.Windows.Forms.ToolStripButton cmdSave;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ColumnHeader colFileName;
		private System.Windows.Forms.ColumnHeader colFileSize;
		private System.Windows.Forms.ColumnHeader colFileStatus;
	}
}

