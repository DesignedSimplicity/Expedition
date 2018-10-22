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

		public bool PromptInput()
		{
			// nothing specified
			if (String.IsNullOrWhiteSpace(DirectoryUri) && String.IsNullOrWhiteSpace(FileUri))
			{
				Console.WriteLine("WARNING: No file or directory specified!");
				if (PromptAutoCreate())
				{
					return IsValid;
				}

				if (PromptAutoVerify())
				{
					return IsValid;
				}

				Console.WriteLine("Please enter a path to a directory to hash or a file to verify.");
				while (!IsValid)
				{
					Console.Write("Path: ");
					var path = Console.ReadLine();
					path = path.Trim(new char[] { '"' });
					if (File.Exists(path))
					{
						FileUri = path;
						IsValid = true;
					}
					else if (Directory.Exists(path))
					{
						DirectoryUri = path;
						IsValid = true;
					}
					else
					{
						Console.WriteLine("WARNING: Path is invalid!");
					}
				}
			}

			return IsValid;
		}

		private bool PromptYN()
		{
			var key = Console.ReadKey();
			Console.WriteLine();
			return (Char.ToUpperInvariant(key.KeyChar) == 'Y');
		}

		private bool PromptAutoCreate()
		{
			// auto-create a new MD5 file in current directory
			Console.Write($"Would you like to auto-create a new hash file in the current directory? (y/n):");
			if (PromptYN())
			{
				DirectoryUri = Environment.CurrentDirectory;
				IsValid = true;
			}

			return IsValid;
		}

		private bool PromptAutoVerify()
		{
			var dir = new DirectoryInfo(Environment.CurrentDirectory);
			var verify = dir.GetFiles().Where(x => String.Compare(x.Extension, ".md5", true) == 0)
									.FirstOrDefault();
			if (verify != null)
			{
				Console.Write($"Would you like to verify this hash file: {verify.Name}? (y/n):");
				if (PromptYN())
				{
					FileUri = verify.FullName;
					IsValid = true;
				}
			}

			return IsValid;
		}

		public bool PromptOutput()
		{
			Console.WriteLine("WARNING: Verbose was not specified and there were errors.");
			Console.Write("Would you like to create a log file with the errors? (y/n):");
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