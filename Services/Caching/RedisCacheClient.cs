
using Microsoft.Extensions.Options;
using mvc_todolist.Commons.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace mvc_todolist.Services.Caching
{
    public class RedisCacheClient : CacheClientBase
    {

        private readonly ILogger<RedisCacheClient> _logger;
        private readonly RedisClientOptions _redisClientOptions;
        private ConnectionMultiplexer? _connection;
        public RedisCacheClient(IOptions<RedisClientOptions> redisClientOptions, ILogger<RedisCacheClient> logger) : base(logger)
        {
            _logger = logger;
            _redisClientOptions = redisClientOptions.Value;
        }
        ConnectionMultiplexer GetConnection()
        {
            if (_connection == null)
            {
                if (_redisClientOptions.IsEssentialMode == true)
                {

                }
                else
                {
                    _connection = ConnectionMultiplexer.Connect(new ConfigurationOptions
                    {
                        EndPoints = { "localhost:6379" },
                        Password = "mvc-rookies-list",
                        ConnectTimeout = 10000,
                        SyncTimeout = 10000,
                        DefaultDatabase = 0,
                        AbortOnConnectFail = false,
                        ConnectRetry = 3
                    });
                }
            }
            return _connection ?? default!;
        }
        IDatabase GetDatabase() => GetConnection().GetDatabase();
        IServer GetServer() => GetConnection()?.GetServers()?.LastOrDefault() ?? default!;
        public override T GetOrDefault<T>(string key)
        {
            var database = GetDatabase();
            var result = database.StringGet(new RedisKey(key));
            return DeserializeString<T>(result);
        }
        public override void Set<T>(string key, T value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            var database = GetDatabase();
            string valueString = JsonSerializer.Serialize(value);
            database.StringSet(new RedisKey(key), new RedisValue(valueString), absoluteExpireTime, true);
        }
        public override void Remove(string key)
        {
            var database = GetDatabase();
            database.KeyDelete(new RedisKey(key));
        }
        public override async Task<T> GetOrDefaultAsync<T>(string key)
        {
            var database = GetDatabase();
            var result = await database.StringGetAsync(new RedisKey(key));
            return DeserializeString<T>(result)!;
        }

        public override async Task RemoveAsync(string key)
        {
            var database = GetDatabase();
            await database.KeyDeleteAsync(new RedisKey(key));
        }

        public override async Task RemovePatternAsync(string pattern)
        {
            var database = GetDatabase();
            var keys = GetServer()?.Keys(pattern: new RedisValue(pattern));
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    await database.KeyDeleteAsync(key);
                }
            }
        }

        public override async Task SetAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            var database = GetDatabase();
            string valueString = JsonSerializer.Serialize(value);
            await database.StringSetAsync(new RedisKey(key), new RedisValue(valueString), absoluteExpireTime, true);
        }
        public override void RemovePattern(string pattern)
        {
            var database = GetDatabase();
            var keys = GetServer()?.Keys(pattern: new RedisValue(pattern));
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    database.KeyDelete(key);
                }
            }
        }

        #region Dispose
        private bool _disposed = false;
        public override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                if (_disposed)
                {
                    if (disposing)
                    {

                    }
                    _disposed = true;
                }
            }
            base.Dispose(disposing);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~RedisCacheClient()
        {
            Dispose(false);
        }
        #endregion


    }
}
