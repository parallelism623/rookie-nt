
namespace EFCORE.Application.UseCases.Salary;

public class SalaryResponse
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public string EmployeeName { get; set; } = default!;
}
