using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class CreateChecksumsRequest
	{
		/// <summary>
		/// Decides which hash algorithm to use
		/// </summary>
		public HashType HashType { get; set; } = HashType.Md5;

		/// <summary>
		/// Recurse into subdirectories
		/// </summary>
		public bool Recursive { get; set; } = true;

		/// <summary>
		/// File filter pattern (ex: *.jpg)
		/// </summary>
		public string FilePattern { get; set; }

		/// <summary>
		/// Directory with files to hash
		/// </summary>
		public string DirectoryUri { get; set; }

		/// <summary>
		/// Used to make output path relative
		/// </summary>
		public string RelativeToUri { get; set; }

		/// <summary>
		/// Text writer for status logging output
		/// </summary>
		public TextWriter LogStream { get; set; }

		/// <summary>
		/// Enable verbose status logging
		/// </summary>
		public bool Verbose { get; set; } = false;

		/// <summary>
		/// Checks access without creating hashes
		/// </summary>
		public bool Preview { get; set; } = false;
	}

	public class CreateChecksumsExecute : IDisposable
	{
		public CreateChecksumsRequest Request { get; private set; }

		public string[] Files { get; set; }

		public Dictionary<string, Exception> Errors { get; private set; }

		public string OutputFileUri { get; set; }

		private int _truncateLength = 0;

		public CreateChecksumsExecute(CreateChecksumsRequest request)
		{
			// prepare response
			Request = request;
			Errors = new Dictionary<string, Exception>();

			// set up relative path truncation
			if (!String.IsNullOrWhiteSpace(Request.RelativeToUri))
				_truncateLength = ParsePath.FixUri(Request.RelativeToUri, true).Length;
		}

		public void Log(string text)
		{
			Request.LogStream?.Write(text);
		}

		public void LogLine(string text)
		{
			Request.LogStream?.WriteLine(text);
		}

		public void LogError(string uri, Exception ex)
		{
			Errors.Add(uri, ex);
			Request.LogStream?.WriteLine($"ERROR: {uri} @ {ex.Message}");
		}

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

		public string[] Files { get; private set; }
		public Dictionary<string, Exception> Errors { get; private set; }

		public bool HasFiles { get { return Files.Length > 0; } }
		public bool HasErrors { get { return Errors.Count > 0; } }

		public CreateChecksumsResponse(CreateChecksumsExecute execute)
		{
			Started = DateTime.UtcNow;

			Errors = execute.Errors;
			Files = execute.Files;

			OutputFileUri = execute.OutputFileUri;
		}
	}
}
