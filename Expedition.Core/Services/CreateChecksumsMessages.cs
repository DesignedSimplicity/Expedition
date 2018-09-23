using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class CreateChecksumsRequest : QueryFileSystemRequest, IChecksumsRequest
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

		/// <summary>
		/// Creates output excel file report
		/// </summary>
		public bool Report { get; set; } = false;
	}

	public class CreateChecksumsExecute : ChecksumsExecute
	{
		public CreateChecksumsRequest Request { get; private set; }

		public string OutputFileUri { get; set; }

		public string ReportFileUri { get; set; }

		private int _truncateLength = 0;

		public CreateChecksumsExecute(CreateChecksumsRequest request)
		{
			// prepare response
			_request = Request = request;

			// set up truncation
			if (!String.IsNullOrWhiteSpace(Request.RelativeToUri))
				_truncateLength = ParsePath.FixUri(Request.RelativeToUri, true).Length;
		}

		public string GetOuputPath(string uri) { return _truncateLength > 0 ? uri.Substring(_truncateLength) : uri; }
	}

	public class CreateChecksumsResponse : ChecksumsResponse
	{
		public string OutputFileUri { get; set; }

		public CreateChecksumsResponse(CreateChecksumsExecute execute) : base(execute.Request)
		{
			Errors = execute.Exceptions;
			Files = execute.Files;

			OutputFileUri = execute.OutputFileUri;
		}
	}
}
