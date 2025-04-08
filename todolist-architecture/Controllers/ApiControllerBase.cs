using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Todo.Application.Commons.Models;

namespace todolist_architecture.Controllers;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    protected IActionResult Success(string message, string details, object data)
    {
        return Ok(new Result<object>
        (
            data,
            message,
            details
        ));
    }
    protected IActionResult Success(string message, string details)
    {
        return Ok(new Result
        (
            message,
            details,
            200,
            true
        ));
    }
    protected void ValidationModel()
    {
        var error = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .Select(x => x.Value?.Errors?.FirstOrDefault()?.ErrorMessage)
                .FirstOrDefault();
        if(!string.IsNullOrEmpty(error))
            throw new ValidationException(error);
    }
}
