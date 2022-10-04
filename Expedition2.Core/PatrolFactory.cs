using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Expedition2.Core;

namespace Expedition2.Core
{
	public class PatrolFactory
	{
		public SourcePatrolInfo CreateNew(DirectoryInfo d, string? baseFileName = null)
		{
			var name = baseFileName ?? DateTime.Now.ToString("yyyyMMdd-HHmmss");
			
			var p = new SourcePatrolInfo();
			p.PatrolName = name;
			p.HashType = HashType.Md5;

			p.TargetFolderUri = d.FullName;
			p.SourcePatrolUri = Path.Combine(d.FullName, name) + ".patrol";
			p.SourceType = SourceType.FileSystem;

			p.SystemName = Environment.MachineName;

			var now = DateTime.UtcNow;
			p.Created = now;
			p.Updated = now;

			return p;
		}

		public SourcePatrolInfo StartExisting(FileInfo f)
		{
			var p = new SourcePatrolInfo();
			p.PatrolName = f.Name;
			p.HashType = HashType.Md5;

			p.TargetFolderUri = f.Directory?.FullName ?? "";
			p.SourcePatrolUri = f.FullName;
			p.SourceType = SourceType.PatrolSource;

			p.SystemName = Environment.MachineName;

			return p;
		}

		public FolderPatrolInfo GetFolder(DirectoryInfo d)
		{
			return new FolderPatrolInfo(d);
		}

		public FilePatrolInfo GetFile(FileInfo f)
		{
			return new FilePatrolInfo(f);
		}

		public IEnumerable<FolderPatrolInfo> LoadFolders(string uri, bool recursive = false)
		{
			return new DirectoryInfo(uri).EnumerateDirectories("*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(x => GetFolder(x));
		}

		public IEnumerable<FilePatrolInfo> LoadFiles(string uri, bool recursive = false, string? filter = null)
		{
			return new DirectoryInfo(uri).EnumerateFiles(filter ?? "*.*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Select(x => GetFile(x));
		}
	}
}
