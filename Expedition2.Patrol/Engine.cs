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

			// access source directory if exists, assume current if null
			var sourceUri = Directory.Exists(c.Name) ? c.Name : Environment.CurrentDirectory;

			// prepare service request
			var request = new CreateChecksumsRequest()
			{
				HashType = c.Sha512 ? HashType.Sha512 : HashType.Md5,
				DirectoryUri = ParsePath.FixUri(sourceUri),
				RelativeToUri = ParsePath.FixUri(sourceUri),
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
	}
}
