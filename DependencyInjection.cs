using aspnetcore.Commons.Encrypts;
using aspnetcore.Models;
using aspnetcore.Repositories.Implements;
using aspnetcore.Repositories.Interfaces;
using aspnetcore.Services.Implements;
using aspnetcore.Services.Interfaces;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace aspnetcore
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
        {
            return services.AddRepositoriesDependencyInjection()
                           .AddServicesDependencyInjection()
                           .AddDbContextDependencyInjection()
                           .AddConfigResourceLocalization();
        }

        public static IServiceCollection AddRepositoriesDependencyInjection(this IServiceCollection services)
        {
            return services.AddScoped<IAuthRepository, AuthRepository>()
                           .AddScoped<IEncryptAlgorithm, RSAEncryptAlgorithm>();
                           //.AddScoped<ILabels, Labels>(); 
        }

        public static IServiceCollection AddServicesDependencyInjection(this IServiceCollection services)
        {
            return services.AddScoped<IAuthServices, AuthService>();
        }
        public static IServiceCollection AddDbContextDependencyInjection(this IServiceCollection services)
        {
            return services.AddSingleton<MemoryDbContext>();
        }

        public static IServiceCollection AddConfigResourceLocalization(this IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("vi")
                };

                options.DefaultRequestCulture = new RequestCulture("vi");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider{ QueryStringKey = "locale", UIQueryStringKey="locale"}
                };
            });
            return services;
        }
    }
}
