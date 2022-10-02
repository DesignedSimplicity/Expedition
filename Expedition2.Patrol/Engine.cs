using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pastel;
using CommandLine;
using Expedition2.Core;
using System.Drawing;
using Expedition2.Core.Services;
using OfficeOpenXml.Utils;
using OfficeOpenXml.Style;

namespace Expedition2.Patrol
{
	// async and crypto https://stackoverflow.com/questions/48897692/how-does-asynchronous-file-hash-and-disk-write-actually-work

	internal class Engine
	{
		private readonly Factory _factory = new Factory();

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




		private void DisplayPatrolSummary(SourcePatrolInfo patrol)
		{
			// display patrol estimate
			Console.WriteLine($"==================================================");
			Console.WriteLine($"Patrol Source Uri:\t{patrol.SourcePatrolUri}");
			Console.WriteLine($"Folder Target Uri:\t{patrol.TargetFolderUri}");
			Console.WriteLine($"Total Folder Count:\t{patrol.TotalFolderCount:###,###,###,###,###,##0}");
			Console.WriteLine($"Total File Count:\t{patrol.TotalFileCount:###,###,###,###,###,##0}");
			Console.WriteLine($"Total File Size:\t{patrol.TotalFileSize:###,###,###,###,###,##0}".Pastel(Color.DarkGray));
			Console.WriteLine($"");
		}


		public void DoCreate(CreateOptions c)
		{
			DoCommon(c);

			Console.WriteLine($"Create");
			Console.WriteLine($"Filter:\t{c.Filter}");
			Console.WriteLine($"==================================================");

			// access source directory if exists, assume current if null
			var sourceUri = Directory.Exists(c.Name) ? c.Name : ".";

			// prepare service request
			var request = new CreateChecksumsRequest()
			{
				HashType = c.Sha512 ? HashType.Sha512 : HashType.Md5,
				DirectoryUri = sourceUri,
				RelativeToUri = sourceUri,
				FilePattern = c.Filter ?? "",
				Report = c.Report,
				Preview = false,
				//LogStream = Console.Out,
				ConsoleOut = Console.Out,
				Recursive = true,
			};

			// create and execute service request
			var create = new CreateChecksums();
			var response = create.Execute(request);

			// TODO do something with list of patrolfiles

		}






		public void DoCreate2(CreateOptions c)
		{
			DoCommon(c);
			Console.WriteLine($"Create");

			var filter = c.Filter ?? "";
			Console.WriteLine($"Filter:\t{filter}");
			//Console.WriteLine($"Hash:\t{c.HashType.ToString().Pastel("009900")}");
			Console.WriteLine($"==================================================");

			// access source directory if exists, assume current if null
			var sourceUri = Directory.Exists(c.Name) ? c.Name : ".";
			var root = new DirectoryInfo(sourceUri);
			var patrol = _factory.CreateNew(root);
			Console.WriteLine(root.FullName);

			// find all subdirectories, recursive if specified
			IEnumerable<FolderPatrolInfo> folders = _factory.LoadFolders(root.FullName, true);
			Console.WriteLine($"Folders:\t{folders.Count():###,###,###,###,###,##0}");

			// gather files for each folder
			List<FilePatrolInfo> files = new List<FilePatrolInfo>();
			foreach (var folder in folders)
			{
				Console.WriteLine($"--------------------------------------------------");
				Console.Write(folder.Uri.Pastel(Color.Yellow));

				// load all files in this directory
				var list = _factory.LoadFiles(folder.Uri, false, filter);

				// account for files
				files.AddRange(list);
				folder.TotalFileCount = list.Count();
				folder.TotalFileSize += list.Sum(x => x.FileSize);

				// update source patrol
				patrol.TotalFolderCount++;
				patrol.TotalFileCount += folder.TotalFileCount;
				patrol.TotalFileSize += folder.TotalFileSize;

				// show output
				Console.WriteLine($"\tFiles: {folder.TotalFileCount:###,###,###,###,###,##0}\tSize: {folder.TotalFileSize:###,###,###,###,###,##0}");
				foreach (var file in list)
				{
					Console.WriteLine(file.Name.Pastel(Color.Orange));
				}
			}

			// display prepre summary
			DisplayPatrolSummary(patrol);

			// update status
			Console.WriteLine($"==================================================");
			Console.WriteLine($"Start Hash");

			// start loop			
			var hasher = new Hasher();
			foreach (var file in files)
			{
				Console.Write($"{file.Path}\\{file.Name.Pastel(Color.WhiteSmoke)}\t");
				Console.Write($"{file.FileSize:###,###,###,###,###,##0}".Pastel(Color.DarkGray));

				file.Md5 = hasher.GetHash(file.Uri, HashType.Md5);
				Console.Write($"\t{file.Md5.Pastel(Color.Green)}");

				/*
				file.Sha512 = hasher.GetHash(file.Uri, HashType.Sha512);
				Console.Write($"\t{file.Sha512.Pastel(Color.LightGreen)}");
				*/
				Console.WriteLine();

				file.LastVerified = DateTime.UtcNow;
			}
			hasher.Dispose();


			// TODO: create output file as expected




		}
	}
}
