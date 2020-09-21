using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace Tutorials.ExcelParser
{
    public class ExcelParser : IExcelParser
    {
        public IEnumerable<T> Parser<T>(ExcelParserOptions options) where T : new()
        {
            var fileInfo = new FileInfo(options.FilePath);
            using var package = new ExcelPackage(fileInfo);

            var list = new List<T>();
            var worksheet = package.Workbook.Worksheets.First(x => x.Name == options.WorksheetName);

            var headers = worksheet.Cells.Where(x => x.Start.Row == options.HeaderRow).Select(x => new ColumnInfo
            {
                Name = x.Address,
                Column = x.Start.Column,
                Value = x.Value.ToString().Trim().TrimEnd().TrimStart()
            }).ToList();

            var cells = worksheet.Cells.ToList();

            var groupCell = cells.Where(x => x.Start.Row > options.HeaderRow)
                .GroupBy(x => x.Start.Row, arg => arg, (row, columns) => new
                {
                    Row = row,
                    Items = columns.ToList()
                }).ToList();

            var rowOrder = options.HeaderRow + 1;

            foreach (var cell in groupCell)
            {
                var itemModel = new T();
                var properties = itemModel.GetType().GetProperties()
                    .Select(x => new
                    {
                        PropertyInfo = x,
                        Attr = x.GetCustomAttributes(false).Select(y => (ExcelAttribute) y).FirstOrDefault()
                    }).ToArray();

                foreach (var property in properties)
                {
                    var propName = property.Attr != null ? property.Attr.Name : property.PropertyInfo.Name;
                    var columnInfo = headers.FirstOrDefault(x => x.Value.ToString() == propName);
                    if (columnInfo == null)
                    {
                        continue;
                    }

                    var value = cell.Items.FirstOrDefault(x =>
                        x.Start.Column == columnInfo.Column && x.Start.Row == rowOrder);
                    if (value != null && !string.IsNullOrWhiteSpace(value.Text))
                    {
                        property.PropertyInfo.SetValue(itemModel,
                            Convert.ChangeType(value.Text, property.PropertyInfo.PropertyType), null);
                    }
                }

                rowOrder++;
                list.Add(itemModel);
            }

            return list;
        }
    }
}