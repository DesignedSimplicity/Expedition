using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class CreateChecksums
	{
		private CreateChecksumsExecute _execute;
		private CreateChecksumsRequest _request;

		public CreateChecksumsResponse Execute(CreateChecksumsRequest request)
		{
			_execute = new CreateChecksumsExecute(request);

			_request = request;

			Validate();

			Generate();

			Complete();

			return new CreateChecksumsResponse(_execute);
		}

		private void Validate()
		{
			// check input uri
			var dir = _execute.Request.DirectoryUri;
			if (String.IsNullOrWhiteSpace(dir)) throw new Exception($"Input URI is not provided");
			if (!Directory.Exists(dir)) throw new Exception($"Input URI '{dir}' is not accessible");

			// check output uri
			var root = _execute.Request.RelativeToUri;
			if (!String.IsNullOrWhiteSpace(root))
			{
				if (!Directory.Exists(root)) throw new Exception($"Output URI '{root}' is not accessible");
				if (!ParsePath.IsSamePath(root, dir) && !ParsePath.IsAncestor(root, dir)) throw new Exception($"Input URI '{root}' is not ancestor of output URI '{dir}'");
			}

			// check output file
			var now = DateTime.Now;
			var ext = _execute.Request.HashType.ToString().ToLowerInvariant();
			var path = String.IsNullOrWhiteSpace(root)
				? dir
				: root;
			var file = Path.Combine(path, $"_{now:yyyyMMdd}-{now:HHmmss}.{ext}");
			if (File.Exists(file)) throw new Exception($"Output file '{file}' already exists");
			_execute.OutputFileUri = file;
		}

		private void Generate()
		{
			// set up hashing
			StreamWriter output = null;
			if (!_execute.Request.Preview) output = File.CreateText(_execute.OutputFileUri);

			var hasher = _execute.Request.HashType.ToString();
			using (var algorithm = HashCalc.GetHashAlgorithm(_execute.Request.HashType))
			{
				output?.WriteLine($"# Generated {hasher} with Expedition at {DateTime.UtcNow}");
				output?.WriteLine($"# https://github.com/DesignedSimplicity/Expedition/");
				output?.WriteLine("");

				// set up searching
				var searchPattern = _request.FilePattern ?? "*.*";
				var searchOption = _request.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

				// enumerate and hash files
				int count = 0;
				_execute.Files = Directory.GetFiles(_request.DirectoryUri, searchPattern, searchOption);
				foreach (var file in _execute.Files)
				{
					try
					{
						count++;
						_execute.Log($"{count}. {file} -> {hasher} = ");

						// calculate hash and output hash to log
						var hash = (_request.Preview
							? hasher
							: HashCalc.GetHash(file, algorithm));
						_execute.LogLine(hash);

						// format and write checksum to stream
						var path = _execute.GetRelativePath(file);
						output?.WriteLine($"{hash} {path}");
					}
					catch (Exception ex)
					{
						_execute.LogError(file, ex);
					}
				}

				// clean up output file
				output?.Flush();
				output?.Close();
				output?.Dispose();
			}
		}

		private void Complete()
		{
			// mark as completed
			//_response.Completed = DateTime.UtcNow;

			// show simple summary
			var log = _request.LogStream;
			//log?.WriteLine("Created");
		}
	}
}
