using EFCORE.Application.UseCases.Employee;
using EFCORE.Domain.Abstract;
using EFCORE.Domain.Entities;

namespace EFCORE.Domain.Repositories;

public interface IEmployeeRepository : IBaseRepository<Employee, Guid>
{
    Task<List<Employee>?> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<Employee?> GetEmployeeDetailByIdAsync(Guid id);

    IQueryable<Employee> GetByJoinedDateAndSalary();

}
