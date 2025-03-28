using aspnetcore.Commons.Encrypts;
using aspnetcore.Commons.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

namespace aspnetcore.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        
        public LoggingMiddleware(RequestDelegate next, 
                                 ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await LogBodyRequest(context);
            await LogBodyResponse(context);
        }

        private async Task LogBodyRequest(HttpContext context)
        {
            var request = context.Request;
            request.EnableBuffering();
            var requestBody = await request.Body.ReadAsStringAsync(true);
            _logger.LogInformation("Begined Request: {Method} {Path} {QueryString} | Request: {Request}",
            GetMethod(context), GetPath(context), GetQueryString(context), requestBody);
        }

        private async Task LogBodyResponse(HttpContext context)
        {
            using var memoryStream = new MemoryStream();
            var originalBody = context.Response.Body;
            try
            {
                context.Response.Body = memoryStream;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                await _next(context);

                stopwatch.Stop();
                memoryStream.Position = 0;
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                _logger.LogInformation("Finished Request: {Method} {Path} {QueryString} | Response: {Response} | Time: {ElapsedMs} ms",
                                        GetMethod(context), GetPath(context), 
                                        GetQueryString(context),
                                        responseBody
                                        , stopwatch.ElapsedMilliseconds);
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBody);
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private string GetPath(HttpContext context)
        {
            return context.Request.Path;
        }
        private string GetMethod(HttpContext context)
        {
            return context.Request.Method;
        }
        private string GetQueryString(HttpContext context)
        {
            return context.Request.QueryString.ToString();
        }

  
    }
}
