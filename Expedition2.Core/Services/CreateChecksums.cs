using CommandLine;
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

			execute.Request.ConsoleOut?.WriteLine($"----------------------------------------------------------------------------------------------------");
			execute.Request.ConsoleOut?.WriteLine(root.FullName.Pastel(Color.Yellow));
			execute.PatrolFolders.Add(new FolderPatrolInfo(root));
			execute.PatrolSource.TotalFolderCount = 1;

			foreach (var dir in root.EnumerateDirectories("*", new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = true }).OrderBy(x => x.FullName))
			{
				var patrolFolder = new FolderPatrolInfo(dir);
				execute.PatrolFolders.Add(patrolFolder);
				execute.PatrolSource.TotalFolderCount++;
				/*
				try
				{
					var patrolFolder = new FolderPatrolInfo(dir);
					execute.PatrolFolders.Add(patrolFolder);
				}
				catch (UnauthorizedAccessException ex)
				{
					// log exception and re-throw if not silent
					execute.LogError(dir.FullName, ex);
					if (execute.Request.ErrorSafe)
					{
						execute.Request.ConsoleOut?.Write($"FOLDER ERROR: {dir.FullName}=".Pastel(Color.Red));
						execute.Request.ConsoleOut?.WriteLine(ex.ToString().Pastel(Color.DarkRed));
					}
					else
						throw;
				}*/
			}
			execute.Request.ConsoleOut?.WriteLine($"----------------------------------------------------------------------------------------------------");
			foreach (var patrolFolder in execute.PatrolFolders)
			{
				var dir = new DirectoryInfo(patrolFolder.Uri);
				execute.Request.ConsoleOut?.Write(dir.FullName.Pastel(Color.Yellow));
				execute.LogLine(dir.FullName);
				foreach (var file in dir.EnumerateFiles(execute.Request.FilePattern, new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false }))
				{
					var patrolFile = new FilePatrolInfo(file);
					execute.PatrolFiles.Add(patrolFile);
					patrolFolder.Files.Add(patrolFile);

					execute.PatrolSource.TotalFileCount++;
					execute.PatrolSource.TotalFileSize += file.Length;

					execute.Request.ConsoleOut?.Write('.');
					/*
					try
					{
						var patrolFile = new FilePatrolInfo(file);
						execute.PatrolFiles.Add(patrolFile);
						patrolFolder.Files.Add(patrolFile);

						execute.PatrolSource.TotalFileCount++;
						execute.PatrolSource.TotalFileSize += file.Length;
					}
					catch (UnauthorizedAccessException ex)
					{
						// log exception and re-throw if not silent
						execute.LogError(file.FullName, ex);
						if (execute.Request.ErrorSafe)
						{
							execute.Request.ConsoleOut?.Write($"FILE ERROR: {file.FullName}=".Pastel(Color.Red));
							execute.Request.ConsoleOut?.WriteLine(ex.ToString().Pastel(Color.DarkRed));
						}
						else
							throw;
					}*/
				}
				execute.Request.ConsoleOut?.WriteLine();
			}

			execute.Request.ConsoleOut?.WriteLine($"----------------------------------------------------------------------------------------------------");
			execute.Request.ConsoleOut?.WriteLine($"Folders: {patrol.TotalFolderCount}");
			execute.Request.ConsoleOut?.WriteLine($"====================================================================================================");
		}

		private void Execute(CreateChecksumsExecute execute)
		{
			var console = execute.Request.ConsoleOut;

			var patrol = execute.PatrolSource;
			var patrolFiles = execute.PatrolFiles;
			var patrolFolders = execute.PatrolFolders;

			long totalDataProcessed = 0;
			var hasher = execute.Request.HashType.ToString();
			using (var algorithm = HashCalc.GetHashAlgorithm(execute.Request.HashType))
			{
				int count = 0;
				foreach(var folder in patrolFolders)
				{
					console?.WriteLine(folder.Uri.Pastel(Color.Yellow));
					foreach (var file in folder.Files)
					{
						count++;
						string hash = "";
						var fileName = file.Uri;

						totalDataProcessed += file.FileSize;

						execute.Log($"{count:###,###,###,###,##0} of {patrol.TotalFileCount:###,###,###,###,##0} {fileName} -> {file.FileSize:###,###,###,###,##0}");
						console?.Write($"[{count:###,###,###,###,##0} of {patrol.TotalFileCount:###,###,###,###,##0}] ".Pastel(Color.Gray));
						console?.Write($"{file.Name} -> ");
						console?.Write($"{file.FileSize:###,###,###,###,##0}".Pastel(Color.LightYellow));

						if (execute.Request.Preview)
						{
							execute.LogLine($" LOG");
							console?.WriteLine($" LOG".Pastel(Color.GreenYellow));
						}
						else
						{
							// calculate hash and output hash to log
							console?.Write($" {hasher.Pastel(Color.LightGreen)} = ");
							var s = new Stopwatch();
							s.Start();
							try
							{
								hash = HashCalc.GetHash(fileName, algorithm);
								var rate = file.FileSize / (s.ElapsedMilliseconds + 1);
								s.Stop();
								execute.LogLine($" {hasher} = {hash} @ {rate:###,###,###,###,##0} b/ms");
								console?.WriteLine($"{hash.Pastel(Color.Green)} @ {rate:###,###,###,###,##0} b/ms".Pastel(Color.Gray));

								file.FileStatus = FileStatus.Exists;
								if (!execute.Request.Preview)
								{
									if (execute.Request.HashType == HashType.Sha512)
									{
										file.Sha512 = hash;
										file.Sha512Status = HashStatus.Created;
										file.LastVerified = DateTime.UtcNow;
									}
									else
									{
										file.Md5 = hash;
										file.Md5Status = HashStatus.Created;
										file.LastVerified = DateTime.UtcNow;
									}
								}
							}
							catch (UnauthorizedAccessException ex)
							{
								s.Stop();
								file.FileStatus = FileStatus.Error;

								// log exception and re-throw if not silent
								execute.LogError(fileName, ex);
								if (execute.Request.ErrorSafe)
								{
									console?.WriteLine($"UnauthorizedAccessException".Pastel(Color.Red));
								}
								else
									throw;
							}
						}


					}
				}
			}

			// gather data files
			//execute.PatrolFiles = patrolFiles;
			//execute.Files = files.ToArray();

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

			/*
			if (queryResult.Errors.Count > 0)
			{
				console?.WriteLine($"####################################################################################################".Pastel(Color.DarkRed));
				console?.WriteLine($"QUERY ERRORS: {queryResult.Errors.Count}".Pastel(Color.Red));
				execute.LogLine($"QUERY ERRORS: {queryResult.Errors.Count}");
				foreach (var error in queryResult.Errors)
				{
					console?.Write($"{error.Key}=".Pastel(Color.Red));
					console?.WriteLine(error.Value.ToString().Pastel(Color.DarkRed));
					execute.LogLine($"{error.Key}={error.Value}");
				}
				
			}
			*/
			if (execute.Exceptions.Count > 0)
			{
				console?.WriteLine($"####################################################################################################".Pastel(Color.DarkRed));
				console?.WriteLine($"EXCEPTIONS: {execute.Exceptions.Count()}".Pastel(Color.Red));
				execute.LogLine($"EXCEPTIONS: {execute.Exceptions.Count()}");
				foreach(var exception in execute.Exceptions)
				{
					console?.Write($"{exception.Key}=".Pastel(Color.Red));
					console?.WriteLine(exception.Value.ToString().Pastel(Color.DarkRed));
					execute.LogLine($"{exception.Key}={exception.Value}");
				}
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
			output.WriteLine($"# System Name: {execute.PatrolSource.SystemName}");
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

			// format and write checksum to stream
			report.PopulateSource(execute.PatrolSource);
			report.PopulateFolders(execute.PatrolFolders.OrderBy(x => x.Uri));
			report.PopulateFiles(execute.PatrolFiles.OrderBy(x => x.Uri));

			// close out report
			execute.LogLine($"\tDONE");
			execute.Request.ConsoleOut?.WriteLine($"\tDONE".Pastel(Color.Cyan));
			report.SaveAs(execute.ReportFileUri);
			report.Dispose();
		}
	}
}
