using aspnetcore.Commons.Encrypts;
using aspnetcore.Models;
using aspnetcore.Repositories.Implements;
using aspnetcore.Repositories.Interfaces;
using aspnetcore.Services.Implements;
using aspnetcore.Services.Interfaces;

namespace aspnetcore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
        {
            return services.AddRepositoriesDependencyInjection()
                           .AddServicesDependencyInjection()
                           .AddDbContextDependencyInjection();
        }

        public static IServiceCollection AddRepositoriesDependencyInjection(this IServiceCollection services)
        {
            return services.AddScoped<IAuthRepository, AuthRepository>()
                           .AddScoped<IEncryptAlgorithm, RSAEncryptAlgorithm>();
        }

        public static IServiceCollection AddServicesDependencyInjection(this IServiceCollection services)
        {
            return services.AddScoped<IAuthServices, AuthService>();
        }
        public static IServiceCollection AddDbContextDependencyInjection(this IServiceCollection services)
        {
            return services.AddSingleton<MemoryDbContext>();
        }
    }
}
