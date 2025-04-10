using EFCORE.Domain.Entities;
using System.Linq.Expressions;

namespace EFCORE.Domain.Abstract;
public interface IBaseRepository<TEntity, in TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
{
    IQueryable<TEntity> GetAll();
    Task SaveChangesAsync();
    Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includes);
    void Add(TEntity entity);
    void Update(TEntity entity);    
    void Delete(TEntity entity);

}
