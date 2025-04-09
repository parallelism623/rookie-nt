using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCORE.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePersistenceLayer(this IServiceCollection services)
    {
        return services.ConfigureDbContext();
    }

    public static IServiceCollection ConfigureDbContext(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnectionString"))
        );
        return services;
    }
}