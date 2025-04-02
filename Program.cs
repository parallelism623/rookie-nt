using Microsoft.EntityFrameworkCore;
using mvc_todolist;
using mvc_todolist.Models.DbContexts;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using StackExchange.Redis; 

try
{
    var builder = WebApplication.CreateBuilder(args);
    var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Loggings");
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
            new JsonFormatter(),
            Path.Combine(logDirectory, $"/log_{DateTime.Now:yyyyMMdd}.json"),
            rollingInterval: RollingInterval.Day
        )
        .Enrich.FromLogContext()
        .CreateLogger();



    builder.Services.AddSession(options =>
    {
        options.Cookie.Name = "mvc-list-rookies-app";
        options.IdleTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.IOTimeout = TimeSpan.FromSeconds(10);
    });
    builder.Services.AddControllersWithViews();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddApplicationService();
    builder.Services.AddExceptionHandler<mvc_todolist.Middlewares.ExceptionHandlerMiddleware>();
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseInMemoryDatabase(databaseName: "RookieDb");
    });
    var app = builder.Build();
    app.UseExceptionHandler(_ => { });
    if (app.Environment.IsDevelopment())
    {
        using (var scoped = app.Services.CreateScope())
        {
            var dbContext = scoped.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.AddDataSeeder().GetAwaiter();
        }
    }
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    
    app.UseHttpsRedirection();

   
    app.UseRouting();
    
    app.UseAuthorization();
    app.UseSession();

    app.MapStaticAssets();
    app.MapControllerRoute(
        name: "nt_rookie",
        pattern: "NashTech/{controller=Rookies}/{action=Index}/{id?}",
        constraints: new
            {
                controller = "Rookies"
            }
        );
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();


    app.Run();
}
catch (Exception ex)
{
    Log.Error(ex.Message);
}
finally
{
    Log.Information("Stopping app...");
    Log.CloseAndFlush();
}