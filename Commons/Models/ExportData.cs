namespace mvc_todolist.Commons.Models
{
    public class ExportData<T>
    {
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public IEnumerable<T> Data { get; set; }

        public byte[] DataBytes { get; set; }

    }
}
