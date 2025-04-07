using Microsoft.AspNetCore.Mvc;
using Rookies.Contract.Shared;
using System.Net;

namespace Rookies.API.Presentation;

public static class ApiBehaviorConfigure
{
    public static IServiceCollection ConfigureApiController(this IServiceCollection services)
    {
        return services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                var result = new Result
                {
                    Success = false,
                    Message = "Validation failed",
                    Detail = errors.FirstOrDefault(),
                    StatusCode = (int)HttpStatusCode.UnprocessableContent
                };
                return new BadRequestObjectResult(result);
            };
        });

    }
}
