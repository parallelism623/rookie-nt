using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;

namespace EFCORE.Domain.Abstract;
public interface IDepartmentRepository : IBaseRepository<Department, Guid>
{
}
