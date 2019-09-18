using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Expedition.Win.NavTree.Framework;

namespace Expedition.Win.NavTree.Controls
{
    public class ExpeditionList : ListView
    {
        public enum GroupMode { None, Type, Size, Dates, Folder, Pattern }
        public enum SortMode { Name, Path, Size, Dates }
        public int Depth = 0; // -1 = full recursive, 0 = this folder, 1 = include subfolders
        // SimpleList, SimpleListWithFolders, SimpleDetailsWithFolders

        // List vs Details
        // Flat vs Groups
        // Simple vs Folders vs Recursive

        protected bool _recursive = false;
        protected bool _grouped = true;
        protected bool _folders = true;

        public ExpeditionList()
        {
            this.Columns.Clear();
            this.Columns.Add("Name", 300);
            this.Columns.Add("Type", 50);
            this.Columns.Add("Size", 75);
            this.Columns.Add("Last Modified", 200);

            this.Resize += ExpeditionList_Resize;
        }

        protected void ExpeditionList_Resize(object sender, EventArgs e)
        {
            int colWidths = 0;
            for (int i = 1; i < this.Columns.Count; i++)
            {
                colWidths += this.Columns[i].Width;
            }
            int width = this.Width - colWidths - 21; // for scroll bar
            this.Columns[0].Width = width > 100 ? width : 100;
        }

        public void ShowFolder(DossierFolderInfo folder, int depth = 0, GroupMode mode = GroupMode.None)
        {
            this.BeginUpdate();
            this.Items.Clear();
            this.Groups.Clear();

            InitGroups(mode);

            this.Items.Add(GetFolderItem(folder, mode, true));

            List<ListViewItem> sort = new List<ListViewItem>();
            foreach (DossierFileInfo file in folder.Files)
            {
                sort.Add(GetFileItem(file, mode));
            }

            int recurse = (depth == -1 ? int.MaxValue : depth);
            if (recurse > 0)
            {
                foreach (DossierFolderInfo sub in folder.Folders)
                {
                    this.Items.Add(GetFolderItem(sub, mode));
                    sort.AddRange(GetFileItems(sub, recurse - 1, mode));
                }
            }

            this.Items.AddRange(sort.OrderBy(i => i.Text).ToArray());

            this.EndUpdate();
        }

        protected List<ListViewItem> GetFileItems(DossierFolderInfo folder, int recurse, GroupMode mode)
        {
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (DossierFileInfo file in folder.Files)
            {
                items.Add(GetFileItem(file, mode));
            }

            if (recurse > 1)
            {
                foreach (DossierFolderInfo sub in folder.Folders)
                {
                    items.AddRange(GetFileItems(sub, recurse - 1, mode));
                }
            }
            return items;
        }

        protected ListViewItem GetFolderItem(DossierFolderInfo folder, GroupMode mode, bool root = false)
        {
            ListViewItem item = new ListViewItem();
            item.Text = root ? folder.FullPath : folder.Name;
            item.Tag = folder;
            
            if (this.View == View.Details)
            {
                item.SubItems.Add("Folder");
                item.SubItems.Add(Dossier.BytesDisplayText(root ? folder.ReportRecursive.GrandTotal.TotalSize : folder.Report.GrandTotal.TotalSize));
            }

            if (mode == GroupMode.Type || mode == GroupMode.Folder) item.Group = GetGroup("Folders");

            item.ImageKey = root ? "open" : "closed";
            item.ForeColor = Color.White;
            item.BackColor = root ? Color.Black : Color.Gray;
            return item;
        }

        protected ListViewItem GetFileItem(DossierFileInfo file, GroupMode mode)
        {
            ListViewItem item = new ListViewItem();
            item.Text = file.Name;
            item.Tag = file;

            if (file.IsSystem)
                item.ForeColor = Color.Red;
            else if (file.IsHidden)
                item.ForeColor = Color.DarkGray;
            else if (file.IsEncrypted)
                item.ForeColor = Color.Green;
            else if (file.IsCompressed)
                item.ForeColor = Color.Blue;

            if (file.IsVerified)
                item.BackColor = Color.Honeydew;
            else if (file.Hash != Guid.Empty)
                item.BackColor = Color.LemonChiffon;

            item.ImageKey = file.Type.ToString();

            if (this.View == View.Details)
            {
                item.SubItems.Add(file.Type.ToString());
                item.SubItems.Add(Dossier.BytesDisplayText(file.Size));
                item.SubItems.Add(file.DisplayDate);
            }

            if (mode == GroupMode.Folder)
            {
                DossierFolderInfo root = (DossierFolderInfo)this.Items[0].Tag;
                int length = file.Parent.FullPath.Length;
                if (length > root.FullPath.Length) length = root.FullPath.Length + 1;
                item.Group = this.GetGroup(file.Parent.FullPath.Substring(length));
            }
            else if (mode == GroupMode.Type)
            {
                if (file.Type == DossierTypes.Text)
                    item.Group = this.GetGroup(DossierTypes.Text.ToString());
                else if (file.Type == DossierTypes.Audio)
                    item.Group = this.GetGroup(DossierTypes.Audio.ToString());
                else if (file.Type == DossierTypes.Image)
                    item.Group = this.GetGroup(DossierTypes.Image.ToString());
                else if (file.Type == DossierTypes.Video)
                    item.Group = this.GetGroup(DossierTypes.Video.ToString());
                else
                    item.Group = this.GetGroup(DossierTypes.File.ToString());
            }
            else
                item.Group = null;

            return item;
        }

        protected void InitGroups(GroupMode mode)
        {
            switch (mode)
            {
                case GroupMode.None:
                    break;
                case GroupMode.Folder:
                    this.Groups.Add("Folders", "Folders");
                    break;
                case GroupMode.Type:
                    this.Groups.Add("Folders", "Folders");
                    this.Groups.Add("Text", "Text");
                    this.Groups.Add("Audio", "Audio");
                    this.Groups.Add("Image", "Image");
                    this.Groups.Add("Video", "Video");
                    this.Groups.Add("Other", "Other");
                    break;
                case GroupMode.Size:
                    this.Groups.Add("Small", "Small 0 - 9 MB");
                    this.Groups.Add("Medium", "Medium 10 - 99 MB");
                    this.Groups.Add("Large", "Large 100 MB - 999 GB");
                    this.Groups.Add("Huge", "Huge > 1 GB");
                    break;
                default:
                    throw new NotImplementedException(String.Format("GroupMode {0} not implemented", mode));

            }
        }

        protected ListViewGroup GetGroup(string name)
        {
            foreach (ListViewGroup group in this.Groups)
            {
                if (group.Name == name) return group;
            }
            return this.Groups.Add(name, name);
        }
    }
}
