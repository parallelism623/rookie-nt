
using EFCORE.Domain.Entities;
using System.Linq.Expressions;

namespace EFCORE.Persistence.Specifications;

public class EmployeesByJoinedDateAndSalarySpecification : Specification<Employee, Guid>
{
    public EmployeesByJoinedDateAndSalarySpecification() 
        : base(e => e.JoinedDate >= new DateOnly(2024, 1, 1) && e.Salary.Amount > 100)
    {
    }
}
