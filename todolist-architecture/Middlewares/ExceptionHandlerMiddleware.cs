using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using Todo.Application.Commons.Exceptions;
using Todo.Application.Commons.Models;

namespace todolist_architecture.Middlewares;

public class ExceptionHandlerMiddleware : IExceptionHandler
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;


    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {


        _logger.LogError(exception.Message);


        var statusCode = GetExceptionResponseStatusCode(exception);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var message = GetExceptionResponseMessage(exception) ?? "";
        var detail = exception.Message;
        var errorResponse = new Result(message, detail, statusCode, false);

        await context.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }


    private static int GetExceptionResponseStatusCode(Exception exception)
    {
        return exception switch
        {
            BadRequestException => 400,
            NotFoundException => 404,
            ValidationException => 400,
            UnAuthorizedException => 401,
            InternalServerErrorException => 500,
            _ => 400
        };
    }

    private static string GetExceptionResponseMessage(Exception exception)
    {
        return exception switch
        {
            BadRequestException => "Bad request",
            NotFoundException => "Not found",
            ValidationException => "UnProcessable Entity",
            UnAuthorizedException => "UnAuthorized",
            InternalServerErrorException => "Internal server error",
            _ => "Bad request"
        };
    }

}
