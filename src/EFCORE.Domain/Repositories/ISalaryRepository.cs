using EFCORE.Domain.Abstract;
using EFCORE.Domain.Entities;

namespace EFCORE.Domain.Repositories;

public interface ISalaryRepository : IBaseRepository<Salary, Guid>
{
}
