using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;
using EFCORE.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Persistence.Repositories;

public class EmployeeRepository : BaseRepository<Employee, Guid>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<List<Employee>?> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        return _context.Employees?.AsNoTracking().Where(e => ids.Contains(e.Id))?.ToListAsync()!;
    }

    public IQueryable<Employee> GetByJoinedDateAndSalary()
    {
        return ApplySpecification(new EmployeesByJoinedDateAndSalarySpecification());
    }

    public Task<Employee?> GetEmployeeDetailByIdAsync(Guid id)
    {
        return _context.Employees?
            .AsNoTracking()
            .Where(e => e.Id == id)?
            .Include(e => e.Department)
            .Include(e => e.Salary)
            .Include(e => e.ProjectEmployees)
            .ThenInclude(pe => pe.Project)
            .AsSplitQuery()
            .FirstOrDefaultAsync()!;
    }

    private IQueryable<Employee> ApplySpecification(
        Specification<Employee, Guid> specification)
    {
        return SpecificationsExecutor.GetQuery(
            GetAll(),
            specification);
    }
}
