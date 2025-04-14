namespace Rookies.Domain.Abstractions;

public interface IBaseRepository<T, TKey>
    where T : IEntity<TKey>
    where TKey : notnull
{
    IUnitOfWork UnitOfWork { get; }

    Task<T?> GetByIdAsync(TKey id);

    void Add(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task BulkAddAsync(IEnumerable<T> entities);

    Task BulkUpdateAsync(IEnumerable<T> entities);

    Task BulkDeleteAsync(IEnumerable<T> entities);
}