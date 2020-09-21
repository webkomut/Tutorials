namespace Tutorials.ExcelParser
{
    public class ExcelParserOptions
    {
        public string FilePath { get; }
        public string WorksheetName { get; set; }
        public int HeaderRow { get; set; }

        public ExcelParserOptions(string filePaht)
        {
            FilePath = filePaht;
        }
    }

    public class ColumnInfo
    {
        public int Column { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}