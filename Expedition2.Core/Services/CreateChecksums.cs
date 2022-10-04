using Expedition2.Core.Parse;
using OfficeOpenXml.Style;
using Pastel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core.Services
{
	public class CreateChecksums
	{
		public CreateChecksumsResponse Execute(CreateChecksumsRequest request)
		{
			var execute = new CreateChecksumsExecute(request);

			Validate(execute);

			Prepare(execute);

			Execute(execute);

			Output(execute);

			Report(execute);

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

		private void Prepare(CreateChecksumsExecute execute)
		{
			var root = new DirectoryInfo(execute.Request.DirectoryUri);
			var patrol = execute.Factory.CreateNew(root);
			execute.PatrolSource = patrol;

			execute.Request.ConsoleOut?.WriteLine($"====================================================================================================");
			execute.Request.ConsoleOut?.WriteLine(root.FullName.Pastel(Color.Orange));
			execute.LogLine(root.FullName);

			execute.Request.ConsoleOut?.WriteLine($"----------------------------------------------------------------------------------------------------");
			List<FolderPatrolInfo> patrolFolders = execute.Factory.LoadFolders(root.FullName, true).ToList();
			patrolFolders.Insert(0, execute.Factory.GetFolder(root));
			execute.PatrolFolders = patrolFolders;			
			foreach (var folder in patrolFolders)
			{
				execute.Request.ConsoleOut?.WriteLine(folder.Uri.Pastel(Color.Yellow));
				execute.LogLine(folder.Uri);
				patrol.TotalFolderCount++;
			}

			execute.Request.ConsoleOut?.WriteLine($"----------------------------------------------------------------------------------------------------");
			execute.Request.ConsoleOut?.WriteLine($"Folders: {patrol.TotalFolderCount}");
			execute.Request.ConsoleOut?.WriteLine($"====================================================================================================");
		}

		private void Execute(CreateChecksumsExecute execute)
		{
			var console = execute.Request.ConsoleOut;

			var files = new List<FileInfo>();
			var patrolFiles = new List<FilePatrolInfo>();
			var patrolFolders = execute.PatrolFolders;

			// query file system
			var query = new QueryFileSystem();
			execute.Request.ErrorSafe = true;
			var queryResult = query.Execute(execute.Request);

			long totalDataProcessed = 0;
			var hasher = execute.Request.HashType.ToString();
			using (var algorithm = HashCalc.GetHashAlgorithm(execute.Request.HashType))
			{
				// enumerate and hash files
				int count = 0;
				FolderPatrolInfo? currentFolder = null;
				foreach (var file in queryResult.Files)
				{
					string hash = "";
					var fileName = file.FullName;

					// exclude/skip output file
					if (String.Compare(execute.OutputFileUri, fileName, true) == 0)
						continue;

					// match to folder
					// TODO BAD MATCH - make better, add caching from previous
					// TODO BAD MATCH - make better, add caching from previous
					// TODO BAD MATCH - make better, add caching from previous
					// TODO BAD MATCH - make better, add caching from previous
					// TODO BAD MATCH - make better, add caching from previous
					// TODO BAD MATCH - make better, add caching from previous
					var folder = patrolFolders.FirstOrDefault(x => x.Uri == file.DirectoryName);
					if (currentFolder?.Uri != folder?.Uri)
					{
						console?.WriteLine(folder?.Uri.Pastel(Color.Yellow));
						currentFolder = folder;
					}

					// process the next file
					try
					{
						count++;
						files.Add(file);

						execute.PatrolSource.TotalFileCount++;
						execute.PatrolSource.TotalFileSize += file.Length;
						if (folder != null)
						{
							execute.PatrolSource.TotalFolderCount++;
							folder.TotalFileCount++;
							folder.TotalFileSize += file.Length;
						}

						execute.SetState(new BaseFileSytemState(file));
						execute.Log($"{count}. {fileName} -> {file.Length:###,###,###,###,##0}");

						console?.Write($"{count}. ".Pastel(Color.WhiteSmoke));
						console?.Write($"{file.Name} -> ");
						console?.Write($"{file.Length:###,###,###,###,##0}".Pastel(Color.LightYellow));

						if (execute.Request.Preview)
						{
							execute.LogLine($" LOG");
							console?.WriteLine($" LOG".Pastel(Color.GreenYellow));
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
							console?.WriteLine($" {hasher.Pastel(Color.LightGreen)} = {hash.Pastel(Color.Green)} @ {rate:###,###,###,###,##0} b/ms".Pastel(Color.Gray));
						}

						// update patrol file
						var patrolFile = execute.Factory.GetFile(file);
						if (execute.Request.HashType == HashType.Sha512) patrolFile.Sha512 = hash; else patrolFile.Md5 = hash;
						patrolFiles.Add(patrolFile);


						// update file data state
						totalDataProcessed += file.Length;
						execute.SetState(new BaseFileSytemState(file, hash));

						// write report data if requested
						//report?.AddFileInfo(file, null, hash);
					}
					catch (Exception ex)
					{
						execute.LogError(fileName, ex);
						execute.SetState(new BaseFileSytemState(file, ex));
						//report?.AddFileInfo(file, null, hash, ex.Message);

						if (!execute.Request.ErrorSafe) throw;

						// TODO - deal with missing patrol file
						// TODO - deal with missing patrol file
						// TODO - deal with missing patrol file
						// TODO - deal with missing patrol file
						// TODO - deal with missing patrol file
					}
				}
			}

			// gather data files
			execute.PatrolFiles = patrolFiles;
			execute.Files = files.ToArray();

			// show folders summary
			console?.WriteLine($"====================================================================================================");
			foreach (var folder in patrolFolders)
			{
				console?.WriteLine($"{folder.Uri.Pastel(Color.Yellow)}\tFiles: {folder.TotalFileCount:###,###,###,###,###,##0}\tSize: {folder.TotalFileSize:###,###,###,###,###,##0}");
				execute.PatrolSource.TotalFolderCount++;
			}

			// show finaly summary
			console?.WriteLine($"----------------------------------------------------------------------------------------------------".Pastel(Color.Yellow));
			console?.Write(execute.PatrolSource.TargetFolderUri.Pastel(Color.DarkOrange));
			console?.WriteLine($"\tFolders: {execute.PatrolSource.TotalFolderCount:###,###,###,###,###,##0}\tFiles: {execute.PatrolSource.TotalFileCount:###,###,###,###,###,##0}\tSize: {execute.PatrolSource.TotalFileSize:###,###,###,###,###,##0}".Pastel(Color.WhiteSmoke));

			// show output
			var time = DateTime.Now.Subtract(execute.Request.Started);
			execute.PatrolSource.TotalSeconds = Convert.ToInt64(time.TotalSeconds);

			var mb = 1.0 * totalDataProcessed / 1024.0 / 1024.0;
			var gb = 1.0 * mb / 1024;
			var tb = 1.0 * gb / 1024.0;
			var gbh = 1.0 * gb / time.TotalHours;
			var mbs = 1.0 * mb / time.TotalMinutes;
			console?.WriteLine($"====================================================================================================".Pastel(Color.DarkOrange));
			console?.WriteLine($"BYTES: {totalDataProcessed:###,###,###,###,###,##0} = {mb:###,###,###,###,###,##0.0} MB = {gb:###,###,###,###,###,##0.0} GB = {tb:###,###,###,###,###,##0.0} TB".Pastel(Color.DarkGoldenrod));
			console?.WriteLine($"TIME: {time:hh\\:mm\\:ss} H:M:S = {time.TotalSeconds:###,###,###,###,##0} SEC".Pastel(Color.Goldenrod));
			console?.WriteLine($"RATE: {gbh:##,##0.0} GB/HOUR = {mbs:##,##0.0} MB/SEC".Pastel(Color.Gold));

			if (queryResult.Errors.Count > 0)
			{
				console?.WriteLine($"####################################################################################################".Pastel(Color.DarkRed));
				console?.WriteLine($"QUERY ERRORS: {queryResult.Errors.Count}".Pastel(Color.Red));
				execute.LogLine($"QUERY ERRORS: {queryResult.Errors.Count}");
			}
			if (execute.Exceptions.Count > 0)
			{
				console?.WriteLine($"####################################################################################################".Pastel(Color.DarkRed));
				console?.WriteLine($"EXCEPTIONS: {execute.Exceptions.Count()}".Pastel(Color.Red));
				execute.LogLine($"EXCEPTIONS: {execute.Exceptions.Count()}");
			}

			console?.WriteLine($"====================================================================================================".Pastel(Color.Cyan));
		}

		private void Output(CreateChecksumsExecute execute)
		{
			// set up hashing
			if (execute.Request.Preview || String.IsNullOrEmpty(execute.OutputFileUri))
			{
				return;
			}

			execute.Request.ConsoleOut?.Write($"SAVING {execute.PatrolFiles.Count()} {execute.Request.HashType} HASHES: {execute.OutputFileUri}".Pastel(Color.Cyan));
			execute.Log($"SAVING {execute.PatrolFiles.Count()} {execute.Request.HashType} HASHES: {execute.OutputFileUri}");

			// create md5/sha512 output file header
			StreamWriter output = File.CreateText(execute.OutputFileUri);
			output.WriteLine($"# Generated {execute.Request.HashType} with Patrol at UTC {DateTime.UtcNow}");
			output.WriteLine($"# https://github.com/DesignedSimplicity/Expedition/");
			output.WriteLine($"# ==================================================");
			output.WriteLine($"# System Name: {Environment.MachineName}");
			output.WriteLine($"# Patrol Source: {execute.PatrolSource.SourcePatrolUri}");
			output.WriteLine($"# Target Folder: {execute.PatrolSource.TargetFolderUri}");
			output.WriteLine($"# Hash: {execute.Request.HashType}");
			output.WriteLine($"# Size: {execute.PatrolSource.TotalFileSize:###,###,###,###,###,##0}");
			output.WriteLine($"# Files: {execute.PatrolSource.TotalFileCount:###,###,###,###,###,##0}");
			output.WriteLine($"# Folders: {execute.PatrolSource.TotalFolderCount:###,###,###,###,###,##0}");
			output.WriteLine($"# --------------------------------------------------");
			foreach (var folder in execute.PatrolFolders.OrderBy(x => x.Uri))
			{
				output.WriteLine($"# Files: {folder.TotalFileCount:###,###,###,###,###,##0}\tSize: {folder.TotalFileSize:###,###,###,###,###,##0}\t\t{folder.Uri}");
			}
			output.WriteLine("");

			// format and write checksum to stream
			foreach (var file in execute.PatrolFiles.OrderBy(x => x.Uri))
			{
				var path = execute.GetOuputPath(file.Uri);
				output?.WriteLine($"{file.Md5} {path}");
			}

			// clean up output file
			execute.LogLine($"\tDONE");
			execute.Request.ConsoleOut?.WriteLine($"\tDONE".Pastel(Color.Cyan));
			output.Flush();
			output.Close();
			output.Dispose();
		}

		private void Report(CreateChecksumsExecute execute)
		{
			if (!execute.Request.Report || String.IsNullOrWhiteSpace(execute.ReportFileUri))
			{
				return;
			}

			execute.Request.ConsoleOut?.Write($"SAVING PATROL REPORT: {execute.ReportFileUri}".Pastel(Color.Cyan));
			execute.Log($"SAVING PATROL REPORT: {execute.ReportFileUri}".Pastel(Color.Cyan));

			// set up reporting
			ParseExcel report = new ParseExcel();
			report.StartFileSheet();

			// format and write checksum to stream
			foreach (var file in execute.PatrolFiles.OrderBy(x => x.Uri))
			{
				//var path = execute.GetOuputPath(file.Uri);
				var f = new FileInfo(file.Uri);
				report?.AddFileInfo(f, "Create", file.Md5);
			}

			// close out report
			execute.LogLine($"\tDONE");
			execute.Request.ConsoleOut?.WriteLine($"\tDONE".Pastel(Color.Cyan));
			report.SaveAs(execute.ReportFileUri);
			report.Dispose();
		}
	}
}
