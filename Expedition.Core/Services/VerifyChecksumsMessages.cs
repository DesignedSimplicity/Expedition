using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class VerifyChecksumsRequest : BaseRequest, IChecksumsRequest
	{
		/// <summary>
		/// Used to make output path relative
		/// </summary>
		public string FileUri { get; set; }

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

	public class VerifyChecksumsExecute : ChecksumsExecute
	{
		public new VerifyChecksumsRequest Request => (VerifyChecksumsRequest)base.Request;

		public string[] FileContents { get; set; }

		public string InputFileUri { get; set; }

		public string ReportFileUri { get; set; }

		public HashType HashType { get; set; } = HashType.Md5;

		public VerifyChecksumsExecute(VerifyChecksumsRequest request) : base(request)
		{
			InputFileUri = request.FileUri;
		}
	}

	public class VerifyChecksumsResponse : ChecksumsResponse
	{
		public string InputFileUri { get; set; }

		public VerifyChecksumsResponse(VerifyChecksumsExecute execute) : base(execute.Request)
		{
			Files = execute.Files;

			Errors = execute.Exceptions;

			InputFileUri = execute.InputFileUri;
		}
	}
}