using EFCORE.Domain.Entities;

namespace EFCORE.Domain.Repositories;

public interface IProjectEmployeeRepository : IBaseRepository<ProjectEmployee, Guid>
{
    Task<bool> IsExistsAsync(Guid employeeId, Guid projectId);
}
