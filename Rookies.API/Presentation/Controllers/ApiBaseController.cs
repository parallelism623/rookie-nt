using Microsoft.AspNetCore.Mvc;
using Rookies.API.Filters.Attributes;
using Rookies.Contract.Shared;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rookies.API.Presentation.Controllers;

[ApiController]
[ValidateModel]
public abstract class ApiBaseController : ControllerBase
{
    protected IActionResult OnResultSuccess<T>(T result)
        where T : Result
    {
        return result.StatusCode switch
        {
            StatusCodes.Status200OK => Ok(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result)
        };
    }
    protected IActionResult OnResultFailure<T>(T result)
        where T : Result
    {
        return result.StatusCode switch
        {
            StatusCodes.Status400BadRequest => BadRequest(result),
            StatusCodes.Status404NotFound => NotFound(result),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result)
        };
    }

    protected IActionResult GetResponse<T>(T result)
        where T : Result
    {
        return result.IsSuccess ? OnResultSuccess(result) : OnResultFailure(result);
    }
}
