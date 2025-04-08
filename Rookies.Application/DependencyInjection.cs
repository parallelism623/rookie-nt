using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Rookies.Application;
public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplicationLayer(this IServiceCollection services)
    {
        ConfigureMapster();
        return services.ConfigureMediator();
    }
    public static IServiceCollection ConfigureMediator(this IServiceCollection services)
    {
        return services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
    }

    public static void ConfigureMapster()
    {
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
