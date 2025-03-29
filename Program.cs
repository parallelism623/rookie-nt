using aspnetcore;
using aspnetcore.Middlewares;
using Microsoft.AspNetCore.Localization;
using Serilog;
using Serilog.Events;
using System.Globalization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    #region SERILOG_CONFIG
    var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logging");
    if (!Directory.Exists(logDirectory))
    {
        Directory.CreateDirectory(logDirectory);
    }
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        // .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        // .MinimumLevel.Override("System", LogEventLevel.Warning) 
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .WriteTo.File(
            Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd}.txt"),
            rollingInterval: RollingInterval.Day,
            outputTemplate: "[{Level}] [{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] : {Message} {NewLine} {Exception}"
        )
        .Enrich.FromLogContext()
        .CreateLogger();
    #endregion SERILOG_CONFIG

    Log.Information("Ứng dụng đang khởi động...");


    builder.Host.UseSerilog();
    builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
    builder.Services.AddApplicationDependencyInjection();
    builder.Services.AddControllers();
    builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();

    var app = builder.Build();

    app.UseRequestLocalization();

    app.UseMiddleware<LoggingMiddleware>();
    app.UseExceptionHandler(_ => { });
    app.UseRouting();
    app.MapControllers();
    app.Run();

}
catch(Exception ex)
{
    Log.Error(ex.Message);
}
finally
{
    Log.Information("Stopping app...");
    Log.CloseAndFlush();
}