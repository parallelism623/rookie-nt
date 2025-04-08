using Microsoft.Extensions.DependencyInjection;
using Todo.Application.Services;
using Todo.Persistence.Services;

namespace Todo.Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddSingleton<ITodoItemService, TodoItemService>();
    }
}
