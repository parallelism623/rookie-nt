
using EFCORE.Application.UseCases.Employee;
using EFCORE.Domain.Entities;

namespace EFCORE.Persistence.Specifications;

public class EmployeesByJoinedDateAndSalarySpecification : Specification<Employee, Guid>
{
    public EmployeesByJoinedDateAndSalarySpecification(EmployeeQueryParamerters employeeQueryParamerters) 
        : base(e => e.JoinedDate >= employeeQueryParamerters.JoinedFromDate && e.Salary.Amount > employeeQueryParamerters.MinSalary)
    {
        AddInclude(e => e.Salary);
        AddInclude(e => e.Department);

    }
}
