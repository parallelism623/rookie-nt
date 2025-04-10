using EFCORE.Domain.Abstract;
using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.Data;
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

    public Task SaveChangesEntitiesAsync()
    {
        var entries = ChangeTracker.Entries()
    .                           Where(e => e.Entity is AuditableEntity
                                    && (e.State == EntityState.Added 
                                    || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var auditableEntity = (AuditableEntity)entry.Entity;
            var now = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                auditableEntity.CreatedAt = now;
                auditableEntity.CreatedBy = Guid.NewGuid();
            }

            if(entry.State == EntityState.Modified)
            {
                auditableEntity.ModifiedAt = now;
                auditableEntity.ModifiedBy = Guid.NewGuid();
            }   
        }
        return base.SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    private IDbContextTransaction? _transaction;

    public IDbContextTransaction? GetCurrentTransaction() => _transaction;

    private bool HasActiveTransaction => _transaction != null;
    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_transaction != null)
        {
            return null;
        }
        _transaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        return _transaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (_transaction == null)
        {
            throw new Exception("");
        }
        if (_transaction != transaction)
        {
            throw new Exception("");
        }
        try
        {
            await _transaction.CommitAsync();
        }
        catch
        {
            Rollback();
            throw;
        }
        finally
        {
            if(HasActiveTransaction)
            {
                _transaction.Dispose();
                _transaction = null;
            }    
        }
    }

    public void Rollback()
    {
        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _transaction!.Dispose();
                _transaction = null;
            }
        }
    }
}