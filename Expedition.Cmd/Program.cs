using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Expedition.Core.Services;
using System.Reflection;
using Expedition.Core.Parse;

namespace Expedition.Cmd
{
	class Program
	{
		static void Main(string[] args)
		{
			//"D:\Development\Sources\Expedition\Expedition.Cmd\bin\Debug\Scout.exe"
			//args = new string[] { @"R:\_20180924-012312.md5", "-r", "" };

			/* TODO:
			 * Combine Error/Exception in Execute/Response
			 * Add -verify assumes verify on first md5 file found in current directory
			 * Add simple performance/benchmark summary
			 * Add sheet for directory sizes/counts
			 * Add sheet for exceptions/errors
			 */

			// parse out command line options
			var arguments = new Arguments();
			if (!arguments.Parse(args))
				arguments.PromptInput();

			// execute valid response
			if (arguments.IsValid)
			{
				Console.WriteLine($"STARTING SCOUT...");

				// decide if we are creating or verifying
				var response = arguments.FileExists
					? Verify(arguments)
					: Create(arguments);

				// parse service response here
				if (response.HasErrors)
				{
					// show error details here
					var errorCount = 1;
					var errorLog = new StringBuilder();
					Console.WriteLine($"ERRORS: {response.Errors.Count}");					
					foreach (var error in response.Errors)
					{
						var errorText = $"ERROR #{errorCount.ToString("0000")}: {error.Key} = {error.Value.Message}";
						errorLog.AppendLine(errorText);
						Console.WriteLine(errorText);
						errorCount++;
					}

					// prompt to create log file
					if (!arguments.CreateReport && arguments.PromptOutput())
					{
						var now = DateTime.Now;
						var logFile = $"Errors_{now:yyyyMMdd}-{now:HHmmss}.txt";
						File.WriteAllText(logFile, errorLog.ToString());
					}
				}

#if DEBUG
				//Console.ReadKey();
#endif
			}
		}

		static ChecksumsResponse Verify(Arguments arguments)
		{
			Console.Write($"VERIFY: {arguments.FileUri}");
			if (arguments.UseAbsolutePath) Console.Write(" -absolute");
			if (arguments.RunAsPreview) Console.Write(" -preview");
			if (arguments.CreateReport) Console.Write(" -report");
			Console.WriteLine();

			var request = new VerifyChecksumsRequest()
			{
				FileUri = arguments.FileUri,
				LogStream = Console.Out,
				Report = arguments.CreateReport,
			};

			var verify = new VerifyChecksums();
			return verify.Execute(request);
		}

		static ChecksumsResponse Create(Arguments arguments)
		{
			Console.Write($"CREATE: {arguments.DirectoryUri}");
			if (arguments.UseAbsolutePath) Console.Write(" -absolute");
			if (arguments.RunAsPreview) Console.Write(" -preview");
			if (arguments.CreateReport) Console.Write(" -report");
			if (arguments.HashType == Core.HashType.Sha1) Console.Write(" -sha1");
			Console.WriteLine();

			// get starting directory
			var currentUri = arguments.CurrentDirectoryUri;
			var directoryUri = arguments.IsCurrentDirectory
				? currentUri
				: arguments.DirectoryUri;

			// decide to make it relative
			var relativeToUri = arguments.UseAbsolutePath
				? string.Empty
				: directoryUri;

			// prepare service request
			var request = new CreateChecksumsRequest()
			{
				HashType = arguments.HashType,
				DirectoryUri = directoryUri,
				RelativeToUri = relativeToUri,
				Report = arguments.CreateReport,
				Preview = arguments.RunAsPreview,
				LogStream = Console.Out,
				Recursive = true,
			};

			// create and execute service request
			var create = new CreateChecksums();
			var response = create.Execute(request);

			return response;
		}
	}
}