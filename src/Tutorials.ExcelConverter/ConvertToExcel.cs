using System;
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
            AddHeaderColumns<T>(worksheet, options.HeaderBeginRow);
            AddRows(worksheet, options);
            return workbook;
        }

        private void AddRows<T>(IXLWorksheet worksheet, ExcelOptions<T> options)
        {
            var culture = new CultureInfo("tr-TR");
            options.HeaderBeginRow++;
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

                options.HeaderBeginRow++;
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