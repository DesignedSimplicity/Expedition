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
			Console.WriteLine($"Name:\t{c.StartUri}");
			//Console.WriteLine($"Silent:\t{c.Silent}");
			//Console.WriteLine($"Pause:\t{c.Pause}");
			Console.WriteLine($"Log:\t{c.Log}");
			//Console.WriteLine($"Error:\t{c.Error}");
			Console.WriteLine($"Report:\t{c.Report}");
			Console.WriteLine($"Report:\t{c.Preview}");
		}

		public void DoVerify(VerifyOptions v)
		{
			Console.WriteLine($"Verify");
			DoCommon(v);
		}




		public void DoCreate(CreateOptions c)
		{
			DoCommon(c);

			Console.WriteLine($"Create");
			Console.WriteLine($"Filter:\t{c.Filter}");

			// access source directory if exists, assume current if null
			var sourceUri = "";
			if (String.IsNullOrWhiteSpace(c.StartUri))
				sourceUri = ParsePath.FixUri(Environment.CurrentDirectory, true);
			else if (c.StartUri.Contains(Path.DirectorySeparatorChar) && Directory.Exists(c.StartUri))
				sourceUri = ParsePath.FixUri(c.StartUri, true);
			else
				sourceUri = c.StartUri;

			// prepare service request
			var request = new CreateChecksumsRequest()
			{
				NamePrefix = c.Name,
				HashType = c.Sha512 ? HashType.Sha512 : HashType.Md5,
				DirectoryUri = sourceUri,
				RelativeToUri = sourceUri,
				FilePattern = c.Filter ?? "*.*",
				Report = c.Report,
				Preview = c.Preview,
				ConsoleOut = Console.Out,
				//LogStream = Console.Out,
				Recursive = true,
				ErrorSafe = true,
		};

			// create and execute service request
			var create = new CreateChecksums();
			var response = create.Execute(request);

			// TODO do something with list of patrolfiles

		}
	}
}
