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
		//public TextWriter HashOutput { get; set; }

		/// <summary>
		/// Text writer for status logging output
		/// </summary>
		public TextWriter LogOutput { get; set; }

		/// <summary>
		/// Enable verbose status logging
		/// </summary>
		public bool Verbose { get; set; } = false;

		/// <summary>
		/// Checks access without creating hashes
		/// </summary>
		public bool Preview { get; set; } = false;
	}

	public class CreateChecksumsResponse
	{
		public DateTime Started { get; set; }
		public DateTime Completed { get; set; }

		public TextWriter LogOutput { get; set; }
		public string OutputFileUri { get; set; }

		public string[] Files { get; set; }
		public Dictionary<string, Exception> Errors { get; set; } = new Dictionary<string, Exception>();

		public bool HasFiles { get { return Files.Length > 0; } }
		public bool HasErrors { get { return Errors.Count > 0; } }

		public CreateChecksumsResponse(CreateChecksumsRequest request)
		{
			Started = DateTime.UtcNow;
			LogOutput = request.LogOutput;
			//HashOutput = request.HashOutput;
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

			Validate();

			Generate();

			Complete();

			return _response;
		}

		private void Complete()
		{
			_response.Completed = DateTime.UtcNow;
		}

		private void Validate()
		{
			// check input uri
			var dir = _request.DirectoryUri;
			if (String.IsNullOrWhiteSpace(dir)) throw new Exception($"Input URI is not provided");
			if (!Directory.Exists(dir)) throw new Exception($"Input URI '{dir}' is not accessible");

			// check output uri
			var root = _request.RelativeToUri;
			if (!String.IsNullOrWhiteSpace(root))
			{
				if (!Directory.Exists(root)) throw new Exception($"Output URI '{root}' is not accessible");
				if (!ParsePath.IsSamePath(root, dir) && !ParsePath.IsAncestor(root, dir)) throw new Exception($"Input URI '{root}' is not ancestor of output URI '{dir}'");
			}

			// check output file
			var now = DateTime.Now;
			var ext = _request.HashType.ToString().ToLowerInvariant();
			var path = String.IsNullOrWhiteSpace(root) 
				? dir 
				: root;
			var file = Path.Combine(path, $"_{now:yyyyMMdd}-{now:HHmmss}.{ext}");
			if (File.Exists(file)) throw new Exception($"Output file '{file}' already exists");
			_response.OutputFileUri = file;
		}

		private void Generate()
		{
			// set up paths
			var truncate = 0;
			var root = _request.RelativeToUri;
			if (!String.IsNullOrWhiteSpace(root))
				truncate = ParsePath.FixUri(root, true).Length;

			// set up logging
			var log = _request.LogOutput;
			var verbose = _request.Verbose;

			// set up hashing
			StreamWriter output = null;
			if (!_request.Preview) output = File.CreateText(_response.OutputFileUri);

			var hasher = _request.HashType.ToString();
			using (var algorithm = HashCalc.GetHashAlgorithm(_request.HashType))
			{
				output?.WriteLine($"# Generated with Expedition at {DateTime.UtcNow}");
				output?.WriteLine($"# https://github.com/DesignedSimplicity/Expedition/");
				output?.WriteLine("");

				// set up searching
				var searchPattern = _request.FilePattern ?? "*.*";
				var searchOption = _request.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

				// enumerate and hash files
				int count = 0;
				_response.Files = Directory.GetFiles(_request.DirectoryUri, searchPattern, searchOption);
				foreach (var file in _response.Files)
				{
					count++;
					log?.Write($"{count}. {file}");

					// calculate hash and output hash to log
					var hash = (_request.Preview
						? hasher
						: HashCalc.GetHash(file, algorithm));
					log?.WriteLine($" -> {hasher} = {hash}");

					// format and write checksum to stream
					var path = (truncate == 0
						? file
						: "*" + file.Substring(truncate));
					output?.WriteLine($"{hash} {path}");
				}

				// clean up output file
				output?.Flush();
				output?.Close();
				output?.Dispose();
			}
		}
	}
}
