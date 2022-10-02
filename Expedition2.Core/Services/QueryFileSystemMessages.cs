using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core.Services
{
	public class QueryFileSystemRequest : BaseRequest
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
		public bool Recursive { get; set; } = false;

		/// <summary>
		/// File matching pattern
		/// </summary>
		public string FilePattern { get; set; }

		/// <summary>
		/// Initial directory
		/// </summary>
		public string DirectoryUri { get; set; }
	}

	public class QueryFileSystemResponse : BaseResponse
	{
		public FileInfo[] Files { get; set; }

		public QueryFileSystemResponse(QueryFileSystemRequest request) : base(request) { }
	}
}
