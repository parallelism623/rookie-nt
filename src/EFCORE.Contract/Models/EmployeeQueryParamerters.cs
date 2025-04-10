
using EFCORE.Contract.Shared;

namespace EFCORE.Application.UseCases.Employee;

public class EmployeeQueryParamerters : QueryParameters
{
    public string? Includes { get; set; }

    public DateOnly? JoinedFromDate { get; set; }

    public decimal? MinSalary { get; set; }

}
