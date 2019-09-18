using System;
using System.Collections.Generic;
using System.IO;

namespace Expedition.Win.NavTree.Framework
{
    //====================================================================================================
    public abstract class DossierInfo
    {
        public static long _nextID = 0;

        public long ID;
        public Guid GUID;

        public DossierTypes Type;
        public DossierStatus Status;
        public DossierRootInfo Root;
        public DossierFolderInfo Parent;

        public string Name;
        public long Size = -1;
        public Guid Hash = Guid.Empty;
        public FileAttributes Attributes;

        public bool IsPresent = true;
        public bool IsMatching = true;
        public bool IsVerified = false;

        public DateTime Created;
        public DateTime? Modified;
        public DateTime? Verified;

        public bool IsSystem { get { return (this.Attributes & FileAttributes.System) == FileAttributes.System; } }
        public bool IsHidden { get { return (this.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden; } }
        public bool IsArchive { get { return (this.Attributes & FileAttributes.Archive) == FileAttributes.Archive; } }
        public bool IsReadOnly { get { return (this.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly; } }
        public bool IsEncrypted { get { return (this.Attributes & FileAttributes.Encrypted) == FileAttributes.Encrypted; } }
        public bool IsCompressed { get { return (this.Attributes & FileAttributes.Compressed) == FileAttributes.Compressed; } }

        public string DisplayName { get { return null; } }
        public string DisplayType { get { return null; } }
        public string DisplaySize { get { return null; } }
        public string DisplayDate { get { return this.Modified.HasValue ? this.Modified.ToString() : this.Created.ToString(); } }
        public string DisplayInfo { get { return null; } }

        //--------------------------------------------------
        public DossierInfo()
        {
            ID = GetNextID();
            GUID = Guid.NewGuid();
        }

        public virtual string FullPath
        {
            get { return Path.Combine(Parent.FullPath, Name); }
        }

        //--------------------------------------------------
        protected string BuildFullPath(DossierInfo info)
        {
            List<string> paths = new List<string>();
            while (info != null)
            {
                paths.Add(info.Name);
                info = info.Parent;
            }
            paths.Reverse();
            string fullPath = "";
            foreach (string path in paths)
            {
                fullPath = Path.Combine(fullPath, path);
            }
            return fullPath;
        }

        //--------------------------------------------------
        public static long GetNextID()
        {
            return _nextID++;
        }
    }

    //====================================================================================================
    public class DossierDriveInfo : DossierInfo
    {
        protected string _driveName;
        protected long _totalSpace;
        protected long _freeSpace;

        //--------------------------------------------------
        public char DriveLetter { get { return Name[0]; } }
        public string DriveName { get { return _driveName; } }
    }

    //====================================================================================================
    public class DossierRootInfo : DossierFolderInfo
    {
        protected DossierDriveInfo _drive;

        //--------------------------------------------------
        public DossierRootInfo(DirectoryInfo root) : base(root)
        {
            this.Status = DossierStatus.Normal;
            this.Type = DossierTypes.Root;
            this.Name = root.FullName;
            this.Attributes = root.Attributes;
            this.Created = root.CreationTimeUtc;
            if (root.CreationTimeUtc != root.LastWriteTimeUtc) this.Modified = root.LastWriteTimeUtc;

            this._fullPathCache = root.FullName;
        }

        public string FolderName
        {
            get
            {
                int index = this.Name.LastIndexOf(@"\");
                if (index > 1)
                    return this.Name.Substring(index + 1);
                else
                    return @"\";
            }
        }
    }

    //====================================================================================================
    public class DossierFolderInfo : DossierInfo
    {
        protected List<DossierFolderInfo> _folders;
        protected List<DossierFileInfo> _files;
        protected DossierReport _report;
        protected string _fullPathCache;

        //--------------------------------------------------
        public DossierFolderInfo(string dir)
        {
            this.Status = DossierStatus.Unknown;
            this.Type = DossierTypes.Folder;
            this.Name = dir;
        }
        public DossierFolderInfo(DirectoryInfo dir)
        {
            this.Status = DossierStatus.Normal;
            this.Type = DossierTypes.Folder;
            this.Name = dir.Name;
            this.Attributes = dir.Attributes;
            this.Created = dir.CreationTimeUtc;
            if (dir.CreationTimeUtc != dir.LastWriteTimeUtc) this.Modified = dir.LastWriteTimeUtc;
        }

        //--------------------------------------------------
        public List<DossierFolderInfo> Folders
        {
            get { if (_folders == null) { _folders = new List<DossierFolderInfo>(); } return _folders; }
            set { _folders = value; }
        }

        public List<DossierFileInfo> Files
        {
            get { if (_files == null) { _files = new List<DossierFileInfo>(); } return _files; }
            set { _files = value; }
        }

        public DossierReport Report
        {
            get { if (_report == null) _report = new DossierReport(this.Files); return _report; }
        }

        public DossierReport ReportRecursive
        {
            get
            {
                DossierReport recursive = new DossierReport(this.Files);
                foreach (DossierFolderInfo folder in this.Folders)
                {
                    recursive.Add(folder.ReportRecursive);
                }
                return recursive;
            }
        }

        public override string FullPath
        {
            get
            {
                if (_fullPathCache.IsNullOrWhiteSpace()) _fullPathCache = BuildFullPath(this);
                return _fullPathCache;
            }
        }

        //--------------------------------------------------
        public List<DossierFileInfo> GetAllFiles()
        {
            List<DossierFileInfo> list = new List<DossierFileInfo>();
            list.AddRange(_files);
            foreach (DossierFolderInfo folder in this._folders)
            {
                list.AddRange(folder.GetAllFiles());
            }
            return list;
        }
    }

    //====================================================================================================
    public class DossierFileInfo : DossierInfo
    {
        public DossierFileInfo(string file)
        {
            this.Name = file;

            this.Status = DossierStatus.Unknown;
            this.Type = Dossier.GetFileType(this.Extension);
        }
        public DossierFileInfo(FileInfo file)
        {
            this.Name = file.Name;
            this.Size = file.Length;
            this.Attributes = file.Attributes;
            this.Created = file.CreationTimeUtc;
            if (file.CreationTimeUtc != file.LastWriteTimeUtc) this.Modified = file.LastWriteTimeUtc;

            this.Status = DossierStatus.Normal;
            this.Type = Dossier.GetFileType(this.Extension);
        }

		//--------------------------------------------------
		public string NameWithoutExtension
		{
			get { return Dossier.GetFileNameWithoutExtension(Name); ; }
		}
		public string Extension
		{
			get { return Dossier.GetFileExtension(Name); ; }
		}
		public bool IsExtension(string ext)
		{
			return Extension.ToLowerInvariant() == ext.ToLowerInvariant();
		}
	}
}
