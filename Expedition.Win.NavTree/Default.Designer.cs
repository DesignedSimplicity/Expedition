namespace Expedition.Win.NavTree
{
    partial class Default
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Default));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Folders", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Text", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Audio", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Image", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Video", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Other", System.Windows.Forms.HorizontalAlignment.Left);
            this.tools = new System.Windows.Forms.ToolStripContainer();
            this.status = new System.Windows.Forms.StatusStrip();
            this.progress = new System.Windows.Forms.ToolStripProgressBar();
            this.lblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitH = new System.Windows.Forms.SplitContainer();
            this.lstRecursive = new System.Windows.Forms.ListBox();
            this.tree = new System.Windows.Forms.TreeView();
            this.imgSmall = new System.Windows.Forms.ImageList(this.components);
            this.background = new System.ComponentModel.BackgroundWorker();
            this.txtQuery = new System.Windows.Forms.ToolStripTextBox();
            this.cmdStart = new System.Windows.Forms.ToolStripSplitButton();
            this.cmdStartClearResults = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdStartSelectDir = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdSave = new System.Windows.Forms.ToolStripDropDownButton();
            this.cmdExportText = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdExportCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.space1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdViewList = new System.Windows.Forms.ToolStripButton();
            this.cmdViewDetails = new System.Windows.Forms.ToolStripButton();
            this.cmdGroup = new System.Windows.Forms.ToolStripDropDownButton();
            this.cmdGroupNone = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdGroupTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdGroupFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdRecurse = new System.Windows.Forms.ToolStripDropDownButton();
            this.cmdRecurseNormal = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdRecurseShallow = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdRecurseDeep = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdRecurseFull = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdCheck = new System.Windows.Forms.ToolStripSplitButton();
            this.cmdCheckExists = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdCheckSizeDate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdCheckSync = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdHash = new System.Windows.Forms.ToolStripDropDownButton();
            this.cmdHashCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdHashVerify = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdHashLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdHashSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.importOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.photoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentTwoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.space2 = new System.Windows.Forms.ToolStripSeparator();
            this.list = new Expedition.Win.NavTree.Controls.ExpeditionList();
            this.tools.BottomToolStripPanel.SuspendLayout();
            this.tools.ContentPanel.SuspendLayout();
            this.tools.TopToolStripPanel.SuspendLayout();
            this.tools.SuspendLayout();
            this.status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitH)).BeginInit();
            this.splitH.Panel1.SuspendLayout();
            this.splitH.Panel2.SuspendLayout();
            this.splitH.SuspendLayout();
            this.toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tools
            // 
            // 
            // tools.BottomToolStripPanel
            // 
            this.tools.BottomToolStripPanel.Controls.Add(this.status);
            // 
            // tools.ContentPanel
            // 
            this.tools.ContentPanel.Controls.Add(this.splitH);
            this.tools.ContentPanel.Size = new System.Drawing.Size(944, 454);
            this.tools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tools.Location = new System.Drawing.Point(0, 0);
            this.tools.Name = "tools";
            this.tools.Size = new System.Drawing.Size(944, 501);
            this.tools.TabIndex = 0;
            this.tools.Text = "toolStripContainer1";
            // 
            // tools.TopToolStripPanel
            // 
            this.tools.TopToolStripPanel.Controls.Add(this.toolbar);
            // 
            // status
            // 
            this.status.Dock = System.Windows.Forms.DockStyle.None;
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progress,
            this.lblMessage});
            this.status.Location = new System.Drawing.Point(0, 0);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(944, 22);
            this.status.TabIndex = 0;
            // 
            // progress
            // 
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(197, 16);
            // 
            // lblMessage
            // 
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(730, 17);
            this.lblMessage.Spring = true;
            this.lblMessage.Text = "Status message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitH
            // 
            this.splitH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitH.Location = new System.Drawing.Point(0, 0);
            this.splitH.Margin = new System.Windows.Forms.Padding(0);
            this.splitH.Name = "splitH";
            // 
            // splitH.Panel1
            // 
            this.splitH.Panel1.Controls.Add(this.lstRecursive);
            this.splitH.Panel1.Controls.Add(this.tree);
            // 
            // splitH.Panel2
            // 
            this.splitH.Panel2.Controls.Add(this.list);
            this.splitH.Size = new System.Drawing.Size(944, 454);
            this.splitH.SplitterDistance = 200;
            this.splitH.SplitterWidth = 3;
            this.splitH.TabIndex = 0;
            this.splitH.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitH_SplitterMoved);
            // 
            // lstRecursive
            // 
            this.lstRecursive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstRecursive.FormattingEnabled = true;
            this.lstRecursive.Location = new System.Drawing.Point(0, 385);
            this.lstRecursive.Name = "lstRecursive";
            this.lstRecursive.Size = new System.Drawing.Size(200, 69);
            this.lstRecursive.TabIndex = 2;
            // 
            // tree
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.FullRowSelect = true;
            this.tree.HideSelection = false;
            this.tree.ImageIndex = 0;
            this.tree.ImageList = this.imgSmall;
            this.tree.Location = new System.Drawing.Point(0, 0);
            this.tree.Name = "tree";
            this.tree.SelectedImageIndex = 0;
            this.tree.Size = new System.Drawing.Size(200, 386);
            this.tree.TabIndex = 0;
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            // 
            // imgSmall
            // 
            this.imgSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSmall.ImageStream")));
            this.imgSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imgSmall.Images.SetKeyName(0, "open");
            this.imgSmall.Images.SetKeyName(1, "closed");
            this.imgSmall.Images.SetKeyName(2, "file");
            this.imgSmall.Images.SetKeyName(3, "text");
            this.imgSmall.Images.SetKeyName(4, "audio");
            this.imgSmall.Images.SetKeyName(5, "image");
            this.imgSmall.Images.SetKeyName(6, "video");
            // 
            // txtQuery
            // 
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(196, 25);
            this.txtQuery.Click += new System.EventHandler(this.txtQuery_Click);
            // 
            // cmdStart
            // 
            this.cmdStart.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdStartClearResults,
            this.importOnlyToolStripMenuItem,
            this.cmdStartSelectDir});
            this.cmdStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(63, 22);
            this.cmdStart.Text = "Start";
            this.cmdStart.ButtonClick += new System.EventHandler(this.cmdStart_ButtonClick);
            // 
            // cmdStartClearResults
            // 
            this.cmdStartClearResults.Name = "cmdStartClearResults";
            this.cmdStartClearResults.Size = new System.Drawing.Size(156, 22);
            this.cmdStartClearResults.Text = "Clear Tree";
            this.cmdStartClearResults.Click += new System.EventHandler(this.cmdStartClearResults_Click);
            // 
            // cmdStartSelectDir
            // 
            this.cmdStartSelectDir.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recentOneToolStripMenuItem,
            this.recentTwoToolStripMenuItem});
            this.cmdStartSelectDir.Name = "cmdStartSelectDir";
            this.cmdStartSelectDir.Size = new System.Drawing.Size(156, 22);
            this.cmdStartSelectDir.Text = "Select Directory";
            // 
            // cmdSave
            // 
            this.cmdSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdExportText,
            this.cmdExportCSV});
            this.cmdSave.Image = ((System.Drawing.Image)(resources.GetObject("cmdSave.Image")));
            this.cmdSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(69, 22);
            this.cmdSave.Text = "Export";
            // 
            // cmdExportText
            // 
            this.cmdExportText.Name = "cmdExportText";
            this.cmdExportText.Size = new System.Drawing.Size(96, 22);
            this.cmdExportText.Text = "Text";
            this.cmdExportText.Click += new System.EventHandler(this.cmdExportText_Click);
            // 
            // cmdExportCSV
            // 
            this.cmdExportCSV.Name = "cmdExportCSV";
            this.cmdExportCSV.Size = new System.Drawing.Size(96, 22);
            this.cmdExportCSV.Text = "CSV";
            this.cmdExportCSV.Click += new System.EventHandler(this.cmdExportCSV_Click);
            // 
            // space1
            // 
            this.space1.Name = "space1";
            this.space1.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdViewList
            // 
            this.cmdViewList.Image = ((System.Drawing.Image)(resources.GetObject("cmdViewList.Image")));
            this.cmdViewList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdViewList.Name = "cmdViewList";
            this.cmdViewList.Size = new System.Drawing.Size(45, 22);
            this.cmdViewList.Text = "List";
            this.cmdViewList.Click += new System.EventHandler(this.cmdViewList_Click);
            // 
            // cmdViewDetails
            // 
            this.cmdViewDetails.Image = ((System.Drawing.Image)(resources.GetObject("cmdViewDetails.Image")));
            this.cmdViewDetails.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdViewDetails.Name = "cmdViewDetails";
            this.cmdViewDetails.Size = new System.Drawing.Size(62, 22);
            this.cmdViewDetails.Text = "Details";
            this.cmdViewDetails.Click += new System.EventHandler(this.cmdViewDetails_Click);
            // 
            // cmdGroup
            // 
            this.cmdGroup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdGroupNone,
            this.cmdGroupTypes,
            this.cmdGroupFolders});
            this.cmdGroup.Image = ((System.Drawing.Image)(resources.GetObject("cmdGroup.Image")));
            this.cmdGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdGroup.Name = "cmdGroup";
            this.cmdGroup.Size = new System.Drawing.Size(69, 22);
            this.cmdGroup.Text = "Group";
            // 
            // cmdGroupNone
            // 
            this.cmdGroupNone.Name = "cmdGroupNone";
            this.cmdGroupNone.Size = new System.Drawing.Size(112, 22);
            this.cmdGroupNone.Text = "None";
            this.cmdGroupNone.Click += new System.EventHandler(this.cmdGroupNone_Click);
            // 
            // cmdGroupTypes
            // 
            this.cmdGroupTypes.Name = "cmdGroupTypes";
            this.cmdGroupTypes.Size = new System.Drawing.Size(112, 22);
            this.cmdGroupTypes.Text = "Types";
            this.cmdGroupTypes.Click += new System.EventHandler(this.cmdGroupTypes_Click);
            // 
            // cmdGroupFolders
            // 
            this.cmdGroupFolders.Name = "cmdGroupFolders";
            this.cmdGroupFolders.Size = new System.Drawing.Size(112, 22);
            this.cmdGroupFolders.Text = "Folders";
            this.cmdGroupFolders.Click += new System.EventHandler(this.cmdGroupFolders_Click);
            // 
            // cmdRecurse
            // 
            this.cmdRecurse.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdRecurseNormal,
            this.cmdRecurseShallow,
            this.cmdRecurseDeep,
            this.cmdRecurseFull});
            this.cmdRecurse.Image = ((System.Drawing.Image)(resources.GetObject("cmdRecurse.Image")));
            this.cmdRecurse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdRecurse.Name = "cmdRecurse";
            this.cmdRecurse.Size = new System.Drawing.Size(68, 22);
            this.cmdRecurse.Text = "Depth";
            // 
            // cmdRecurseNormal
            // 
            this.cmdRecurseNormal.Name = "cmdRecurseNormal";
            this.cmdRecurseNormal.Size = new System.Drawing.Size(115, 22);
            this.cmdRecurseNormal.Text = "Normal";
            this.cmdRecurseNormal.Click += new System.EventHandler(this.cmdRecurseNormal_Click);
            // 
            // cmdRecurseShallow
            // 
            this.cmdRecurseShallow.Name = "cmdRecurseShallow";
            this.cmdRecurseShallow.Size = new System.Drawing.Size(115, 22);
            this.cmdRecurseShallow.Text = "Shallow";
            this.cmdRecurseShallow.Click += new System.EventHandler(this.cmdRecurseShallow_Click);
            // 
            // cmdRecurseDeep
            // 
            this.cmdRecurseDeep.Name = "cmdRecurseDeep";
            this.cmdRecurseDeep.Size = new System.Drawing.Size(115, 22);
            this.cmdRecurseDeep.Text = "Deep";
            this.cmdRecurseDeep.Click += new System.EventHandler(this.cmdRecurseDeep_Click);
            // 
            // cmdRecurseFull
            // 
            this.cmdRecurseFull.Name = "cmdRecurseFull";
            this.cmdRecurseFull.Size = new System.Drawing.Size(115, 22);
            this.cmdRecurseFull.Text = "Full";
            this.cmdRecurseFull.Click += new System.EventHandler(this.cmdRecurseFull_Click);
            // 
            // cmdCheck
            // 
            this.cmdCheck.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdCheckExists,
            this.cmdCheckSizeDate,
            this.cmdCheckSync});
            this.cmdCheck.Image = ((System.Drawing.Image)(resources.GetObject("cmdCheck.Image")));
            this.cmdCheck.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdCheck.Name = "cmdCheck";
            this.cmdCheck.Size = new System.Drawing.Size(72, 22);
            this.cmdCheck.Text = "Check";
            // 
            // cmdCheckExists
            // 
            this.cmdCheckExists.Name = "cmdCheckExists";
            this.cmdCheckExists.Size = new System.Drawing.Size(166, 22);
            this.cmdCheckExists.Text = "All Files Exist";
            // 
            // cmdCheckSizeDate
            // 
            this.cmdCheckSizeDate.Name = "cmdCheckSizeDate";
            this.cmdCheckSizeDate.Size = new System.Drawing.Size(166, 22);
            this.cmdCheckSizeDate.Text = "Sizes and Dates";
            // 
            // cmdCheckSync
            // 
            this.cmdCheckSync.Name = "cmdCheckSync";
            this.cmdCheckSync.Size = new System.Drawing.Size(166, 22);
            this.cmdCheckSync.Text = "Recursive Refresh";
            // 
            // cmdHash
            // 
            this.cmdHash.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdHashCreate,
            this.cmdHashVerify,
            this.cmdHashLoad,
            this.cmdHashSave});
            this.cmdHash.Image = ((System.Drawing.Image)(resources.GetObject("cmdHash.Image")));
            this.cmdHash.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdHash.Name = "cmdHash";
            this.cmdHash.Size = new System.Drawing.Size(63, 22);
            this.cmdHash.Text = "Hash";
            // 
            // cmdHashCreate
            // 
            this.cmdHashCreate.Name = "cmdHashCreate";
            this.cmdHashCreate.Size = new System.Drawing.Size(108, 22);
            this.cmdHashCreate.Text = "Create";
            this.cmdHashCreate.Click += new System.EventHandler(this.cmdHashCreate_Click);
            // 
            // cmdHashVerify
            // 
            this.cmdHashVerify.Name = "cmdHashVerify";
            this.cmdHashVerify.Size = new System.Drawing.Size(108, 22);
            this.cmdHashVerify.Text = "Verify";
            this.cmdHashVerify.Click += new System.EventHandler(this.cmdHashVerify_Click);
            // 
            // cmdHashLoad
            // 
            this.cmdHashLoad.Name = "cmdHashLoad";
            this.cmdHashLoad.Size = new System.Drawing.Size(108, 22);
            this.cmdHashLoad.Text = "Load";
            // 
            // cmdHashSave
            // 
            this.cmdHashSave.Name = "cmdHashSave";
            this.cmdHashSave.Size = new System.Drawing.Size(108, 22);
            this.cmdHashSave.Text = "Save";
            // 
            // toolbar
            // 
            this.toolbar.Dock = System.Windows.Forms.DockStyle.None;
            this.toolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtQuery,
            this.cmdStart,
            this.space1,
            this.cmdViewList,
            this.cmdViewDetails,
            this.cmdGroup,
            this.cmdRecurse,
            this.space2,
            this.cmdSave,
            this.cmdCheck,
            this.cmdHash});
            this.toolbar.Location = new System.Drawing.Point(3, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(724, 25);
            this.toolbar.TabIndex = 0;
            // 
            // importOnlyToolStripMenuItem
            // 
            this.importOnlyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.photoToolStripMenuItem,
            this.textToolStripMenuItem,
            this.dataToolStripMenuItem});
            this.importOnlyToolStripMenuItem.Name = "importOnlyToolStripMenuItem";
            this.importOnlyToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.importOnlyToolStripMenuItem.Text = "Import Only";
            // 
            // audioToolStripMenuItem
            // 
            this.audioToolStripMenuItem.Name = "audioToolStripMenuItem";
            this.audioToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.audioToolStripMenuItem.Text = "Audio";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // photoToolStripMenuItem
            // 
            this.photoToolStripMenuItem.Name = "photoToolStripMenuItem";
            this.photoToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.photoToolStripMenuItem.Text = "Photo";
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.textToolStripMenuItem.Text = "Text";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // recentOneToolStripMenuItem
            // 
            this.recentOneToolStripMenuItem.Name = "recentOneToolStripMenuItem";
            this.recentOneToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.recentOneToolStripMenuItem.Text = "Recent one";
            // 
            // recentTwoToolStripMenuItem
            // 
            this.recentTwoToolStripMenuItem.Name = "recentTwoToolStripMenuItem";
            this.recentTwoToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.recentTwoToolStripMenuItem.Text = "Recent two";
            // 
            // space2
            // 
            this.space2.Name = "space2";
            this.space2.Size = new System.Drawing.Size(6, 25);
            // 
            // list
            // 
            this.list.BackColor = System.Drawing.SystemColors.Window;
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.FullRowSelect = true;
            listViewGroup1.Header = "Folders";
            listViewGroup1.Name = "grpFolders";
            listViewGroup2.Header = "Text";
            listViewGroup2.Name = "grpText";
            listViewGroup3.Header = "Audio";
            listViewGroup3.Name = "grpAudio";
            listViewGroup4.Header = "Image";
            listViewGroup4.Name = "grpImage";
            listViewGroup5.Header = "Video";
            listViewGroup5.Name = "grpVideo";
            listViewGroup6.Header = "Other";
            listViewGroup6.Name = "grpFiles";
            this.list.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.list.Location = new System.Drawing.Point(0, 0);
            this.list.MultiSelect = false;
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(741, 454);
            this.list.SmallImageList = this.imgSmall;
            this.list.TabIndex = 0;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            // 
            // Default
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 501);
            this.Controls.Add(this.tools);
            this.Name = "Default";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Default_Load);
            this.tools.BottomToolStripPanel.ResumeLayout(false);
            this.tools.BottomToolStripPanel.PerformLayout();
            this.tools.ContentPanel.ResumeLayout(false);
            this.tools.TopToolStripPanel.ResumeLayout(false);
            this.tools.TopToolStripPanel.PerformLayout();
            this.tools.ResumeLayout(false);
            this.tools.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.splitH.Panel1.ResumeLayout(false);
            this.splitH.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitH)).EndInit();
            this.splitH.ResumeLayout(false);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer tools;
        private System.Windows.Forms.SplitContainer splitH;
        private System.Windows.Forms.TreeView tree;
        private Expedition.Win.NavTree.Controls.ExpeditionList list;
        private System.Windows.Forms.ListBox lstRecursive;
        private System.Windows.Forms.ImageList imgSmall;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel lblMessage;
        private System.Windows.Forms.ToolStripProgressBar progress;
        private System.ComponentModel.BackgroundWorker background;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.ToolStripTextBox txtQuery;
        private System.Windows.Forms.ToolStripSplitButton cmdStart;
        private System.Windows.Forms.ToolStripMenuItem cmdStartClearResults;
        private System.Windows.Forms.ToolStripMenuItem cmdStartSelectDir;
        private System.Windows.Forms.ToolStripButton cmdViewList;
        private System.Windows.Forms.ToolStripButton cmdViewDetails;
        private System.Windows.Forms.ToolStripDropDownButton cmdGroup;
        private System.Windows.Forms.ToolStripMenuItem cmdGroupNone;
        private System.Windows.Forms.ToolStripMenuItem cmdGroupTypes;
        private System.Windows.Forms.ToolStripMenuItem cmdGroupFolders;
        private System.Windows.Forms.ToolStripDropDownButton cmdRecurse;
        private System.Windows.Forms.ToolStripMenuItem cmdRecurseNormal;
        private System.Windows.Forms.ToolStripMenuItem cmdRecurseShallow;
        private System.Windows.Forms.ToolStripMenuItem cmdRecurseDeep;
        private System.Windows.Forms.ToolStripMenuItem cmdRecurseFull;
        private System.Windows.Forms.ToolStripSeparator space1;
        private System.Windows.Forms.ToolStripDropDownButton cmdSave;
        private System.Windows.Forms.ToolStripMenuItem cmdExportText;
        private System.Windows.Forms.ToolStripMenuItem cmdExportCSV;
        private System.Windows.Forms.ToolStripSplitButton cmdCheck;
        private System.Windows.Forms.ToolStripMenuItem cmdCheckExists;
        private System.Windows.Forms.ToolStripMenuItem cmdCheckSizeDate;
        private System.Windows.Forms.ToolStripMenuItem cmdCheckSync;
        private System.Windows.Forms.ToolStripDropDownButton cmdHash;
        private System.Windows.Forms.ToolStripMenuItem cmdHashCreate;
        private System.Windows.Forms.ToolStripMenuItem cmdHashVerify;
        private System.Windows.Forms.ToolStripMenuItem cmdHashLoad;
        private System.Windows.Forms.ToolStripMenuItem cmdHashSave;
        private System.Windows.Forms.ToolStripMenuItem importOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem photoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentOneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentTwoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator space2;
    }
}

