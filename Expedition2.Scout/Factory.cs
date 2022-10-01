using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Scout
{
	public class PatrolFactory
	{
		private static int _fileId = 0;
		private static int _folderId = 0;

		public SourcePatrolInfo CreateNew(DirectoryInfo d, string? baseFileName = null)
		{
			var p = new SourcePatrolInfo();
			p.TargetUri = d.FullName;
			p.SourceUri = Path.Combine(d.FullName, baseFileName ?? "YYYYMMDD") + ".patrol";
			p.SourceType = SourceType.FileSystem;
			return p;
		}

		public SourcePatrolInfo StartExisting(FileInfo f)
		{
			var p = new SourcePatrolInfo();
			p.TargetUri = f.Directory?.FullName ?? "";
			p.SourceUri = f.FullName;
			p.SourceType = SourceType.PatrolSource;
			return p;
		}

		public FolderPatrolInfo GetFolder(DirectoryInfo d)
		{
			var x = new FolderPatrolInfo();
			x.Id = ++_folderId;
			x.Guid = Guid.NewGuid();
			x.Uri = d.FullName;
			x.Name = d.Name;

			x.Created = d.CreationTimeUtc;
			x.Updated = d.LastWriteTimeUtc;

			return x;
		}

		public FilePatrolInfo GetFile(FileInfo f)
		{
			var x = new FilePatrolInfo();
			x.Id = ++_fileId;
			x.Guid = Guid.NewGuid();
			x.Uri = f.FullName;

			x.Name = f.Name;
			x.Path = f.Directory?.FullName ?? "";

			x.Extension = f.Extension.ToUpperInvariant();
			x.FileSize = f.Length;

			x.Created = f.CreationTimeUtc;
			x.Updated = f.LastWriteTimeUtc;

			return x;
		}

		public IEnumerable<FolderPatrolInfo> LoadFolders(string uri, bool recursive = false)
		{
			var dir = new DirectoryInfo(uri);
			return dir.EnumerateDirectories("*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(x => GetFolder(x));
		}

		public IEnumerable<FilePatrolInfo> LoadFiles(string uri, bool recursive = false, string? filter = null)
		{
			var dir = new DirectoryInfo(uri);
			return dir.EnumerateFiles(filter ?? "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(x => GetFile(x));
		}
	}
}
