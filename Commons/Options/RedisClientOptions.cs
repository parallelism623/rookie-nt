namespace mvc_todolist.Commons.Options
{
    public class RedisClientOptions
    {
        public bool IsEssentialMode { get; set; }
        public string? MasterServerName { get; set; }
        public string? ConnectionString { get; set; }
        public int? DefaultDatabase { get; set; }
    }
}
