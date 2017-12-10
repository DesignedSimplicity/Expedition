using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Expedition.Core.Services;
using System.Reflection;

namespace Expedition.Cmd
{
	class Program
	{
		static void Main(string[] args)
		{
			args = new string[] { @"C:\Apps\" };

			// parse out command line options
			var start = new Arguments();
			if (!start.Parse(args))
				start.PromptInput();

			// execute valid response
			if (start.IsValid)
			{
				// get starting directory
				var currentUri = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				var directoryUri = start.IsCurrentDirectory
					? currentUri
					: start.DirectoryUri;

				// decide to make it relative
				var relativeToUri = start.IsAbsolutePath
					? string.Empty
					: directoryUri;

				// prepare service request
				var request = new CreateChecksumsRequest()
				{
					HashType = start.HashType,
					DirectoryUri = directoryUri,
					RelativeToUri = relativeToUri,
					Preview = start.IsPreview,
					LogStream = Console.Out,
					Recursive = true,
				};

				// create and execute service request
				var create = new CreateChecksums();
				var response = create.Execute(request);

				// parse service response here
				if (response.HasErrors)
				{
					// prompt to show errors here
					if (start.IsVerboseReport || start.PromptOutput())
					{
						// show detailed errors here
					}
				}
			}

			Console.ReadKey();
		}
	}
}