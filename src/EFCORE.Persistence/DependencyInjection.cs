using EFCORE.Application.UseCases.Department;
using EFCORE.Application.UseCases.Employee;
using EFCORE.Application.UseCases.Project;
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Application.UseCases.Salary;
using EFCORE.Domain.Abstract;
using EFCORE.Domain.Repositories;
using EFCORE.Persistence.Repositories;
using EFCORE.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EFCORE.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePersistenceLayer(this IServiceCollection services)
    {
        return services.ConfigureDbContext()
                       .ConfigureUseCaseServices()
                       .ConfigureRepositories();
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services)
    {
        var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
        services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnectionString"))
        );
        services.AddScoped<ITransactionManager, TransactionManager>();
        return services;
    }
    
    private static IServiceCollection ConfigureUseCaseServices(this IServiceCollection services)
    {
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ISalaryService, SalaryService>();
        services.AddScoped<IProjectEmployeeSerivce, ProjectEmployeeSerivce>();    
        return services;
    }

    private static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        return services.AddScoped<IDepartmentRepository, DepartmentRepository>()
                       .AddScoped<IProjectRepository, ProjectRepository>()
                       .AddScoped<IEmployeeRepository, EmployeeRepository>()
                       .AddScoped<ISalaryRepository, SalaryRepository>()
                       .AddScoped<IProjectEmployeeRepository, ProjectEmployeeRepository>();

    }
}