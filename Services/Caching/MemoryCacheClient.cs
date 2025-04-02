
using Microsoft.Extensions.Caching.Memory;

namespace mvc_todolist.Services.Caching
{
    public class MemoryCacheClient : CacheClientBase
    {
        private readonly IMemoryCache _cache;
        ILogger<CacheClientBase> _logger;
        public MemoryCacheClient(ILogger<CacheClientBase> logger, IMemoryCache cache) : base(logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public override T GetOrDefault<T>(string key)
        {
            var valueCache = _cache.Get(key)?.ToString();
            if(string.IsNullOrEmpty(valueCache))
            {
                return default!;
            }
            return DeserializeString<T>(valueCache);
        }

        public async override Task<T> GetOrDefaultAsync<T>(string key)
        {
            var valueCache = _cache.Get(key)?.ToString();
            if (string.IsNullOrEmpty(valueCache))
            {
                return default!;
            }
            return DeserializeString<T>(valueCache);
        }

        public override void Remove(string key)
        {
            _cache.Remove(key);
        }

        public override async Task RemoveAsync(string key)
        {
            _cache.Remove(key);
        }

        public override void RemovePattern(string pattern)
        {
            throw new Exception("Cannot support remove pattern key in mem cache");
        }

        public override Task RemovePatternAsync(string pattern)
        {
            throw new Exception("Cannot support remove pattern key in mem cache");
        }

        public override void Set<T>(string key, T value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            var seconds = 300;
            _cache.Set(key, SerializeString<T>(value), absoluteExpiration: DateTime.Now.AddSeconds(seconds));
        }

        public override Task SetAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            throw new Exception("Cannot support set cache async key in mem cache");
        }
    }
}
