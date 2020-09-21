using System;

namespace Tutorials.ExcelParser
{
    public class ExcelAttribute : Attribute
    {
        public string Name { get; set; }

        public ExcelAttribute(string name)
        {
            Name = name;
        }
    }
}