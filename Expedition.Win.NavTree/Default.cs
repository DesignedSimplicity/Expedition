using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Expedition.Win.NavTree.Framework;
using Expedition.Win.NavTree.Controls;

namespace Expedition.Win.NavTree
{
    public partial class Default : Form
    {
        public DossierRootInfo _root;
        public DossierInfo[] _results;
        public DossierFolderInfo _folder;
        public ExpeditionList.GroupMode _mode;
        public EventHandler<DossierEventArgs> _events;
        public int _depth = 0;

        public Default()
        {
            InitializeComponent();
        }
        
        private void Default_Load(object sender, EventArgs e)
        {
            // event handlers
            list.DoubleClick += list_DoubleClick;
            txtQuery.KeyDown += txtQuery_KeyDown;

            background.WorkerReportsProgress = true;
            background.DoWork += background_DoWork;
            background.ProgressChanged += background_ProgressChanged;
            background.RunWorkerCompleted += background_RunWorkerCompleted;

            _events = new EventHandler<DossierEventArgs>(DossierEventHandlerMethod); // new DossierEventHandler(DossierEventHandlerMethod);
        }

        private void background_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.ProgressBar.Value = e.ProgressPercentage;
            lblMessage.Text = e.UserState.ToString();
        }

        private void background_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progress.ProgressBar.Value = 0;
            progress.ProgressBar.Style = ProgressBarStyle.Blocks;
            BuildTreeView((DossierRootInfo)_results[0]);
        }

        public void DossierEventHandlerMethod(object sender, DossierEventArgs args)
        {
            background.ReportProgress(1, args.Message);
        }

        private void background_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime start = DateTime.Now;
            string type = e.Argument.ToString().ToLowerInvariant();
            switch (type)
            {
                case "loaddirectory":
                    string query = txtQuery.Text.Trim();
                    background.ReportProgress(0, "Open: " + query);
                    _results = DossierStream.Dir(query, _events);
                    background.ReportProgress(2, "Count: " + _results.Count().ToString());
                    if (_results.Count() > 0)
                    {
                        DossierRootInfo root = (DossierRootInfo)_results[0];
                        background.ReportProgress(3, "Size: " + Dossier.BytesDisplayText(root.ReportRecursive.GrandTotal.TotalSize));
                    }
                    break;
                case "createmd5":
                case "verifymd5":
                    background.ReportProgress(0, "Start: " + _root.FullPath);
                    IEnumerable<DossierFileInfo> all = _root.GetAllFiles().OrderBy(f => f.FullPath);
                    int total = all.Count();

                    background.ReportProgress(0, "Total: " + total.ToString());
                    int count = 0;
                    foreach (DossierFileInfo file in all)
                    {
                        background.ReportProgress(100 * count / total, "Hash: " + file.FullPath + " @ " + Dossier.BytesDisplayText(file.Size));
                        Guid hash = Dossier.CalculateMd5(file.FullPath);
                        if (type == "createmd5")
                            file.Hash = hash;
                        else if (file.Hash != hash)
                        {
                            background.ReportProgress(100 * count / total, "CORRUPT: " + file.FullPath);
                            break;
                        }
                        file.IsVerified = file.Hash == hash;
                        count++;
                    }
                    break;
            }

            DateTime stop = DateTime.Now;
            TimeSpan time = new TimeSpan(stop.Ticks - start.Ticks);            
            background.ReportProgress(100, String.Format("DONE: {0}:{1:00}", time.Minutes, time.Seconds));
        }

        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                cmdStart_ButtonClick(sender, e);
        }

        private void cmdStart_ButtonClick(object sender, EventArgs e)
        {
            /// http://objectlistview.sourceforge.net/cs/index.html

            string query = txtQuery.Text.Trim();
            if (String.IsNullOrEmpty(query))
                MessageBox.Show("Enter a path or dir query");
            else
            {
                progress.ProgressBar.Value = 0;
                progress.ProgressBar.Style = ProgressBarStyle.Marquee;
                background.RunWorkerAsync("loaddirectory");
                /*
                _results = DossierStream.Dir(query);
                
                if ((_results == null) || (_results.Length < 1))
                    MessageBox.Show("No results!");
                else
                    BuildTreeView((DossierRootInfo)_results[0]);
                */    
            }
        }

        private void cmdHashCreate_Click(object sender, EventArgs e)
        {
            progress.ProgressBar.Value = 0;
            progress.ProgressBar.Style = ProgressBarStyle.Blocks;
            background.RunWorkerAsync("createmd5");
        }

        private void cmdHashVerify_Click(object sender, EventArgs e)
        {
            progress.ProgressBar.Value = 0;
            progress.ProgressBar.Style = ProgressBarStyle.Blocks;
            background.RunWorkerAsync("verifymd5");
        }

        public void BuildTreeView(DossierRootInfo root)
        {
            _root = root;
            _folder = (DossierFolderInfo)root;

            tree.BeginUpdate();
            tree.Nodes.Clear();

            TreeNode start = new TreeNode();
            start.Tag = root;
            start.Text = root.Name;
            start.ImageIndex = 0;
            if (root.ReportRecursive.GrandTotal.TotalSize > Dossier.GetUnitBytes(SizeDisplayUnits.GB)) start.Text += " @ " + Dossier.BytesDisplayText(root.ReportRecursive.GrandTotal.TotalSize, SizeDisplayUnits.GB);
            tree.Nodes.Add(start);

            BuildTreeNode(start.Nodes, root.Folders);

            tree.SelectedNode = start;

            tree.EndUpdate();

        }

        public void BuildTreeNode(TreeNodeCollection nodes, List<DossierFolderInfo> folders)
        {
            if (folders == null) return;
            foreach (DossierFolderInfo folder in folders)
            {
                TreeNode node = new TreeNode();
                node.Tag = folder;
                node.Text = folder.Name;
                node.ImageIndex = 1;
                if (folder.ReportRecursive.GrandTotal.TotalSize > Dossier.GetUnitBytes(SizeDisplayUnits.GB)) node.Text += " @ " + Dossier.BytesDisplayText(folder.ReportRecursive.GrandTotal.TotalSize, SizeDisplayUnits.GB);
                nodes.Add(node);
                BuildTreeNode(node.Nodes, folder.Folders);
            }
        }

        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _folder = (DossierFolderInfo)tree.SelectedNode.Tag;
            RefreshView();
        }

        private void RefreshView()
        {
            list.ShowGroups = _mode != ExpeditionList.GroupMode.None;
            list.ShowFolder(_folder, _depth, _mode);
            RefreshReport(lstRecursive, _folder.ReportRecursive);
        }

        private void RefreshReport(ListBox list, DossierReport report)
        {
            list.BeginUpdate();
            list.Items.Clear();
            long totalCount = 0;
            long totalSize = 0;
            foreach (DossierReportInfo info in report.GetTypeSummaryReport())
            {
                list.Items.Add(String.Format("{0:###,###,##0}\t{1}\t{2}", info.TotalCount, info.Extension, Dossier.BytesDisplayText(info.TotalSize)));
                totalCount += info.TotalCount;
                totalSize += info.TotalSize;
            }
            list.EndUpdate();
        }

        private void cmdViewList_Click(object sender, EventArgs e)
        {
            list.View = View.List;
        }

        private void cmdViewDetails_Click(object sender, EventArgs e)
        {
            list.View = View.Details;
        }

        protected void list_DoubleClick(object sender, EventArgs e)
        {
            if ((tree.SelectedNode != null) && (list.SelectedItems.Count == 1))
            {
                ListViewItem item = list.SelectedItems[0];
                if (item == list.Items[0])
                {
                    if (tree.SelectedNode != tree.Nodes[0])
                    {
                        tree.SelectedNode = tree.SelectedNode.Parent;
                        return;
                    }
                }
                else
                {
                    foreach (TreeNode node in tree.SelectedNode.Nodes)
                    {
                        if (node.Tag == item.Tag)
                        {
                            tree.SelectedNode = node;
                            return;
                        }
                    }
                }
            }
        }

        private void txtQuery_Click(object sender, EventArgs e)
        {
            txtQuery.SelectAll();
        }

        private void cmdRecurseNormal_Click(object sender, EventArgs e)
        {
            _depth = 0;
            RefreshView();
        }

        private void cmdRecurseShallow_Click(object sender, EventArgs e)
        {
            _depth = 1;
            RefreshView();
        }

        private void cmdRecurseDeep_Click(object sender, EventArgs e)
        {
            _depth = 3;
            RefreshView();
        }

        private void cmdRecurseFull_Click(object sender, EventArgs e)
        {
            _depth = -1;
            RefreshView();
        }

        private void cmdGroupNone_Click(object sender, EventArgs e)
        {
            _mode = ExpeditionList.GroupMode.None;
            RefreshView();
        }

        private void cmdGroupTypes_Click(object sender, EventArgs e)
        {
            _mode = ExpeditionList.GroupMode.Type;
            RefreshView();
        }

        private void cmdGroupFolders_Click(object sender, EventArgs e)
        {
            _mode = ExpeditionList.GroupMode.Folder;
            RefreshView();
        }

        private void cmdExportText_Click(object sender, EventArgs e)
        {
            string uri = _root.FullPath + @"\" + _root.FolderName + " " + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".txt";
            DossierStream.ExportTXT(uri, (DossierFolderInfo)_root, false);
        }

        private void cmdExportCSV_Click(object sender, EventArgs e)
        {
            string uri = _root.FullPath + @"\" + _root.FolderName + " " + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".csv";
            DossierStream.ExportCSV(uri, (DossierFolderInfo)_root);
        }

        private void splitH_SplitterMoved(object sender, SplitterEventArgs e)
        {
            txtQuery.TextBox.Width = splitH.SplitterDistance - 4;
            progress.ProgressBar.Width = splitH.SplitterDistance - 3;
        }

        private void cmdStartClearResults_Click(object sender, EventArgs e)
        {
            list.BeginUpdate();
            list.Items.Clear();
            list.EndUpdate();

            tree.BeginUpdate();
            tree.Nodes.Clear();
            tree.EndUpdate();

            lstRecursive.Items.Clear();
        }
    }
}
