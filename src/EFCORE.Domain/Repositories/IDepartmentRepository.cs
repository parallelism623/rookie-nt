using EFCORE.Domain.Entities;

namespace EFCORE.Domain.Abstract;
public interface IDepartmentRepository : IBaseRepository<Department, Guid>
{
}
