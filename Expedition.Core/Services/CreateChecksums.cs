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
		/// Text writer for checksum output
		/// </summary>
		public TextWriter HashOutput { get; set; }

		/// <summary>
		/// Text writer for status logging output
		/// </summary>
		public TextWriter LogOutput { get; set; }

		/// <summary>
		/// Enable verbose status logging
		/// </summary>
		public bool Verbose { get; set; }
	}

	public class CreateChecksumsResponse
	{
		public DateTime Started { get; set; }
		public DateTime Completed { get; set; }

		public TextWriter HashOutput { get; set; }
		public TextWriter LogOutput { get; set; }

		public string[] Files { get; set; }
		public Dictionary<string, Exception> Errors { get; set; } = new Dictionary<string, Exception>();

		public bool HasFiles { get { return Files.Length > 0; } }
		public bool HasErrors { get { return Errors.Count > 0; } }

		public CreateChecksumsResponse(CreateChecksumsRequest request)
		{
			Started = DateTime.UtcNow;
			LogOutput = request.LogOutput;
			HashOutput = request.HashOutput;
		}
	}

	public class CreateChecksums
	{
		private CreateChecksumsRequest _request;
		private CreateChecksumsResponse _response;

		public CreateChecksumsResponse Create(CreateChecksumsRequest request)
		{
			_request = request;
			_response = new CreateChecksumsResponse(request);

			Execute();

			_response.Completed = DateTime.UtcNow;
			return _response;
		}

		private void Execute()
		{
			// set up paths
			var truncate = 0;
			var root = _request.RelativeToUri;
			if (!String.IsNullOrWhiteSpace(root))
			{
				// TODO ensure DirectoryUri is child of RelativeToUri
				if (!root.EndsWith(Path.PathSeparator.ToString())) root += Path.PathSeparator;
				truncate = root.Length;
			}

			// set up logging
			var log = _request.LogOutput;
			var verbose = _request.Verbose;

			// set up hashing
			var output = _request.HashOutput;
			var hasher = _request.HashType.ToString();
			using (var algorithm = HashCalc.GetHashAlgorithm(_request.HashType))
			{
				output?.WriteLineAsync($"# Generated with Expedition at {DateTime.UtcNow}");
				output?.WriteLineAsync($"# https://github.com/DesignedSimplicity/Expedition/");
				output?.WriteLineAsync("");

				// set up searching
				var searchPattern = _request.FilePattern ?? "*.*";
				var searchOption = _request.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

				// enumerate and hash files
				int count = 0;
				_response.Files = Directory.GetFiles(_request.DirectoryUri, searchPattern, searchOption);
				foreach (var file in _response.Files)
				{
					count++;
					log?.WriteAsync($"{count}. {file}");

					// calculate hash
					var hash = HashCalc.GetHash(file, algorithm);

					log?.WriteLineAsync($" -> {hasher} = {hash}");

					// write checksum
					var path = (truncate == 0
						? file
						: "*" + file.Substring(truncate));
					output?.WriteLineAsync($"{hash} {path}");
				}
			}
		}
	}
}
