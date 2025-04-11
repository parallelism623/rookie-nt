using EFCORE.Application.UseCases.Employee;
using EFCORE.Domain.Entities;
using System.Data.Common;

namespace EFCORE.Domain.Repositories;

public interface IEmployeeRepository : IBaseRepository<Employee, Guid>
{
    Task<List<Employee>?> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<Employee?> GetEmployeeDetailByIdAsync(Guid id);
    DbConnection GetDbConnection();

}
