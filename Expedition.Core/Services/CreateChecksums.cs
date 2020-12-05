using Expedition.Core.Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			// prepare output files
			var now = DateTime.Now;
			var ext = execute.Request.HashType.ToString().ToLowerInvariant();
			var path = String.IsNullOrWhiteSpace(root)
				? dir
				: root;
			var name = $"_{now:yyyyMMdd}-{now:HHmmss}";

			// check hash file
			var hash = Path.Combine(path, $"{name}.{ext}");
			if (File.Exists(hash)) throw new Exception($"Output hash file '{name}' already exists");
			execute.OutputFileUri = hash;

			// check excel file
			if (execute.Request.Report)
			{
				var excel = Path.Combine(path, $"{name}.xlsx");
				if (File.Exists(excel)) throw new Exception($"Output report file '{name}' already exists");
				execute.ReportFileUri = excel;
			}
		}

		private void Execute(CreateChecksumsExecute execute)
		{
			// set up hashing
			StreamWriter output = null;
			if (!execute.Request.Preview)
			{
				output = File.CreateText(execute.OutputFileUri);
			}

			// set up reporting
			ParseExcel report = null;
			if (execute.Request.Report)
			{
				report = new ParseExcel();
				report.StartFileSheet();
			}

            long totalDataProcessed = 0;
			var hasher = execute.Request.HashType.ToString();
			using (var algorithm = HashCalc.GetHashAlgorithm(execute.Request.HashType))
			{
				output?.WriteLine($"# Generated {hasher} with Expedition at {DateTime.Now}");
				output?.WriteLine($"# https://github.com/DesignedSimplicity/Expedition/");
				output?.WriteLine("");

				// query file system
				var query = new QueryFileSystem();
				//TODO attach event handler here
				execute.Request.Silent = true;
				var queryResult = query.Execute(execute.Request);

				// enumerate and hash files
				int count = 0;
				var files = new List<FileInfo>();
				foreach (var file in queryResult.Files)
				{
					string hash = null;
					var fileName = file.FullName;

					// exclude/skip output file
					if (String.Compare(execute.OutputFileUri, fileName, true) == 0)
						continue;
					
					try
					{
						
						count++;
						files.Add(file);
						execute.SetState(new BaseFileSytemState(file));
						execute.Log($"{count}. {fileName} -> {file.Length:###,###,###,###,##0}");
                        if (execute.Request.Preview)
						{
                            execute.LogLine($" LOG");
						}
						else
						{
							// calculate hash and output hash to log
							var s = new Stopwatch();
							s.Start();
							hash = HashCalc.GetHash(fileName, algorithm);
							var rate = file.Length / (s.ElapsedMilliseconds + 1);
							s.Stop();
							execute.LogLine($" {hasher} = {hash} @ {rate:###,###,###,###,##0} b/ms");
						}

						// format and write checksum to stream
						var path = execute.GetOuputPath(fileName);
						output?.WriteLine($"{hash} {path}");

						// update file data state
						totalDataProcessed += file.Length;
						execute.SetState(new BaseFileSytemState(file, hash));

						// write report data if requested
						report?.AddFileInfo(file, null, hash);
					}
					catch (Exception ex)
					{
						execute.LogError(fileName, ex);
						execute.SetState(new BaseFileSytemState(file, ex));
						report?.AddFileInfo(file, null, hash, ex.Message);
					}
				}

                // gather output files
                execute.Files = files.ToArray();

                // show output
                var time = DateTime.Now.Subtract(execute.Request.Started);
				var mb = totalDataProcessed / 1024.0 / 1024.0;
				var gb = mb / 1024;
				var tb = gb / 1024;
				var gbh = gb / time.TotalHours;
				var mbs = mb / time.TotalMinutes;
				execute.LogLine($"==================================================");
				execute.LogLine($"FILES: {files.Count:###,###,###,###,###,##0}");
				execute.LogLine($"BYTES: {totalDataProcessed:###,###,###,###,###,##0} = {gb:###,###,###,###,###,##0} GB = {tb:###,###,###,###,###,##0} TB");
				execute.LogLine($"TIME: {time:hh\\:mm\\:ss} H:M:S = {time.TotalSeconds:###,###,###,###,##0} SEC");
				execute.LogLine($"--------------------------------------------------");
				execute.LogLine($"RATE: {gbh:##,##0.0} GB/HOUR = {mbs:##,##0.0} MB/SEC");

				if (queryResult.Errors.Count > 0)
				{
					execute.LogLine($"##################################################");
					execute.LogLine($"QUERY ERRORS: {queryResult.Errors.Count}");
				}
				if (execute.Exceptions.Count > 0)
				{
					execute.LogLine($"##################################################");
					execute.LogLine($"EXCEPTIONS: {execute.Exceptions.Count()}");
				}

				// clean up output file
				if (output != null)
				{
					execute.LogLine($"==================================================");
					execute.LogLine($"SAVING HASHES: {execute.OutputFileUri}");
					output?.Flush();
					output?.Close();
					output?.Dispose();
				}

				// close out report
				if (report != null)
				{
					execute.LogLine($"--------------------------------------------------");
					execute.LogLine($"SAVING REPORT: {execute.ReportFileUri}");
					report.SaveAs(execute.ReportFileUri);
				}
			}
		}
	}
}
