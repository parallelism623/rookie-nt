using Rookies.API;
using Rookies.API.Middlewares;
using Rookies.API.Presentation;
using Rookies.Application;
using Rookies.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
try
{
    Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
    builder.Host.UseSerilog();
    builder.Services.AddOpenApi();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddPersistenceService(builder.Configuration)
                    .ConfigureApiController()
                    .ConfigureMediator();
    builder.Services.AddControllers();
    builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();
    var app = builder.Build();

    app.UseExceptionHandler(_ => { });
    if (app.Environment.IsDevelopment())
    {
        await app.InitializeDatabaseAsync();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();
    await app.RunAsync();
}
finally
{
    Console.WriteLine("Application has stopped.");
}

