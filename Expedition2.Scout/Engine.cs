﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
using CommandLine;
using Expedition2.Core2;

namespace Expedition2.Scout
{
	// async and crypto https://stackoverflow.com/questions/48897692/how-does-asynchronous-file-hash-and-disk-write-actually-work

	internal class PatrolEngine
	{
		private readonly PatrolFactory _factory = new PatrolFactory();

		public void DoCommon(CommonOptions c)
		{
			Console.WriteLine($"Name:\t{c.Name}");
			Console.WriteLine($"Silent:\t{c.Silent}");
			Console.WriteLine($"Pause:\t{c.Pause}");
			Console.WriteLine($"Log:\t{c.Log}");
			Console.WriteLine($"Error:\t{c.Error}");
			Console.WriteLine($"Report:\t{c.Report}");
		}

		public void DoVerify(VerifyOptions v)
		{
			Console.WriteLine($"Verify");
			DoCommon(v);
		}


		public void DoCreate(CreateOptions c)
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
			Console.WriteLine($"Folders:\t{dirs.Count():###,###,###,###,###,##0}");

			IEnumerable<FolderPatrolInfo> folders = _factory.LoadFolders(root.FullName, true);
			List<FilePatrolInfo> files = new List<FilePatrolInfo>();			
			
			foreach(var folder in folders)
			{
				Console.WriteLine($"--------------------------------------------------");
				Console.WriteLine(folder.Uri);				
				var dir = new DirectoryInfo(folder.Uri);
				
				patrol.TotalFolderCount++;

				var list = _factory.LoadFiles(folder.Uri, false, filter);
				Console.WriteLine($"Files:\t{list.Count()}");
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
			Console.WriteLine($"Total Folder Count:\t{patrol.TotalFolderCount:###,###,###,###,###,##0}");
			Console.WriteLine($"Total File Count:\t{patrol.TotalFileCount:###,###,###,###,###,##0}");
			Console.WriteLine($"Total File Size:\t{patrol.TotalFileSize:###,###,###,###,###,##0}");
			Console.WriteLine($"");

			Console.WriteLine($"==================================================");
			Console.WriteLine($"Start Hash");
			var md5 = Hasher.GetHashAlgorithm(HashType.Md5);
			var sha512 = Hasher.GetHashAlgorithm(HashType.Sha512);
			foreach (var file in files)
			{
				Console.Write($"{file.Uri}\t{file.FileSize:###,###,###,###,###,##0}");

				file.Md5 = Hasher.GetHash(file.Uri, md5);
				Console.Write($"\t{file.Md5}");

				file.Sha512 = Hasher.GetHash(file.Uri, sha512);
				Console.WriteLine($"\t{file.Sha512}");

				file.FirstStored = file.LastVerified = DateTime.UtcNow;				
			}
		}
	}
}
