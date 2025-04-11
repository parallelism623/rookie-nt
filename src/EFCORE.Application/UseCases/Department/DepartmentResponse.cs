
using EFCORE.Application.UseCases.Employee;

namespace EFCORE.Application.UseCases.Department;

public class DepartmentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public List<EmployeeResponse>? Employees { get; set; } = new();
}
