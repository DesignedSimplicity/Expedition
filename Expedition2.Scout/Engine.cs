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

		[Option('s', "stop", HelpText = "Stop on any error")]
		public bool Stop { get; set; }

		[Option('l', "log", HelpText = "Create a simple text log")]
		public bool Log { get; set; }

		[Option('e', "error", HelpText = "Create an error log only")]
		public bool Error { get; set; }

		[Option('r', "report", HelpText = "Create a detailed report in Excel")]
		public bool Report { get; set; }
	}

	[Verb("create", HelpText = "Creates a new hash report")]
	public class CreateOptions : CommonOptions
	{
		[Option('h', "hash", Default = HashType.Md5)]
		public HashType HashType { get; set; }

		[Option('f', "filter", HelpText = "Filename wildcard filter")]
		public string? Filter { get; set; }
	}

	[Verb("verify", HelpText = "Verifies an existing hash report")]
	public class VerifyOptions : CommonOptions
	{
	}

	internal class Engine
	{
		public static void DoCommon(CommonOptions c)
		{
			Console.WriteLine($"Name:\t{c.Name}");
			Console.WriteLine($"Stop:\t{c.Stop}");
			Console.WriteLine($"Log:\t{c.Log}");
			Console.WriteLine($"Error:\t{c.Error}");
			Console.WriteLine($"Report:\t{c.Report}");
			
		}

		public static void DoCreate(CreateOptions c)
		{
			Console.WriteLine($"Create");
			DoCommon(c);

			Console.WriteLine($"Hash:\t{c.HashType.ToString().Pastel("009900")}");
			Console.WriteLine($"Filter:\t{c.Filter}");


			var uri = Directory.Exists(c.Name) ? c.Name : ".";
			var dir = new DirectoryInfo(uri);
			var dirs = dir.EnumerateDirectories("", SearchOption.AllDirectories);
			Console.WriteLine($"Directories:\t{dirs.Count()}");
		}

		public static void DoVerify(VerifyOptions v)
		{
			Console.WriteLine($"Verify");
			DoCommon(v);
		}

	}
}
