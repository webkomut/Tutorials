using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Tutorials.ExcelParser.Test
{
    public class ExcelParserTest
    {
        private readonly IExcelParser parser;

        public ExcelParserTest()
        {
            parser = new ExcelParser();
        }
        [Fact]
        public void Excel_Parser()
        {
            var myDocumentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var excelFilePath = Path.Combine(myDocumentFolder, "MyExcelFile.xlsx");

            var options = new ExcelParserOptions(excelFilePath)
            {
                HeaderRow = 3,
                WorksheetName = "Personnels"
            };
            var result = parser.Parser<Personnel>(options);
            Assert.True(result.Any());
        }
    }

    public class Personnel
    {
        [Excel("Ya��")]
        public int Age { get; set; }
        [Excel("Ad�")]
        public string Name { get; set; }
        [Excel("Soyad�")]
        public string Surname { get; set; }
        [Excel("Do�um Tarihi")]
        public DateTime BirthDate { get; set; }
        [Excel("Cinsiyet")]
        public string Gender { get; set; }
    }
}
