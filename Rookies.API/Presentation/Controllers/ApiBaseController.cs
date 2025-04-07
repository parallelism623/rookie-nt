using Microsoft.AspNetCore.Mvc;
using Rookies.Contract.Exceptions;

namespace Rookies.API.Presentation.Controllers;

[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected void ValidationModel()
    {
        var error = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .Select(x => x.Value?.Errors.First().ErrorMessage)
                .FirstOrDefault();
        if (!string.IsNullOrEmpty(error))
            throw new InvalidModelRequestException(error);
    }


}
