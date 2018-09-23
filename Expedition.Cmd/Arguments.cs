using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Cmd
{
	public class Arguments
	{
		public bool Parse(string[] args)
		{
			// set defaults
			HashType = Core.HashType.Md5;

			// parse out 
			if (args != null && args.Length > 0)
			{
				foreach (var arg in args)
				{
					var a = arg.ToLowerInvariant().Trim();
					switch (a)
					{
						case ".":
							DirectoryUri = ".";
							break;
						case "-1":
						case "-sha1":
							HashType = Core.HashType.Sha1;
							break;
						case "-p":
						case "-preview":
							RunAsPreview = true;
							break;
						case "-r":
						case "-report":
							CreateReport = true;
							break;
						case "-a":
						case "-absolute":
							UseAbsolutePath = true;
							break;
						default:
							if (Directory.Exists(arg))
							{
								DirectoryUri = arg;
							}
							else if (a.EndsWith(".md5") || a.EndsWith(".sha1"))
							{
								FileUri = arg.Trim();
							}
							else if (File.Exists(a + ".md5"))
							{
								FileUri = arg + ".md5";
							}
							break;
					}
				}
			}

			// check and return state
			IsValid = !String.IsNullOrWhiteSpace(DirectoryUri) || !String.IsNullOrWhiteSpace(FileUri);
			return IsValid;
		}

		private bool PromptYN()
		{
			var key = Console.ReadKey();
			Console.WriteLine();
			return (Char.ToUpperInvariant(key.KeyChar) == 'Y');
		}

		public bool PromptInput()
		{
			// neither specified
			if (String.IsNullOrWhiteSpace(DirectoryUri) && String.IsNullOrWhiteSpace(FileUri))
			{
				Console.WriteLine("No file or directory specified.");
				Console.Write("Would you like to auto-create a new hashfile in the current directory? (Y/n):");
				if (PromptYN())
				{
					DirectoryUri = Environment.CurrentDirectory;
					IsValid = true;
				}
			}

			return IsValid;
		}

		public bool PromptOutput()
		{
			Console.WriteLine("Verbose was not specified and there were errors.");
			Console.Write("Would you like to see a report of the errors? (Y/n):");
			return PromptYN();
		}

		public bool IsValid { get; private set; }

		public bool RunAsPreview { get; private set; }
		public bool CreateReport { get; private set; }
		public bool UseAbsolutePath { get; private set; }
		public Core.HashType HashType { get; private set; }

		public string FileUri { get; private set; }
		public string DirectoryUri { get; private set; }

		public bool FileExists { get { return !String.IsNullOrWhiteSpace(FileUri) && File.Exists(FileUri); } }
		public bool DirectoryExists { get { return !String.IsNullOrWhiteSpace(DirectoryUri) && Directory.Exists(DirectoryUri); } }

		public bool IsDefaultDirectory { get { return DirectoryUri == "."; } }
		public bool IsCurrentDirectory { get { return IsDefaultDirectory || String.Compare(CurrentDirectoryUri, DirectoryUri) == 0; } }
		public string CurrentDirectoryUri { get { return Environment.CurrentDirectory; } }
	}
}