using mvc_todolist.Models.Entities;
using System.Linq.Expressions;

namespace mvc_todolist.Repositories.Interfaces;

public interface IRepositoryBase<T>
    where T : class, IEntity
{
    Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = default!,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = default!,
            string includeProperties = default!);
    public Task<IEnumerable<T>> GetAll();
    public Task<T> GetByIdAsync(Guid id);
    public void Add(T entity);
    public void Remove(T entity);
    public void Update(T entity);
}