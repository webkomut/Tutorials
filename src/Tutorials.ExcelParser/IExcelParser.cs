using System.Collections.Generic;

namespace Tutorials.ExcelParser
{
    public interface IExcelParser
    {
        IEnumerable<T> Parser<T>(ExcelParserOptions options) where T : new();
    }
}