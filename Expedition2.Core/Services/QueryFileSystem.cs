using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core.Services
{
	public class QueryFileSystem
	{
		private void UpdateState(QueryFileSystemRequest request, QueryFileSystemResponse response, BaseFileSytemState state)
		{
			//response.State.Add(state);
			//request.OnQueryFileSystemStateChange?.Invoke(state);
		}

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
			var response = new QueryFileSystemResponse(request);
			var dir = new DirectoryInfo(request.DirectoryUri);

			// start state
			var execute = new BaseExecute(request);

			// TODO HACK
			if (true || request.Verbose)
			{
				// enumerate files and optionally supress errors
				var files = new List<FileInfo>();
				foreach (var file in dir.EnumerateFiles(searchPattern))
				{
					try
					{
						files.Add(file);
						execute.SetState(new BaseFileSytemState(file));

					}
					catch (Exception ex)
					{
						// log exception and re-throw if not silent
						response.Errors.Add(file.FullName, ex);
						execute.SetState(new BaseFileSytemState(file, ex));
						if (!request.ErrorSafe) throw ex;
					}
				}
				if (request.Recursive)
				{
					foreach (var sub in dir.EnumerateDirectories())
					{
						//execute.SetState(new ChecksumSystemState(sub));
						try
						{
							foreach (var file in sub.EnumerateFiles(searchPattern, searchOption))
							{
								try
								{
									files.Add(file);
									execute.SetState(new BaseFileSytemState(file));
								}
								catch (Exception ex)
								{
									// log exception and re-throw if not silent
									response.Errors.Add(file.FullName, ex);
									execute.SetState(new BaseFileSytemState(file, ex));
									if (!request.ErrorSafe) throw ex;
								}
							}
						}
						catch (Exception ex)
						{
							// log exception and re-throw if not silent
							response.Errors.Add(sub.FullName, ex);
							//execute.SetState(new ChecksumSystemState(sub, ChecksumSystemStatus.Unknown, ex));
							if (!request.ErrorSafe) throw ex;
						}
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
					response.Errors.Add($"Directory = {dir?.FullName}", ex);
					if (!request.ErrorSafe) throw ex;
				}
			}

			// return results
			response.SetCompleted(execute.State);
			return response;
		}
	}
}
