using Microsoft.AspNetCore.Mvc;
using Rookies.Contract.Shared;
using System.Net;

namespace Rookies.API.Presentation.Controllers;

public static class ApiBehaviorConfigure
{
    public static IServiceCollection ConfigureApiController(this IServiceCollection services)
    {
        return services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

    }
}
