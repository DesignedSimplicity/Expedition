﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class CreateChecksums
	{
		public CreateChecksumsResponse Execute(CreateChecksumsRequest request)
		{
			var execute = new CreateChecksumsExecute(request);

			Validate(execute);

			Execute(execute);

			return new CreateChecksumsResponse(execute);
		}

		private void Validate(CreateChecksumsExecute execute)
		{
			// check input uri
			var dir = execute.Request.DirectoryUri;
			if (String.IsNullOrWhiteSpace(dir)) throw new Exception($"Input URI is not provided");
			if (!Directory.Exists(dir)) throw new Exception($"Input URI '{dir}' is not accessible");

			// check output uri
			var root = execute.Request.RelativeToUri;
			if (!String.IsNullOrWhiteSpace(root))
			{
				if (!Directory.Exists(execute.Request.RelativeToUri)) throw new Exception($"Output URI '{root}' is not accessible");
				if (!ParsePath.IsSamePath(root, dir) && !ParsePath.IsAncestor(root, dir)) throw new Exception($"Input URI '{root}' is not ancestor of output URI '{dir}'");
			}

			// check output file
			var now = DateTime.Now;
			var ext = execute.Request.HashType.ToString().ToLowerInvariant();
			var path = String.IsNullOrWhiteSpace(root)
				? dir
				: root;
			var file = Path.Combine(path, $"_{now:yyyyMMdd}-{now:HHmmss}.{ext}");
			if (File.Exists(file)) throw new Exception($"Output file '{file}' already exists");
			execute.OutputFileUri = file;
		}

		private void Execute(CreateChecksumsExecute execute)
		{
			// set up hashing
			StreamWriter output = null;
			if (!execute.Request.Preview) output = File.CreateText(execute.OutputFileUri);

			var hasher = execute.Request.HashType.ToString();
			using (var algorithm = HashCalc.GetHashAlgorithm(execute.Request.HashType))
			{
				output?.WriteLine($"# Generated {hasher} with Expedition at {DateTime.Now}");
				output?.WriteLine($"# https://github.com/DesignedSimplicity/Expedition/");
				output?.WriteLine("");

				// query file system
				var query = new QueryFileSystem();
				//TODO attach event handler here
				var queryResult = query.Execute(execute.Request);

				// enumerate and hash files
				int count = 0;
				var files = new List<FileInfo>();
				foreach (var file in queryResult.Files)
				{
					var fileName = file.FullName;

					// exclude/skip output file
					if (String.Compare(execute.OutputFileUri, fileName, true) == 0)
						continue;

					try
					{
						count++;
						files.Add(file);
						execute.Log($"{count}. {fileName} -> {hasher} = ");

						// calculate hash and output hash to log
						var hash = (execute.Request.Preview
							? hasher
							: HashCalc.GetHash(fileName, algorithm));
						execute.LogLine(hash);

						// format and write checksum to stream
						var path = execute.GetOuputPath(fileName);
						output?.WriteLine($"{hash} {path}");
					}
					catch (Exception ex)
					{
						execute.LogError(fileName, ex);
					}
				}

				// gather output files
				execute.Files = files.ToArray();

				// clean up output file
				output?.Flush();
				output?.Close();
				output?.Dispose();
			}
		}
	}
}
