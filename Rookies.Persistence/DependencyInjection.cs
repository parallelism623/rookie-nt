using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rookies.Domain;
using Rookies.Domain.Repositories;
using Rookies.Persistence.Repositories;

namespace Rookies.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePersistenceLayer(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<RookiesDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IUnitOfWork, RookiesDbContext>();
        return services;
    }
}