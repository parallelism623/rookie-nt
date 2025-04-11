using EFCORE.Domain.Abstract;
using EFCORE.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EFCORE.Persistence.Repositories;

public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    protected readonly ApplicationDbContext _context;
    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void Add(TEntity entity)
    {
        _context.Add(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Remove(entity);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().AsNoTracking();
    }

    public Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _context.Set<TEntity>().Where(t => t.Id.Equals(id));
        foreach(var include in includes)
        {
            query = query.Include(include);
        }

        return query.FirstOrDefaultAsync();
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesEntitiesAsync();
    }

    public void Update(TEntity entity)
    {
        _context.Update(entity);
    }
}
