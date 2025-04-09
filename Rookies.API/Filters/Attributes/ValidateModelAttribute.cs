using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Rookies.Contract.Exceptions;
using Rookies.Contract.Shared;
using System.Net;
using System.Threading;

namespace Rookies.API.Filters.Attributes;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x => new Error(x.Key, x.Value?.Errors?.FirstOrDefault()?.ErrorMessage))
                    .ToList();
            var errorResponse = new Result { StatusCode=400, IsSuccess=false};
            if (errors.Any())
            {
                errorResponse.Errors = errors;
            }
            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            return;
        }
    }
}
