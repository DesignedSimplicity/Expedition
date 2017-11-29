using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class QueryFileSystem
	{
		public QueryFileSystemResponse Execute(QueryFileSystemRequest request)
		{
			var response = new QueryFileSystemResponse();

			if (request.Silent) throw new NotImplementedException("Silent");
			if (request.Verbose) throw new NotImplementedException("Verbose");

			// check input uri
			if (String.IsNullOrWhiteSpace(request.DirectoryUri)) throw new Exception($"Input URI is not provided");

			// validate directory
			var dir = new DirectoryInfo(request.DirectoryUri);
			if (!dir.Exists) throw new Exception($"Input URI '{request.DirectoryUri}' does not exist or is not accessible");

			// set up searching
			var searchPattern = request.FilePattern ?? "*.*";
			var searchOption = request.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			// execute file search
			response.Files = dir.GetFiles(searchPattern, searchOption);

			// return results
			return response;
		}
	}
}
