using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
using CommandLine;
using Expedition2.Core2;

namespace Expedition2.Scout
{
	public class CommonOptions
	{
		[Value(0)]
		public string? Name { get; set; }

		[Option('s', "silent", HelpText = "Suppresses detailed console output")]
		public bool Silent { get; set; }

		[Option('p', "pause", HelpText = "Pause on file system errors")]
		public bool Pause { get; set; }

		[Option('l', "log", HelpText = "Create a simple text log")]
		public bool Log { get; set; }

		[Option('e', "error", HelpText = "Create an error log only")]
		public bool Error { get; set; }

		[Option('r', "report", HelpText = "Create a detailed report in Excel")]
		public bool Report { get; set; }

		/*
		[Option('t', "threads", HelpText = "Set the number of threads for hashing", Min =1, Max =64)]
		public int Threads { get; set; }
		*/
	}

	[Verb("create", HelpText = "Creates a new hash report")]
	public class CreateOptions : CommonOptions
	{
		[Option("md5")]
		public bool Md5 { get; set; }

		[Option("sha1")]
		public bool Sha1 { get; set; }

		[Option("sha512")]
		public bool Sha512 { get; set; }

		[Option('f', "filter", HelpText = "Filename wildcard filter")]
		public string? Filter { get; set; }
	}

	[Verb("verify", HelpText = "Verifies an existing hash report")]
	public class VerifyOptions : CommonOptions
	{
	}

	// async and crypto https://stackoverflow.com/questions/48897692/how-does-asynchronous-file-hash-and-disk-write-actually-work

	internal class Engine
	{
		public static void DoCommon(CommonOptions c)
		{
			Console.WriteLine($"Name:\t{c.Name}");
			Console.WriteLine($"Silent:\t{c.Silent}");
			Console.WriteLine($"Pause:\t{c.Pause}");
			Console.WriteLine($"Log:\t{c.Log}");
			Console.WriteLine($"Error:\t{c.Error}");
			Console.WriteLine($"Report:\t{c.Report}");
		}

		public static void DoCreate(CreateOptions c)
		{
			Console.WriteLine($"Create");
			DoCommon(c);

			//Console.WriteLine($"Hash:\t{c.HashType.ToString().Pastel("009900")}");
			var filter = c.Filter ?? "";
			Console.WriteLine($"Filter:\t{filter}");

			Console.WriteLine($"==================================================");

			var patrol = new SourcePatrolInfo();
			/*
			if (String.IsNullOrEmpty(c.Name))
			{
				c.Name = "YYYYMMDD-HHMMSS";
				var start = new DirectoryInfo(".");
				patrol.TargetUri = start.FullName;
				patrol.SourceUri = Path.Combine(start.FullName, c.Name);
			}
			*/
			var sourceUri = Directory.Exists(c.Name) ? c.Name : ".";						

			// if name is existing directory and default name to yyyymmdd-hhmmss

			// if name file exist, error in create mode
			// if name does not exist, default name to yyyymmdd-hhmmss
			// if name file does not exist, create patrol file name base

			var root = new DirectoryInfo(sourceUri);
			Console.WriteLine(root.FullName);
			

			var dirs = root.EnumerateDirectories("", SearchOption.AllDirectories);
			Console.WriteLine($"Folders:\t{dirs.Count()}");

			List<FolderPatrolInfo> folders = dirs.Select(x => GetFolder(x)).ToList();
			List<FilePatrolInfo> files = new List<FilePatrolInfo>();			
			
			foreach(var folder in folders)
			{
				Console.WriteLine($"--------------------------------------------------");
				Console.WriteLine(folder.Uri);				
				var dir = new DirectoryInfo(folder.Uri);
				
				patrol.TotalFolderCount++;

				var dirFiles = dir.EnumerateFiles(filter);
				Console.WriteLine($"Files:\t{dirFiles.Count()}");

				var list = dirFiles.Select(x => GetFile(x));
				files.AddRange(list);
				foreach (var file in list)
				{
					folder.TotalFileCount++;
					folder.TotalFileSize += file.FileSize;
					Console.WriteLine($"\t{file.Name}");
				}

				patrol.TotalFileCount += folder.TotalFileCount;
				patrol.TotalFileSize += folder.TotalFileSize;
			}

			Console.WriteLine($"==================================================");
			Console.WriteLine($"Target Output File:\t{patrol.SourceUri}");
			Console.WriteLine($"Total Folder Count:\t{patrol.TotalFolderCount}");
			Console.WriteLine($"Total File Count:\t{patrol.TotalFileCount}");
			Console.WriteLine($"Total File Size:\t{patrol.TotalFileSize}");
		}

		public static FolderPatrolInfo GetFolder(DirectoryInfo d)
		{
			var x = new FolderPatrolInfo();
			x.Uri = d.FullName;
			x.Name = d.Name;

			x.Created = d.CreationTimeUtc;
			x.Updated = d.LastWriteTimeUtc;

			return x;
		}


		public static FilePatrolInfo GetFile(FileInfo f)
		{
			var x = new FilePatrolInfo();
			x.Uri = f.FullName;
			x.Name = f.Name;
			
			x.Extension = f.Extension.ToUpperInvariant();
			x.FileSize = f.Length;

			x.Created = f.CreationTimeUtc;
			x.Updated = f.LastWriteTimeUtc;

			return x;
		}


		public static void DoVerify(VerifyOptions v)
		{
			Console.WriteLine($"Verify");
			DoCommon(v);
		}
	}
}
