using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ClosedXML.Excel;

namespace Tutorials.ExcelConverter
{
    public class ConvertToExcel
    {
        public XLWorkbook Converter<T>(ExcelOptions<T> options)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(GenerateSheeName(options.WorksheetName));
            var headerBeginRow = options.HeaderBeginRow;
            AddHeaderColumns<T>(worksheet, options.HeaderBeginRow);
            AddRows(worksheet, options);
            if(options.PaintSameRow)
                SameRowBgColor<T>(worksheet, options.WorksheetName);
            return workbook;
        }

        private void AddRows<T>(IXLWorksheet worksheet, ExcelOptions<T> options)
        {
            var culture = new CultureInfo("tr-TR");
            options.HeaderBeginRow++;
            var counter = 0;
            foreach (var item in options.Data)
            {
                var properties = item.GetType().GetProperties()
                    .Select(x => new
                    {
                        PropertyInfo = x,
                        Attr = x.GetCustomAttributes(false).Select(y => (ExcelAttribute) y).FirstOrDefault()
                    }).OrderBy(x => x.Attr?.Col).ToArray();

                for (var i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    var typeName = property.PropertyInfo.PropertyType.Name;
                    var value = property.PropertyInfo.GetValue(item, null);
                    if (typeName == "DateTimeOffset" || typeName == "DateTime")
                    {
                        var date = DateTimeOffset.Parse(value.ToString()).ToString("MM/dd/yyyy", culture);
                        worksheet.Cell(options.HeaderBeginRow, i + 1).Value = date;
                        var column = worksheet.Column(i + 1);
                        column.AdjustToContents();
                    }
                    else
                    {
                        var cell = worksheet.Cell(options.HeaderBeginRow, i + 1);
                        cell.Value = value ?? "";
                    }
                }
                counter++;
                options.HeaderBeginRow++;
            }
        }

        public static void SameRowBgColor<T>(IXLWorksheet worksheet, string workSheetName)
        {
            var properties = typeof(T).GetProperties();
            var firstWorksheet = worksheet.Workbook.Worksheets.First(x => x.Name == workSheetName);
            var rows = firstWorksheet.Rows().ToList();
            var values = new List<string>();
            foreach (var row in rows)
            {
                var rowValue = "";
                
                for (int j = 1; j <= properties.Length; j++)
                {
                    var cell = row.Cell(j);
                    rowValue += cell.Value;
                }
                var existsItem = values.Any(x => x == rowValue);

                if (existsItem)
                {
                    for (int j = 1; j <= properties.Length; j++)
                    {
                        var cell = row.Cell(j);
                        cell.Style.Fill.BackgroundColor = XLColor.Red;
                    }
                }
                values.Add(rowValue);
            }
        }

        private static void AddHeaderColumns<T>(IXLWorksheet worksheet, in int beginRow)
        {
            var properties = typeof(T).GetProperties();

            var columns = properties.Select(x => new
            {
                PropertyInfo = x,
                Attr = x.GetCustomAttributes(false).Select(y => (ExcelAttribute)y).FirstOrDefault()
            }).OrderBy(x => x.Attr?.Col).ToArray();
            if (columns.Any())
            {
                for (var i = 0; i < columns.Length; i++)
                {
                    var columnItem = columns[i];
                    var name = columnItem.Attr != null ? columnItem.Attr.Name : columnItem.PropertyInfo.Name;
                    worksheet.Cell(beginRow, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(beginRow, i + 1).Style.Font.FontSize = 12;
                    worksheet.Cell(beginRow, i + 1).Value = name;
                    var column = worksheet.Column(i + 1);
                    column.AdjustToContents();
                }
            }
            else
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    var name = properties[i].Name;
                    worksheet.Cell(beginRow, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(beginRow, i + 1).Style.Font.FontSize = 12;
                    worksheet.Cell(beginRow, i + 1).Value = name;
                    var column = worksheet.Column(i + 1);
                    column.AdjustToContents();
                }
            }
            
        }

        private string GenerateSheeName(string optionsWorksheetName)
        {
            if (string.IsNullOrWhiteSpace(optionsWorksheetName))
                return "Sheet1";
            return optionsWorksheetName.Length > 30 ? optionsWorksheetName.Substring(0, 30) : optionsWorksheetName;
        }
    }
}