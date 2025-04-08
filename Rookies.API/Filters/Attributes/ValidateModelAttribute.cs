using Microsoft.AspNetCore.Mvc.Filters;
using Rookies.Contract.Exceptions;

namespace Rookies.API.Filters.Attributes;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var error = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .Select(x => x.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                    .FirstOrDefault();
            if (!string.IsNullOrEmpty(error))
                throw new InvalidModelRequestException(error);
        }
    }
}
