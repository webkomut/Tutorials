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
        [Excel("Yaþý")]
        public int Age { get; set; }
        [Excel("Adý")]
        public string Name { get; set; }
        [Excel("Soyadý")]
        public string Surname { get; set; }
        [Excel("Doðum Tarihi")]
        public DateTime BirthDate { get; set; }
        [Excel("Cinsiyet")]
        public string Gender { get; set; }
    }
}
