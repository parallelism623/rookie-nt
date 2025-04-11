
using EFCORE.Contract.Shared;

namespace EFCORE.Application.UseCases.Employee;

public class EmployeeQueryParamerters : QueryParameters
{

    public DateOnly? JoinedFromDate { get; set; } = default;

    public decimal? MinSalary { get; set; } = default;

}
