using aspnetcore.Models;

namespace aspnetcore.Repositories.Interfaces
{
    public interface IRepositoryBase<T> where T : class, IEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task Add(T entity);
        Task<IEnumerable<T>> GetAll();
        Task Delete(T entity);
        Task Update(T entity);
    }
}
