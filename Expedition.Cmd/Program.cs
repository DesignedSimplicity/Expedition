using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Expedition.Core.Services;

namespace Expedition.Cmd
{
	class Program
	{
		static void Main(string[] args)
		{
			var request = new CreateChecksumsRequest()
			{
				DirectoryUri = @"C:\Apps\",
				RelativeToUri = @"C:\Apps\",
				Recursive = true,
				//HashType = Core.HashType.Sha1,
				LogStream = Console.Out
				//HashOutput = Console.Out,
				//Preview = true
			};

			var create = new CreateChecksums();
			var response = create.Execute(request);
		}
	}
}
