using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Xunit;

namespace Tutorials.ExcelConverter.Test
{
    public class ConvertToExcelTest
    {
        private readonly ConvertToExcel convert;

        public ConvertToExcelTest()
        {
            convert = new ConvertToExcel();
        }

        [Fact]
        public async void Convert_To_Excel()
        {
            var personnels = new List<Personnel>();
            for (var i = 0; i < 30; i++)
            {
                var model = new Personnel
                {
                    Name = $"Name {i+1}",
                    Surname = $"Surname {i+1}",
                    Age = 38,
                    BirthDate = new DateTime(1983, 1, 4),
                    Gender = Gender.Male
                };
                if(i == 10)
                {
                    personnels.Add(model);
                }
                if (i == 17)
                {
                    personnels.Add(model);
                    personnels.Add(model);
                }
                personnels.Add(model);
            }

            await using var stream = new MemoryStream();
            var options = new ExcelOptions<Personnel>
            {
                Data = personnels.ToArray(),
                HeaderBeginRow = 3,
                WorksheetName = "Personnels",
                PaintSameRow = true
            };
            var result = convert.Converter(options);
            result.SaveAs(stream);
            var content = stream.ToArray();
            var myDocFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = Path.Combine(myDocFolder, $"MyExcel_{Guid.NewGuid()}.xlsx");
            await using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            await fileStream.WriteAsync(stream.GetBuffer(), 0, content.Length);
        }
    }

    public class Personnel
    {
        [Excel("Adi", 2)]
        public string Name { get; set; }
        [Excel("Soyadi", 3)]
        public string Surname { get; set; }
        //[Excel("Yaþý", 1)]
        public int Age { get; set; }
        [Excel("Dogum Tarihi", 4)]
        public DateTime BirthDate { get; set; }
        [Excel("Cinsiyet", 4)]
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
