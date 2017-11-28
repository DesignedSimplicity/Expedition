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
			var create = new CreateChecksums();
			create.Create(new CreateChecksumsRequest()
			{
				DirectoryUri = @"C:\Apps\",
				RelativeToUri = @"C:\Apps\",
				//HashType = Core.HashType.Sha1,
				LogOutput = Console.Out
				//HashOutput = Console.Out,
				//Preview = true
			});
		}
	}
}
