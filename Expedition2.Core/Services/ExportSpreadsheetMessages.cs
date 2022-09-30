using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core2.Services
{
	public class ExportSpreadsheetRequest : QueryFileSystemRequest
	{
	}

	public class ExportSpreadsheetExecute
	{
	}

	public class ExportSpreadsheetResponse : QueryFileSystemResponse
	{
		public string OutputFileUri { get; set; }

		public ExportSpreadsheetResponse(CreateChecksumsExecute execute) : base(execute.Request)
		{
			Files = execute.Files;

			OutputFileUri = execute.OutputFileUri;
		}
	}
}
