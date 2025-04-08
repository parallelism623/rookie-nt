using Microsoft.Extensions.DependencyInjection;
using Rookies.Application.Services.Crypto;
using Rookies.Infrastructure.Options;
using Rookies.Infrastructure.Services.Crypto;

namespace Rookies.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services)
    {
        services.AddCryptoServiceStrategy();
        services.AddCryptoService<RsaCryptoService, RsaCryptoOptions>(CryptoAlgorithm.RSA);
        return services;
    }

    public static IServiceCollection AddCryptoServiceStrategy(this IServiceCollection services)
    {
        return services.AddScoped<ICryptoServiceStrategy, CryptoServiceStrategy>();
    }

}
