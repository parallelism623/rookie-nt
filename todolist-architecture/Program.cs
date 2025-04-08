
using Microsoft.AspNetCore.Mvc;
using Todo.Persistence;
using Todo.Application;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<todolist_architecture.Middlewares.ExceptionHandlerMiddleware>();
builder.Services.AddInfrastructureServices();
builder.Services.ApplicationValidationConfig();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
var app = builder.Build();
app.UseExceptionHandler(_ => { });

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();

app.MapControllers();


app.Run();


