using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
namespace Rookies.Contract;
public static class DependencyInjection
{
    public static IServiceCollection ApplicationValidationConfig(this IServiceCollection services)
    {   
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        return services.AddFluentValidationAutoValidation();
    }
}
