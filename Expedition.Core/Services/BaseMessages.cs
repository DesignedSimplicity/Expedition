using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public interface IChecksumsRequest
	{
		TextWriter LogStream { get; }
		bool Preview { get; }
	}

	public class BaseRequest
	{
		public DateTime Started { get; protected set; } = DateTime.UtcNow;
	}

	public class ChecksumsExecute
	{
		protected IChecksumsRequest _request;

		public FileInfo[] Files { get; set; }

		public Dictionary<string, Exception> Exceptions { get; private set; } = new Dictionary<string, Exception>();

		public void Log(string text) { _request.LogStream?.Write(text); }

		public void LogLine(string text) { _request.LogStream?.WriteLine(text); }

		public void LogError(string uri, Exception ex) { Exceptions.Add(uri, ex); LogLine($"ERROR: {uri} @ {ex.Message}"); }
	}

	public class ChecksumsResponse: BaseResponse
	{
		public FileInfo[] Files { get; protected set; }

		public bool HasFiles { get { return Files.Length > 0; } }
	}

	public class BaseResponse
	{
		public DateTime Started { get; protected set; }
		public DateTime Completed { get; protected set; } = DateTime.UtcNow;
		public Dictionary<string, Exception> Errors { get; protected set; }
		public bool HasErrors { get { return Errors.Count > 0; } }
	}
}
