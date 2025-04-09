using EFCORE.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.ConfigurePersistenceLayer();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using (var dbContext = app.Services.GetRequiredService<ApplicationDbContext>())
    {
        dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
    }
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.RunAsync();