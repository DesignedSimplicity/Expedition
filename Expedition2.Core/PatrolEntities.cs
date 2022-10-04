using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core
{
	public enum SourceType { FileSystem, PatrolSource };
	public enum HashType { Md5 = 0, Sha1 = 1, Sha512 = 512}
	public enum HashStatus { Unknown = 0, Created = 1, Updated = 2, Verified = 3 }
	public enum FileStatus { Unknown = 0, Exists = 1, Missing = -1, Error = -9999 }

	public class SourcePatrolInfo
	{
		public Guid Guid = Guid.NewGuid();

		/// <summary>
		/// Machine name
		/// </summary>
		public string SystemName;

		/// <summary>
		/// Friendly name for patrol metadata set
		/// </summary>
		public string PatrolName;

		/// <summary>
		/// Full uri to the patrol metadata source of truth file
		/// </summary>
		public string SourcePatrolUri;
		
		/// <summary>
		/// Full uri to assoicated directory on live filestream
		/// </summary>
		public string TargetFolderUri;

		public SourceType SourceType;   // FileSystem, PatrolSource
		public HashType HashType;    // Md5, Sha1, Sha512

		public long TotalFolderCount;
		public long TotalFileCount;
		public long TotalFileSize;
		public long TotalSeconds;

		public DateTime Created;
		public DateTime? Updated;
	}

	public class FolderPatrolInfo : CompoundInfo
	{
		// summary
		public long TotalFileCount => Files.Count;
		public long TotalFileSize => Files.Sum(x => x.FileSize);

		//int FolderStatus;	// Offline/Online/Verified/Warning/Errors

		public string Description; // put into aggregate readme file

		//SourcePatrolInfo[] Patrols; // list of hash files associated directly with this folder or file

		public List<FilePatrolInfo> Files = new List<FilePatrolInfo>();

		public FolderPatrolInfo(DirectoryInfo d)
		{
			Guid = Guid.NewGuid();
			Uri = d.FullName;
			Name = d.Name;

			Created = d.CreationTimeUtc;
			Updated = d.LastWriteTimeUtc;

			Description = string.Empty;
		}
	}

	public class FilePatrolInfo : CompoundInfo
	{
		// details
		public string Directory;
		public string Extension;
		//public string FileType;
		public long FileSize;

		public string Md5 = string.Empty;
		public string Sha512 = string.Empty;

		public FileStatus FileStatus = FileStatus.Unknown;
		public HashStatus Md5Status = HashStatus.Unknown;
		public HashStatus Sha512Status = HashStatus.Unknown;

		/*
		bool IsStatic;      // file should never change (camera raws)
		bool IsUnique;      // file name is unique (enough to trust)
		bool IsConstant;    // file key/path/name is globally unique
		bool IsExcluded;    // ignore this file
		*/

		public string Notes;
		public string Tags;

		public FilePatrolInfo(FileInfo f)
		{
			Guid = Guid.NewGuid();
			Uri = f.FullName;

			Name = f.Name;
			Path = f.Directory?.FullName ?? "";
			Directory = f.Directory?.Name ?? "";

			Extension = f.Extension.ToUpperInvariant().TrimStart('.');
			FileSize = f.Length;

			Created = f.CreationTimeUtc;
			Updated = f.LastWriteTimeUtc;

			Notes = string.Empty;
			Tags = string.Empty;
		}
	}

	public abstract class CompoundInfo
	{
		// unique
		public Guid Guid;
		public string Uri;

		// compound
		public string Key => Path + Name;
		public string Path;
		public string Name;

		public DateTime Created;
		public DateTime? Updated;
		//public DateTime? FirstStored;
		public DateTime? LastVerified;

		//bool FileOrFolder;  // Drive/Root/Folder/File
	}

}
