using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFCORE.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<Salary> Salaries { get; set; } = default!;
    public DbSet<ProjectEmployee> ProjectEmployees { get; set; } = default!;
    public DbSet<Project> Projects { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}