using EFCORE.Domain.Abstract;
using EFCORE.Domain.Entities;

namespace EFCORE.Domain.Repositories;

public interface IProjectEmployeeRepository : IBaseRepository<ProjectEmployee, Guid>
{
}
