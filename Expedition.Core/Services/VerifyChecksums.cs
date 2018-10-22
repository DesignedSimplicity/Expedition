﻿using Expedition.Core.Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition.Core.Services
{
	public class VerifyChecksums
	{
		public VerifyChecksumsResponse Execute(VerifyChecksumsRequest request)
		{
			var execute = new VerifyChecksumsExecute(request);

			Validate(execute);

			Execute(execute);

			return new VerifyChecksumsResponse(execute);
		}

		private bool IsHexidecimal(string text)
		{
			if (String.IsNullOrWhiteSpace(text))
				return false;

			foreach(var c in text)
			{
				if (!Char.IsLetterOrDigit(c))
					return false;
			}

			return true;
		}

		private void Validate(VerifyChecksumsExecute execute)
		{
			// check input uri
			var uri = execute.Request.FileUri;
			if (String.IsNullOrWhiteSpace(uri)) throw new Exception($"Input URI is not provided");
			if (!File.Exists(uri)) throw new Exception($"Input URI '{uri}' is not accessible");

			// find first parsable line of file
			using (var read = File.OpenText(uri))
			{
				var found = false;
				while (!found && read.Peek() > 0)
				{
					var line = read.ReadLine();
					if (!String.IsNullOrEmpty(line) && !line.StartsWith("#")) // skip comments
					{
						// replace tabs if exists
						int len = 0;
						var fix = line.Replace(@"\t", " ");

						// check first 40 characters is hex + 41 is space
						if (len == 0 && fix.Length > 40)
						{
							if (IsHexidecimal(fix.Substring(0, 40)) && fix[40] == ' ') len = 40;
						}

						// check first 32 characters is hex + 33 is space
						if (len == 0 && fix.Length > 32)
						{
							if (IsHexidecimal(fix.Substring(0, 32)) && fix[32] == ' ') len = 32;
						}

						// check if we have a hash formula
						if (len > 0)
						{
							// set up hash comparison type
							execute.HashType = len == 40
								? HashType.Sha1
								: HashType.Md5;

							// check file name exists
							var file = fix.Substring(len + 1).Trim();
							if (file.Length == 0) throw new Exception($"Input file does not contain a valid file paths");
							found = true;
						}
						else
							throw new Exception($"Input file does not contain a valid checksum format");
					}
				}
			}

			// save input file uri
			execute.InputFileUri = uri;

			// check excel file
			if (execute.Request.Report)
			{
				var now = DateTime.Now;
				var excel = $"{uri}_{now:yyyyMMdd}-{now:HHmmss}.xlsx";
				if (File.Exists(excel)) throw new Exception($"Output report file '{excel}' already exists");
				execute.ReportFileUri = excel;
			}
		}

		private void Execute(VerifyChecksumsExecute execute)
		{
			// set up reporting
			ParseExcel report = null;
			if (execute.Request.Report)
			{
				report = new ParseExcel();
				report.StartFileSheet();
			}

			var hasher = execute.HashType.ToString();
			using (var algorithm = HashCalc.GetHashAlgorithm(execute.HashType))
			{
				// query file system
				var query = new QueryHashSystem();
				var queryResult = query.Execute(execute.InputFileUri);

				// enumerate and hash files
				int count = 0;
				var files = new List<FileInfo>();
				foreach (var entry in queryResult.Entries)
				{
					var fileName = entry.Key;
					var file = new FileInfo(fileName);
					files.Add(file);

					try
					{
						count++;
						var log = new StringBuilder();
						log.Append($"{count}. {fileName} -> {hasher} = ");

						// calculate hash and output hash to log
						var hash = hasher;
						string error = null;
						var status = "PREVIEW";
						if (!execute.Request.Preview)
						{
							hash = HashCalc.GetHash(fileName, algorithm);
							var match = (String.Compare(hash, entry.Value, true) == 0);
							status = match ? "VALID" : "ERROR";
							if (!match)
							{
								error = "Hash Invalid";
								execute.LogError(entry.Key, new Exception(error));
							}
						}
						// write report data if requested
						report?.AddFileInfo(file, status, hash, error);

						// display to output
						log.Append($"{hash} {status}");
						execute.LogLine(log.ToString());
					}
					catch (Exception ex)
					{
						execute.LogError(entry.Key, ex);
					}
				}

				// save file list
				execute.Files = files.ToArray();
				execute.LogLine($"TOTAL FILES: {files.Count}");
				//if (queryResult.Errors.Count > 0) execute.LogLine($"QUERY ERRORS: {queryResult.Errors.Count}");
				if (execute.Exceptions.Count > 0) execute.LogLine($"EXCEPTIONS: {execute.Exceptions.Count()}");

				// close out report
				if (report != null)
				{
					execute.LogLine($"SAVING REPORT: {execute.ReportFileUri}");
					report.SaveAs(execute.ReportFileUri);
				}
			}
		}
	}
}
