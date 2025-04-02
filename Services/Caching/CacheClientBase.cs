using StackExchange.Redis;
using System.Text.Json;

namespace mvc_todolist.Services.Caching;
public abstract class CacheClientBase : ICacheService
{
    private readonly Lock _lock = new Lock();
    private readonly SemaphoreSlim _lockAsync = new SemaphoreSlim(1, 1);

    private readonly ILogger<CacheClientBase> _logger;
    public CacheClientBase(ILogger<CacheClientBase> logger)
    {
        _logger = logger;
    }
    public virtual T Get<T>(string key)
    {
        var item = default(T);
        try
        {
            item = GetOrDefault<T>(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return item;
    }
    public virtual T Get<T>(string key, Func<string, T> factory)
    {
        var item = default(T);
        try
        {
            item = GetOrDefault<T>(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        if (item == null)
        {
            lock (_lock)
            {
                try
                {
                    item = GetOrDefault<T>(key);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                if (item == null)
                {
                    item = factory(key);
                    try
                    {
                        Set(key, item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                    }
                }
            }
        }
        return item;
    }

    public virtual async Task<T> GetAsync<T>(string key, Func<string, Task<T>> factory)
    {
        var item = default(T);
        try
        {
            item = await GetOrDefaultAsync<T>(key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        if (item == null)
        {
            await _lockAsync.WaitAsync();
            try
            {
                item = await GetOrDefaultAsync<T>(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            if (item == null)
            {
                item = await factory(key);
                try
                {
                    await SetAsync(key, item);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
            _lockAsync.Release();
        }
        return item;
    }


    public virtual async Task<T> GetAsync<T>(string key)
    {
        var item = default(T);
        try
        {
            item = await GetOrDefaultAsync<T>(key);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, ex.Message);
        }
        return item!;
    }

    public virtual IEnumerable<T> GetMany<T>(string[] keys)
    {
        var items = new List<T>();
        try
        {
            foreach (var key in keys)
            {
                items.Add(GetOrDefault<T>(key));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return items;
    }

    public virtual async Task<IEnumerable<T>> GetManyAsync<T>(string[] keys)
    {
        var items = new List<T>();
        try
        {
            foreach (var key in keys)
            {
                items.Add(await GetOrDefaultAsync<T>(key));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return items;
    }

    public abstract T GetOrDefault<T>(string key);

    public abstract Task<T> GetOrDefaultAsync<T>(string key);

    public abstract void Remove(string key);

    public abstract Task RemoveAsync(string key);

    public abstract void RemovePattern(string pattern);

    public abstract Task RemovePatternAsync(string pattern);
    public abstract Task SetAsync<T>(string key, T value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
    public abstract void Set<T>(string key, T value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

    public void Set<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
    {
        foreach (var (key, value) in keyValuePairs)
        {
            Set(key, value, slidingExpireTime, absoluteExpireTime);
        }
    }


    public async Task SetAsync<T>(IEnumerable<KeyValuePair<string, T>> keyValuePairs, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
    {
        foreach (var (key, value) in keyValuePairs)
        {
            await SetAsync(key, value, slidingExpireTime, absoluteExpireTime);
        }
    }
    protected T DeserializeString<T>(string value)
    {
        if (value != null && !string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value)!;
        }
        return default!;
    }
    protected T DeserializeString<T>(RedisValue redisValue)
    {
        if (redisValue.HasValue && !redisValue.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<T>(redisValue.ToString())!;
        }
        return default!;
    }
    protected string SerializeString<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }
    #region Dispose pattern
    private bool _disposed = false;
    public virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            if (disposing)
            {
                // Dispose services that are managed by GC collection 
            }
            // Dispose services that are not managed by GC collection 
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~CacheClientBase()
    {
        Dispose(false);
    }
    #endregion Dispose pattern

}


