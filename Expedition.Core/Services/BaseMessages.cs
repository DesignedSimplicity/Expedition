using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
    public enum PowerSize { KB = 1024, MB = 1048576, GB = 1073741824 }

	public class BaseRequest
	{
		public DateTime Started { get; private set; } = DateTime.Now;
	}

	public class BaseExecute
	{
		
	}

	public class BaseResponse
	{
		public DateTime Started { get; private set; }
		public DateTime Completed { get; private set; } = DateTime.Now;
		public Dictionary<string, Exception> Errors { get; protected set; } = new Dictionary<string, Exception>();
		public bool HasErrors { get { return Errors.Count > 0; } }

		public BaseResponse(BaseRequest request)
		{
			Started = request.Started;
		}
	}

	public interface IChecksumsRequest
	{
		TextWriter LogStream { get; }
		bool Preview { get; }
	}

	public class ChecksumsExecute : BaseExecute
	{
		protected IChecksumsRequest _request;

		public FileInfo[] Files { get; set; }

		public Dictionary<string, Exception> Exceptions { get; private set; } = new Dictionary<string, Exception>();

		public void Log(string text) { _request.LogStream?.Write(text); }

		public void LogLine(string text) { _request.LogStream?.WriteLine(text); }
		public void LogError(string uri, Exception ex) { Exceptions.Add(uri, ex); LogLine($"ERROR: {uri} @ {ex.Message}"); }
	}

	public class ChecksumsResponse : BaseResponse
	{
		public FileInfo[] Files { get; protected set; }

		public bool HasFiles { get { return Files.Length > 0; } }

		public ChecksumsResponse(BaseRequest request) : base(request) {}
	}
}
