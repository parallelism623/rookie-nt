using aspnetcore;
using aspnetcore.Middlewares;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

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
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("System", LogEventLevel.Warning) 
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .WriteTo.File(
            new CompactJsonFormatter(),
            Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd}.json"),
            rollingInterval: RollingInterval.Day
        )
        .Enrich.FromLogContext()
        .CreateLogger();
    #endregion SERILOG_CONFIG

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