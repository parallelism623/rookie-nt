using mvc_todolist.Services.Caching;


public static class ConfigClientCacheExtensions
{
    public static IServiceCollection AddRedisCacheService(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, RedisCacheClient>();
        return services;
    }
    public static IServiceCollection AddMemoryCacheService(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, MemoryCacheClient>();
        return services;
    }
}