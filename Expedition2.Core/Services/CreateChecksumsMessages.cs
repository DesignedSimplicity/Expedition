﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expedition2.Core;

namespace Expedition2.Core.Services
{
	public class CreateChecksumsRequest : QueryFileSystemRequest, IChecksumsRequest
	{
		/// <summary>
		/// Name to prefix output files
		/// </summary>
		public string? NamePrefix { get; set; }

		/// <summary>
		/// Decides which hash algorithm to use
		/// </summary>
		public HashType HashType { get; set; } = HashType.Md5;

		/// <summary>
		/// Used to make output path relative
		/// </summary>
		public string? RelativeToUri { get; set; }

		/// <summary>
		/// Text writer for status logging output
		/// </summary>
		public TextWriter? LogStream { get; set; }

		/// <summary>
		/// Console writer for status display output
		/// </summary>
		public TextWriter? ConsoleOut { get; set; }

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
		public new CreateChecksumsRequest Request => (CreateChecksumsRequest)base.Request;

		public string? OutputFileUri { get; set; }

		public string? ReportFileUri { get; set; }

		private int _truncateUriLength = 0;

		public CreateChecksumsExecute(CreateChecksumsRequest request) : base(request)
		{
			// set up truncation
			if (!String.IsNullOrWhiteSpace(Request.RelativeToUri))
			{
				Request.RelativeToUri = ParsePath.FixUri(Request.RelativeToUri, true);
				_truncateUriLength = Request.RelativeToUri.Length;
			}
		}

		public string GetOuputPath(string uri) { return _truncateUriLength > 0 ? uri.Substring(_truncateUriLength) : uri; }
	}

	public class CreateChecksumsResponse : ChecksumsResponse
	{
		public string? OutputFileUri { get; set; }

		public CreateChecksumsResponse(CreateChecksumsExecute execute) : base(execute.Request)
		{
			Errors = execute.Exceptions;
			Files = execute.Files;

			OutputFileUri = execute.OutputFileUri;
		}
	}
}
