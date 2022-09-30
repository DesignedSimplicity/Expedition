using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expedition2.Core2.Parse
{
	public class ParseExcel
	{
		private const string PercentFormat = "#0.0%";
		private const string NumberFormat = "###,###,##0";
		private const string MoneyFormat = "$###,###,##0.00";
		private const string _dateFormat = "yyyy-MM-dd";

		private readonly ExcelPackage _excel = new ExcelPackage();

		private ExcelWorksheet? _worksheet;
		private bool _head = false;
		private bool _foot = false;
		private int _row;
		private int _col;

		public ParseExcel()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		}

		public void SaveAs(string uri)
		{
			var excel = new FileInfo(uri);

			if (!_foot) FinishFileSheet();

			_excel.SaveAs(excel);
			_worksheet.Dispose();
			_excel.Dispose();
		}

		private void AddWorksheet(string name)
		{
			_worksheet = _excel.Workbook.Worksheets.Add(name);
			_worksheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
			_worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
			_worksheet.Row(1).Style.Font.Color.SetColor(System.Drawing.Color.White);
		}

		private void AutoFormatColumns(int count, string format)
		{
			for (var index = 1; index <= count; index++)
			{
				_worksheet.Column(index).Style.Numberformat.Format = format;
			}
		}

		private void AutoFitColumns(int count)
		{
			for (var index = 1; index <= count; index++)
			{
				_worksheet.Column(index).AutoFit();
			}
		}

		public void StartFileSheet()
		{
			AddWorksheet("Expedition.Files");

			_head = true;
			_row = 1;
			_col = 1;

			_worksheet.Cells[_row, _col++].Value = "ExId";
			_worksheet.Cells[_row, _col++].Value = "Status";
			_worksheet.Cells[_row, _col++].Value = "Exception";
			_worksheet.Cells[_row, _col++].Value = "Uri";
			_worksheet.Cells[_row, _col++].Value = "Directory";
			_worksheet.Cells[_row, _col++].Value = "File";
			_worksheet.Cells[_row, _col++].Value = "Type";
			_worksheet.Cells[_row, _col++].Value = "Size";
			_worksheet.Cells[_row, _col++].Value = "Hash";
			_worksheet.Cells[_row, _col++].Value = "Created";
			_worksheet.Cells[_row, _col++].Value = "Modified";
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
			_worksheet.Cells[_row, col++].Value = exid;
			_worksheet.Cells[_row, col++].Value = status;
			_worksheet.Cells[_row, col++].Value = error;
			_worksheet.Cells[_row, col++].Value = file.FullName;
			_worksheet.Cells[_row, col++].Value = file.Directory.Name;
			_worksheet.Cells[_row, col++].Value = file.Name;
			_worksheet.Cells[_row, col++].Value = file.Extension;
			_worksheet.Cells[_row, col++].Value = file.Length;
			_worksheet.Cells[_row, col++].Value = hash;
			_worksheet.Cells[_row, col++].Value = file.CreationTimeUtc;
			_worksheet.Cells[_row, col++].Value = file.LastWriteTimeUtc;
		}

		public void FinishFileSheet()
		{
			_foot = true;

			// do auto formatting
			AutoFormatColumns(_col - 3, NumberFormat);

			// do manual formatting
			//_worksheet.Column(_col).Style.Numberformat.Format = _percentFormat;
			//_worksheet.Column(_col).Style.Numberformat.Format = _moneyFormat;
			_worksheet.Column(_col - 2).Style.Numberformat.Format = _dateFormat;
			_worksheet.Column(_col - 1).Style.Numberformat.Format = _dateFormat;

			// auto size columns
			//AutoFitColumns(_col - 1);
		}
	}
}