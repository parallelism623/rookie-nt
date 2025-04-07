using EFCore.BulkExtensions;
using Rookies.Domain;
using Rookies.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Persistence.Repositories;
public abstract class BaseRepository<T, TKey> : IBaseRepository<T, TKey>
    where TKey : notnull
    where T : class, IEntity<TKey>
{
    private readonly RookiesDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    protected BaseRepository(RookiesDbContext context)
    {
        _context = context;
    }
    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public async Task BulkAddAsync(IEnumerable<T> entities)
    {
       await _context.BulkInsertAsync(entities);  
    }

    public async Task BulkDeleteAsync(IEnumerable<T> entities)
    {
        await _context.BulkDeleteAsync(entities);
    }

    public async Task BulkUpdateAsync(IEnumerable<T> entities)
    {
        await _context.BulkUpdateAsync(entities);
    }

    public void Delete(T entity)
    {
        _context.Remove(entity);
    }

    public async Task<T?> GetByIdAsync(TKey id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(t => t.Id.Equals(id));
    }

    public void Update(T entity)
    {
        _context.Update(entity);
    }
}
