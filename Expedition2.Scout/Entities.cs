using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Scout
{
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
		public DateTime? FirstStored;
		public DateTime? LastVerified;

		//bool FileOrFolder;  // Drive/Root/Folder/File
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
		public string Extension;
		//public string FileType;
		public long FileSize;

		public string Md5;
		string Sha1;
		public string Sha512;

		int FileStatus;     // enum flags
		int HashStatus;     // enum flags

		bool IsStatic;      // file should never change (camera raws)
		bool IsUnique;      // file name is unique (enough to trust)
		bool IsConstant;    // file key/path/name is globally unique
		bool IsExcluded;    // ignore this file

		string Notes;
		string Tags;

		//SourcePatrolInfo Patrol;
	}

	public class SourcePatrolInfo
	{
		Guid PatrolGuid;

		public string SourceUri;	// full uri to the patrol metadata source of truth file
		public string TargetUri;    // full uri to assoicated directory on live filestream

		public SourceType SourceType;	// FileSystem, PatrolSource
		string HashType;	// Md5, Sha1, Sha512
		
		public long TotalFolderCount;
		public long TotalFileCount;
		public long TotalFileSize;
	}

	public enum SourceType { FileSystem, PatrolSource };
}
