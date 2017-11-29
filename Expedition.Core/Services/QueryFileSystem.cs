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
			// check input uri
			var uri = request.DirectoryUri;
			if (String.IsNullOrWhiteSpace(uri)) throw new Exception($"Input URI is not provided");
			if (!Directory.Exists(uri)) throw new Exception($"Input URI '{uri}' does not exist or is not accessible");

			// set up searching
			var searchPattern = request.FilePattern ?? "*.*";
			var searchOption = request.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			// execute file search
			var response = new QueryFileSystemResponse();
			var dir = new DirectoryInfo(request.DirectoryUri);
			if (request.Verbose)
			{
				// enumerate files and optionally supress errors
				var files = new List<FileInfo>();
				foreach (var file in dir.EnumerateFiles(searchPattern, searchOption))
				{
					try
					{
						files.Add(file);
					}
					catch (Exception ex)
					{
						// log exception and re-throw if not silent
						response.Exceptions.Add(file.FullName, ex);
						if (!request.Silent) throw ex;
					}
				}
				response.Files = files.ToArray();
			}
			else
			{
				// just get all of the files at once
				try
				{
					response.Files = dir.GetFiles(searchPattern, searchOption);
				}
				catch (Exception ex)
				{
					// log exception and re-throw if not silent
					response.Exceptions.Add(dir.FullName, ex);
					if (!request.Silent) throw ex;
				}
			}

			// return results
			return response;
		}
	}
}
