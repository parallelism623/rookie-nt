using EFCORE.API;
using EFCORE.API.Middlewares;
using EFCORE.Application;
using EFCORE.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.ConfigurePersistenceLayer()
                .ConfigureApplicationLayer()
                .ConfigureApiVersioning();
builder.Services.AddControllers();
var app = builder.Build();

app.UseExceptionHandler(_ => { });
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
await app.RunAsync();