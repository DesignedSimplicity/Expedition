using CommandLine;

namespace Expedition2.Patrol
{
	public class CommonOptions
	{
		[Value(0)]
		public string? StartUri { get; set; }

		[Option('n', "name", HelpText = "Specify the ouput file name")]
		public string Name { get; set; }

		/*
		[Option('s', "silent", HelpText = "Suppresses detailed console output")]
		public bool Silent { get; set; }

		[Option('p', "pause", HelpText = "Pause on file system errors")]
		public bool Pause { get; set; }
		*/

		[Option('l', "log", HelpText = "Create a simple text log")]
		public bool Log { get; set; }

		/*
		[Option('e', "error", HelpText = "Create an error log only")]
		public bool Error { get; set; }
		*/

		[Option('r', "report", HelpText = "Create a detailed report in Excel")]
		public bool Report { get; set; }

		/*
		[Option('t', "threads", HelpText = "Set the number of threads for hashing", Min =1, Max =64)]
		public int Threads { get; set; }
		*/

		[Option('p', "preview", HelpText = "Run in preview only mode", Default = false)]
		public bool Preview { get; set; }
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
}
