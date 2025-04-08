using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Noding;
using Rookies.API.Filters.Attributes;
using Rookies.Contract.Exceptions;
using Rookies.Contract.Shared;
using System.Net;

namespace Rookies.API.Presentation.Controllers;

[ApiController]
[ValidateModel]
public abstract class ApiBaseController : ControllerBase
{

    protected IActionResult GetResponse<T>(T data, 
                                        string detail)
    {
        var response = new Result<T>(data, HttpStatusCode.OK.ToString(), 200, detail);
        return Ok(response);
    }
    protected IActionResult GetResponse(string detail)
    {
        var response = new Result(HttpStatusCode.OK.ToString(), 200, detail, true);
        return Ok(response);
    }
}
