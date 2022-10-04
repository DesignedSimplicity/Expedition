using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core.Services
{
    public enum PowerSize { KB = 1024, MB = 1048576, GB = 1073741824 }

	public delegate int BaseFileSystemStateChange(BaseFileSytemState state);

	public enum BaseFileSytemStatus
	{
		Exception = -9999,

		NotFound = -1,
		Unknown = 0,

		Default = 1,

		HashCreated = 2,
		HashMissing = -2,

		HashVerified = 3,
		HashInvalid = -3,
	}

	public class BaseFileSytemState
	{
		public BaseFileSytemStatus Status { get; set; } = BaseFileSytemStatus.Default;
		public DateTime Timestamp { get; set; } = DateTime.Now;
		public Guid Id { get; set; } = Guid.NewGuid();

		public string Uri { get; set; } = string.Empty;
		public long Size { get; set; } = 0;

		public string Hash { get; set; } = string.Empty;

		public string Message { get; set; } = string.Empty;

		public Exception? Exception { get; set; } = null;


		public BaseFileSytemState(FileInfo file, BaseFileSytemStatus status = BaseFileSytemStatus.Default, string message = "")
		{
			Uri = file.FullName;
			Size = file.Length;
			Status = status;
			Message = message;
		}

		public BaseFileSytemState(FileInfo file, Exception ex, string message = "")
		{
			Uri = file.FullName;
			Size = file.Length;
			Status = BaseFileSytemStatus.Exception;
			Exception = ex;
			Message = message;
		}

		public BaseFileSytemState(FileInfo file, string hash)
		{
			Uri = file.FullName;
			Size = file.Length;
			Hash = hash;
			Status = BaseFileSytemStatus.HashCreated;
		}
	}

	public interface IBaseRequest
	{
		DateTime Started { get; }

		BaseFileSystemStateChange OnChange { get; set; }
	}

	public class BaseRequest : IBaseRequest
	{
		public DateTime Started { get; private set; } = DateTime.Now;

		public BaseFileSystemStateChange OnChange { get; set; }
	}

	public interface IBaseExecute
	{
		IBaseRequest Request { get; }

		List<BaseFileSytemState> State { get; }

		void SetState(BaseFileSytemState state);
	}

	public class BaseExecute : IBaseExecute
	{
		public IBaseRequest Request { get; private set; }
		public List<BaseFileSytemState> State { get; private set; } = new List<BaseFileSytemState>();

		public BaseExecute(IBaseRequest request)
		{
			Request = request;
		}

		public void SetState(BaseFileSytemState state)
		{
			State.Add(state);
			Request?.OnChange?.Invoke(state);
		}
	}

	public class BaseResponse
	{
		public DateTime Started { get; private set; }
		public DateTime Completed { get; private set; }

		public List<BaseFileSytemState> State { get; protected set; } = new List<BaseFileSytemState>();

		public Dictionary<string, Exception> Errors { get; protected set; } = new Dictionary<string, Exception>();
		public bool HasErrors { get { return Errors.Count > 0; } }

		public BaseResponse(BaseRequest request)
		{
			Started = request.Started;
			SetCompleted(Completed);
		}

		public void SetCompleted(List<BaseFileSytemState> state)
		{
			Completed = DateTime.Now;
			State = state;
		}

		public void SetCompleted(DateTime completed)
		{
			Completed = DateTime.Now;
		}
	}



	public interface IChecksumsRequest
	{
		TextWriter LogStream { get; }
		bool Preview { get; }
	}

	public class ChecksumsExecute : BaseExecute
	{
		protected IChecksumsRequest _checksumsRequest;

		public FileInfo[] Files { get; set; }

		public PatrolFactory Factory { get; set; }
		public SourcePatrolInfo PatrolSource { get; set; }
		public List<FolderPatrolInfo> PatrolFolders { get; set; }
		public List<FilePatrolInfo> PatrolFiles { get; set; }

		public Dictionary<string, Exception> Exceptions { get; private set; } = new Dictionary<string, Exception>();


		public ChecksumsExecute(IChecksumsRequest request) : base((IBaseRequest)request)
		{ 
			_checksumsRequest = request; 
			Factory = new PatrolFactory();
		}


		public void Log(string text) { _checksumsRequest.LogStream?.Write(text); }

		public void LogLine(string text) { _checksumsRequest.LogStream?.WriteLine(text); }
		public void LogError(string uri, Exception ex) { Exceptions.Add(uri, ex); LogLine($"ERROR: {uri} @ {ex.Message}"); }
	}

	public class ChecksumsResponse : BaseResponse
	{
		public FileInfo[] Files { get; protected set; }

		public bool HasFiles { get { return Files.Length > 0; } }

		public ChecksumsResponse(BaseRequest request) : base(request) {}
	}
}
