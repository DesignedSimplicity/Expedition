using CommandLine;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core.Parse
{
	public class ParseExcel
	{
		private const string PercentFormat = "#0.0%";
		private const string NumberFormat = "###,###,##0";
		private const string MoneyFormat = "$###,###,##0.00";
		private const string DateFormat = "yyyy-MM-dd HH:mm:ss";

		private readonly ExcelPackage _package;

		private ExcelWorksheet _sources;
		private ExcelWorksheet _folders;
		private ExcelWorksheet _files;

		private bool _fileHead = false;
		private bool _fileFoot = false;
		private int _fileRow;
		private int folderCol;

		public ParseExcel()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_package = new ExcelPackage();
			_sources = NewWorksheet("Sources");
			_folders = NewWorksheet("Folders");
			_files = NewWorksheet("Files");
		}

		public void SaveAs(string uri)
		{
			var excel = new FileInfo(uri);

			if (!_fileFoot) FinishFileSheet();

			_package.SaveAs(excel);
		}

		public void Dispose()
		{
			_sources.Dispose();
			_folders.Dispose();
			_files.Dispose();
			_package.Dispose();
		}

		private ExcelWorksheet NewWorksheet(string name)
		{
			var ws = _package.Workbook.Worksheets.Add(name);
			ws.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
			ws.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
			ws.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);
			return ws;
		}

		private void AutoFormatColumns(int count, string format)
		{
			for (var index = 1; index <= count; index++)
			{
				_files.Column(index).Style.Numberformat.Format = format;
			}
		}

		private void AutoFitColumns(int count)
		{
			for (var index = 1; index <= count; index++)
			{
				_files.Column(index).AutoFit();
			}
		}

		public void PopulateSource(SourcePatrolInfo source)
		{
			PopulateSources(new List<SourcePatrolInfo> { source });
		}

		public void PopulateSources(IEnumerable<SourcePatrolInfo> sources)
		{
			var row = 1;
			var col = 1;
			_sources.Cells[row, col++].Value = "ExId";
			_sources.Cells[row, col++].Value = "Guid";
			_sources.Cells[row, col++].Value = "SystemName";
			_sources.Cells[row, col++].Value = "PatrolType";
			_sources.Cells[row, col++].Value = "HashType";
			_sources.Cells[row, col++].Value = "SourcePatrolUri";
			_sources.Cells[row, col++].Value = "TargetFolderUri";
			_sources.Cells[row, col++].Value = "TotalFileSize";
			_sources.Cells[row, col++].Value = "TotalFileCount";
			_sources.Cells[row, col++].Value = "TotalFolderCount";
			_sources.Cells[row, col++].Value = "TotalSeconds";
			_sources.Cells[row, col++].Value = "CreatedUTC";
			_sources.Cells[row, col++].Value = "UpdatedUTC";

			foreach (var source in sources)
			{
				row++;
				col = 1;
				_sources.Cells[row, col++].Value = row - 1;
				_sources.Cells[row, col++].Value = "TODO"; // source.Guid;
				_sources.Cells[row, col++].Value = source.SystemName;
				_sources.Cells[row, col++].Value = source.SourceType.ToString();
				_sources.Cells[row, col++].Value = source.HashType.ToString();
				_sources.Cells[row, col++].Value = source.SourcePatrolUri;
				_sources.Cells[row, col++].Value = source.TargetFolderUri;
				_sources.Cells[row, col++].Value = source.TotalFileSize;
				_sources.Cells[row, col++].Value = source.TotalFileCount;
				_sources.Cells[row, col++].Value = source.TotalFolderCount;
				_sources.Cells[row, col++].Value = source.TotalSeconds;
				_sources.Cells[row, col++].Value = source.Created;
				_sources.Cells[row, col++].Value = source.Updated;
			}

			_sources.Column(1).Style.Numberformat.Format = NumberFormat;
			_sources.Column(8).Style.Numberformat.Format = NumberFormat;
			_sources.Column(9).Style.Numberformat.Format = NumberFormat;
			_sources.Column(10).Style.Numberformat.Format = NumberFormat;
			_sources.Column(11).Style.Numberformat.Format = NumberFormat;
			_sources.Column(12).Style.Numberformat.Format = DateFormat;
			_sources.Column(13).Style.Numberformat.Format = DateFormat;
		}

		public void PopulateFolders(IEnumerable<FolderPatrolInfo> folders)
		{
			var row = 1;
			var col = 1;
			_folders.Cells[row, col++].Value = "ExId";
			_folders.Cells[row, col++].Value = "Guid";
			_folders.Cells[row, col++].Value = "FullUri";
			_folders.Cells[row, col++].Value = "DirectoryName";
			_folders.Cells[row, col++].Value = "TotalFileCount";
			_folders.Cells[row, col++].Value = "TotalFileSize";
			_folders.Cells[row, col++].Value = "CreatedUTC";
			_folders.Cells[row, col++].Value = "UpdatedUTC";

			foreach (var folder in folders)
			{
				row++;
				col = 1;
				_folders.Cells[row, col++].Value = row - 1;
				_folders.Cells[row, col++].Value = folder.Guid;
				_folders.Cells[row, col++].Value = folder.Uri;
				_folders.Cells[row, col++].Value = folder.Name;
				_folders.Cells[row, col++].Value = folder.TotalFileCount;
				_folders.Cells[row, col++].Value = folder.TotalFileSize;
				_folders.Cells[row, col++].Value = folder.Created;
				_folders.Cells[row, col++].Value = folder.Updated;
			}

			_folders.Column(1).Style.Numberformat.Format = NumberFormat;
			_folders.Column(5).Style.Numberformat.Format = NumberFormat;
			_folders.Column(6).Style.Numberformat.Format = NumberFormat;
			_folders.Column(7).Style.Numberformat.Format = DateFormat;
			_folders.Column(8).Style.Numberformat.Format = DateFormat;
		}

		public void PopulateFiles(IEnumerable<FilePatrolInfo> files)
		{
			if (!_fileHead) StartFileSheet();

			foreach (var file in files)
			{
				_fileRow++;
				var exid = _fileRow - 1;

				var col = 1;
				_files.Cells[_fileRow, col++].Value = exid;
				_files.Cells[_fileRow, col++].Value = file.Guid;
				_files.Cells[_fileRow, col++].Value = file.Uri;
				_files.Cells[_fileRow, col++].Value = file.Directory;
				_files.Cells[_fileRow, col++].Value = file.Name;
				_files.Cells[_fileRow, col++].Value = file.Extension;
				_files.Cells[_fileRow, col++].Value = file.FileSize;
				_files.Cells[_fileRow, col++].Value = file.FileStatus.ToString();
				_files.Cells[_fileRow, col++].Value = file.Md5;
				_files.Cells[_fileRow, col++].Value = file.Md5Status.ToString();
				_files.Cells[_fileRow, col++].Value = file.Sha512;
				_files.Cells[_fileRow, col++].Value = file.Sha512Status.ToString();
				_files.Cells[_fileRow, col++].Value = file.Created;
				_files.Cells[_fileRow, col++].Value = file.Updated;
				_files.Cells[_fileRow, col++].Value = file.LastVerified;
			}

			FinishFileSheet();
		}

		public void StartFileSheet()
		{
			_fileHead = true;
			_fileRow = 1;
			folderCol = 1;
			_files.Cells[_fileRow, folderCol++].Value = "ExId";
			_files.Cells[_fileRow, folderCol++].Value = "Guid";
			_files.Cells[_fileRow, folderCol++].Value = "FullUri";
			_files.Cells[_fileRow, folderCol++].Value = "DirectoryName";
			_files.Cells[_fileRow, folderCol++].Value = "FileName";
			_files.Cells[_fileRow, folderCol++].Value = "FileExtension";
			_files.Cells[_fileRow, folderCol++].Value = "FileSize";
			_files.Cells[_fileRow, folderCol++].Value = "FileStatus";
			_files.Cells[_fileRow, folderCol++].Value = "Md5";
			_files.Cells[_fileRow, folderCol++].Value = "Md5Status";
			_files.Cells[_fileRow, folderCol++].Value = "Sha512";
			_files.Cells[_fileRow, folderCol++].Value = "Sha512Status";
			_files.Cells[_fileRow, folderCol++].Value = "CreatedUTC";
			_files.Cells[_fileRow, folderCol++].Value = "UpdatedUTC";
			_files.Cells[_fileRow, folderCol++].Value = "VerifiedUTC";
		}


		public void AddFileInfo(FileInfo file, string status = null, string hash = null, string error = null)
		{
		}

		public void FinishFileSheet()
		{
			_fileFoot = true;

			// do auto formatting
			//AutoFormatColumns(7, NumberFormat);
			_files.Column(7).Style.Numberformat.Format = NumberFormat;

			// do manual formatting
			//_worksheet.Column(_col).Style.Numberformat.Format = _percentFormat;
			//_worksheet.Column(_col).Style.Numberformat.Format = _moneyFormat;
			_files.Column(13).Style.Numberformat.Format = DateFormat;
			_files.Column(14).Style.Numberformat.Format = DateFormat;
			_files.Column(15).Style.Numberformat.Format = DateFormat;

			// auto size columns
			//AutoFitColumns(_col - 1);
		}
	}
}