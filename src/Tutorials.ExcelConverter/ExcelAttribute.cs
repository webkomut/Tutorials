using System;

namespace Tutorials.ExcelConverter
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelAttribute : Attribute
    {
        public string Name { get; set; }
        public int Col { get; set; }

        public ExcelAttribute()
        {
            
        }

        public ExcelAttribute(string name)
        {
            Name = name;
        }
        public ExcelAttribute(string name, int col)
        {
            Name = name;
            Col = col;
        }
    }
}