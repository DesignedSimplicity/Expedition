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
	public enum FileStatus { Unknown = 0, Exists = 1, Missing = -1 }

	public class SourcePatrolInfo
	{
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
		public long TotalFileCount;
		public long TotalFileSize;

		int FolderStatus;	// Offline/Online/Verified/Warning/Errors

		string Description; // put into aggregate readme file

		//SourcePatrolInfo[] Patrols; // list of hash files associated directly with this folder or file
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

		bool IsStatic;      // file should never change (camera raws)
		bool IsUnique;      // file name is unique (enough to trust)
		bool IsConstant;    // file key/path/name is globally unique
		bool IsExcluded;    // ignore this file

		string Notes;
		string Tags;

		//SourcePatrolInfo Patrol;
	}

	public class CompoundInfo
	{
		// unique
		public int Id;
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
