using EFCORE.Domain.Entities;

namespace EFCORE.Domain.Repositories;

public interface IProjectRepository : IBaseRepository<Project, Guid>
{
    Task<List<Project>?> GetByIdsAsync(List<Guid> ids);
}
