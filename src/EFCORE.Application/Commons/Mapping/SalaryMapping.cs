
using EFCORE.Application.UseCases.Salary;
using EFCORE.Domain.Entities;

namespace EFCORE.Application.Commons.Mapping;

public static class SalaryMapping
{
    public static SalaryResponse ToSalaryResponse(this Salary salary)
    {
        return new()
        {
            Id = salary.Id,
            EmployeeId = salary.EmployeeId,
            Amount = salary.Amount,
            EmployeeName = salary.Employee.Name,
        };
    }
}
