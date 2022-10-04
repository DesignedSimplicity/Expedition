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
		private const string DateFormat = "yyyy-MM-dd";

		private readonly ExcelPackage _excel;

		private ExcelWorksheet _summary;
		private ExcelWorksheet _folders;
		private ExcelWorksheet _files;

		private bool _head = false;
		private bool _foot = false;
		private int _row;
		private int _col;

		public ParseExcel()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			_excel = new ExcelPackage();
			_summary = NewWorksheet("Summary");
			_folders = NewWorksheet("Folders");
			_files = NewWorksheet("Files");
		}

		public void SaveAs(string uri)
		{
			var excel = new FileInfo(uri);

			if (!_foot) FinishFileSheet();

			_excel.SaveAs(excel);
			_summary.Dispose();
			_folders.Dispose();
			_files.Dispose();
			_excel.Dispose();
		}

		public void Dispose()
		{
			_summary.Dispose();
			_folders.Dispose();
			_files.Dispose();
			_excel.Dispose();
		}

		private ExcelWorksheet NewWorksheet(string name)
		{
			var ws = _excel.Workbook.Worksheets.Add(name);
			ws.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
			ws.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
			ws.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);
			return ws;
		}

		private void AddWorksheet(string name)
		{
			_files = NewWorksheet(name);
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

		public void StartFileSheet()
		{
			AddWorksheet("Expedition.Files");

			_head = true;
			_row = 1;
			_col = 1;

			_files.Cells[_row, _col++].Value = "ExId";
			_files.Cells[_row, _col++].Value = "Status";
			_files.Cells[_row, _col++].Value = "Exception";
			_files.Cells[_row, _col++].Value = "Uri";
			_files.Cells[_row, _col++].Value = "Directory";
			_files.Cells[_row, _col++].Value = "File";
			_files.Cells[_row, _col++].Value = "Type";
			_files.Cells[_row, _col++].Value = "Size";
			_files.Cells[_row, _col++].Value = "Hash";
			_files.Cells[_row, _col++].Value = "Created";
			_files.Cells[_row, _col++].Value = "Modified";
		}


		public void AddFileInfo(FileInfo file, string status = null, string hash = null, string error = null)
		{
			if (!_head) StartFileSheet();

			_row++;
			var exid = _row - 1;

			var col = 1;
			if (String.IsNullOrWhiteSpace(status)) status = String.IsNullOrWhiteSpace(hash) ? "INFO" : "HASH";
			if (String.IsNullOrWhiteSpace(hash)) hash = "";
			if (String.IsNullOrWhiteSpace(error)) error = "";
			_files.Cells[_row, col++].Value = exid;
			_files.Cells[_row, col++].Value = status;
			_files.Cells[_row, col++].Value = error;
			_files.Cells[_row, col++].Value = file.FullName;
			_files.Cells[_row, col++].Value = file.Directory.Name;
			_files.Cells[_row, col++].Value = file.Name;
			_files.Cells[_row, col++].Value = file.Extension;
			_files.Cells[_row, col++].Value = file.Length;
			_files.Cells[_row, col++].Value = hash;
			_files.Cells[_row, col++].Value = file.CreationTimeUtc;
			_files.Cells[_row, col++].Value = file.LastWriteTimeUtc;
		}

		public void FinishFileSheet()
		{
			_foot = true;

			// do auto formatting
			AutoFormatColumns(_col - 3, NumberFormat);

			// do manual formatting
			//_worksheet.Column(_col).Style.Numberformat.Format = _percentFormat;
			//_worksheet.Column(_col).Style.Numberformat.Format = _moneyFormat;
			_files.Column(_col - 2).Style.Numberformat.Format = DateFormat;
			_files.Column(_col - 1).Style.Numberformat.Format = DateFormat;

			// auto size columns
			//AutoFitColumns(_col - 1);
		}
	}
}