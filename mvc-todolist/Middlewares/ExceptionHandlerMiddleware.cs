using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using mvc_todolist.Commons;
using mvc_todolist.Commons.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace mvc_todolist.Middlewares
{
    public class ExceptionHandlerMiddleware : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,
             ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {


            _logger.LogError(exception.Message);

            var tempData = _tempDataFactory.GetTempData(context);
            var statusCode = GetExceptionResponseStatusCode(exception);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            var message = GetExceptionResponseMessage(exception);
            var detail = exception.Message;
            var errorResponse = new Result(message, detail, statusCode);
            
            await context.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }


        private int GetExceptionResponseStatusCode(Exception exception)
        {
            return exception switch
            {
                BadRequestException => 400,
                NotFoundException => 404,
                ValidationException => 422,
                UnAuthorizedException => 401,
                InternalServerException => 500,
                _ => 500
            };
        }

        private string GetExceptionResponseMessage(Exception exception)
        {
            return exception switch
            {
                BadRequestException => "Bad request",
                NotFoundException => "Not found",
                ValidationException => "UnProcessable Entity",
                UnAuthorizedException => "UnAuthorized",
                InternalServerException => "Internal server error",
                _ => "Undefined exception"
            };
        }

    }
}
