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

			Console.WriteLine(Environment.CurrentDirectory);
			//args = new string[] { @"E:\_NEW\_KENNY\_20171210-202950.md5" }; // @"C:\Apps\", "-md5", "-a" };
			//args = new string[] { @"I:\Media\_20180608-070859.md5" };
			args = new string[] { @"R:\_20180924-012312.md5", "-r", "" };

			/* TODO:
			 * Combine Error/Exception in Execute/Response
			 * Add -verify assumes verify on first md5 file found in current directory
			 * Rename -verbose -> -report to enable creation of xlsx output files
			 * Update verify chechsum to support -report mode
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
				// decide if we are creating or verifying
				var response = arguments.FileExists
					? Verify(arguments)
					: Create(arguments);

				// parse service response here
				if (response.HasErrors)
				{
					// show error details here
					Console.WriteLine($"TOTAL ERRORS: {response.Errors.Count}");
					foreach (var error in response.Errors)
					{
						Console.WriteLine($"{error.Key} = {error.Value.Message}");
					}
				}

#if DEBUG
				//Console.ReadKey();
#endif
			}
		}

		static ChecksumsResponse Verify(Arguments arguments)
		{
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