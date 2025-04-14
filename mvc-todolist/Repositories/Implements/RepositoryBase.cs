using Microsoft.EntityFrameworkCore;
using mvc_todolist.Models.DbContexts;
using mvc_todolist.Models.Entities;
using mvc_todolist.Repositories.Interfaces;
using System.Linq.Expressions;

namespace mvc_todolist.Repositories.Implements;

public abstract class RepositoryBase<T> : IRepositoryBase<T>, IDisposable
    where T : class, IEntity
{
    internal readonly AppDbContext _dbContext;
    internal readonly DbSet<T> _dbSet;
    public RepositoryBase(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();    
    }
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }
    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter= default!, 
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = default!,
            string includeProperties = default!)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        if(!string.IsNullOrEmpty(includeProperties))
        {
            foreach(var it in includeProperties.Split(','))
            {
                query.Include(it);
            }
        }

        if(orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }
    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Where(o => o.Id == id).FirstOrDefaultAsync();
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    private bool _disposed = false;
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    ~RepositoryBase()
    {
        Dispose(false);
    }
}