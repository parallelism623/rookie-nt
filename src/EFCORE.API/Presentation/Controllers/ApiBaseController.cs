using Asp.Versioning;
using EFCORE.API.Filters;
using EFCORE.Contract.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EFCORE.API.Presentation.Controllers;

[ApiController]
[ValidationRequestModel]
[ApiVersion(1)]
[ApiVersion(2)]
public class ApiBaseController : ControllerBase
{
    protected IActionResult OnResultSucess<T>(T result)
        where T : Result
    => result.StatusCode switch
    {
        StatusCodes.Status200OK => Ok(result),
        _ => StatusCode(result.StatusCode, result)
    };

    protected IActionResult OnResultFailure<T>(T result)
        where T : Result
    => result.StatusCode switch
    {
        StatusCodes.Status400BadRequest => BadRequest(result),
        StatusCodes.Status404NotFound => NotFound(result),
        _ => StatusCode(result.StatusCode, result)
    };

    protected IActionResult ProcessResult<T>(T result)
        where T : Result
    => result.IsSuccess ? OnResultSucess(result) : OnResultFailure(result);
}
