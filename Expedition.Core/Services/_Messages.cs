using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class CreateChecksumsRequest : QueryFileSystemRequest
	{
		/// <summary>
		/// Decides which hash algorithm to use
		/// </summary>
		public HashType HashType { get; set; } = HashType.Md5;

		/// <summary>
		/// Used to make output path relative
		/// </summary>
		public string RelativeToUri { get; set; }

		/// <summary>
		/// Text writer for status logging output
		/// </summary>
		public TextWriter LogStream { get; set; }

		/// <summary>
		/// Checks access without creating hashes
		/// </summary>
		public bool Preview { get; set; } = false;
	}

	public class CreateChecksumsExecute : IDisposable
	{
		public FileInfo[] Files { get; set; }

		public Dictionary<string, Exception> Exceptions { get; private set; } = new Dictionary<string, Exception>();

		public CreateChecksumsRequest Request { get; private set; }

		public string OutputFileUri { get; set; }

		private int _truncateLength = 0;

		public CreateChecksumsExecute(CreateChecksumsRequest request)
		{
			// prepare response
			Request = request;

			// set up truncation
			if (!String.IsNullOrWhiteSpace(Request.RelativeToUri))
				_truncateLength = ParsePath.FixUri(Request.RelativeToUri, true).Length;
		}

		public void Log(string text) { Request.LogStream?.Write(text); }

		public void LogLine(string text) { Request.LogStream?.WriteLine(text); }

		public void LogError(string uri, Exception ex) { Exceptions.Add(uri, ex); LogLine($"ERROR: {uri} @ {ex.Message}"); }


		public string GetRelativePath(string uri) { return uri.Substring(_truncateLength); }

		public void Dispose()
		{
			//throw new NotImplementedException();
		}
	}

	public class CreateChecksumsResponse
	{
		public DateTime Started { get; set; }
		public DateTime Completed { get; set; }

		public string OutputFileUri { get; set; }

		public FileInfo[] Files { get; private set; }
		public Dictionary<string, Exception> Errors { get; private set; }

		public bool HasFiles { get { return Files.Length > 0; } }
		public bool HasErrors { get { return Errors.Count > 0; } }

		public CreateChecksumsResponse(CreateChecksumsExecute execute)
		{
			Started = DateTime.UtcNow;

			Errors = execute.Exceptions;
			Files = execute.Files;

			OutputFileUri = execute.OutputFileUri;
		}
	}



	public class QueryFileSystemRequest
	{
		/// <summary>
		/// Ignore file system access errors
		/// </summary>
		public bool Silent { get; set; } = false;

		/// <summary>
		/// Manual recursion with status report
		/// </summary>
		public bool Verbose { get; set; } = false;

		/// <summary>
		/// Recurse directory
		/// </summary>
		public bool Recursive { get; set; }

		/// <summary>
		/// File matching pattern
		/// </summary>
		public string FilePattern { get; set; }

		/// <summary>
		/// Initial directory
		/// </summary>
		public string DirectoryUri { get; set; }
	}

	public class QueryFileSystemResponse
	{
		public FileInfo[] Files { get; set; }

		public Dictionary<string, Exception> Exceptions { get; private set; } = new Dictionary<string, Exception>();
	}
}
