namespace Tutorials.ExcelConverter
{
    public class ExcelOptions<T>
    {
        public T[] Data { get; set; }
        public string WorksheetName { get; set; }
        public int HeaderBeginRow { get; set; }
        public bool PaintSameRow { get; set; }
    }
}