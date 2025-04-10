
namespace EFCORE.Application.UseCases.Employee;

public class EmployeeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid DepartmentId { get; set; }
    public DateOnly JoinedDate { get; set; }
    public decimal Amount { get; set; }
}

