namespace mvc_todolist.Services.Caching
{
    public interface ICacheService : IDisposable
    {
        public T Get<T>(string key);
        public T Get<T>(string key, Func<string, T> factory);
        public Task<T> GetAsync<T>(string key);
        public Task<T> GetAsync<T>(string key, Func<string, Task<T>> factory);
        public T GetOrDefault<T>(string key);
        public Task<T> GetOrDefaultAsync<T>(string key);
        public IEnumerable<T> GetMany<T>(string[] keys);
        public Task<IEnumerable<T>> GetManyAsync<T>(string[] keys);
        public void Set<T>(string key, T value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        public void Set<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        public Task SetAsync<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        public void Remove(string key);
        public Task RemoveAsync(string key);
        public Task RemovePatternAsync(string pattern);
        public void RemovePattern(string pattern);
    }
}
