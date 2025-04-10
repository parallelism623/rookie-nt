
namespace EFCORE.Application.UseCases.Employee;

public class EmployeeDetailResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public DateOnly JoinedDate { get; set; }
    
    public EmployeeSalaryResponse? Salary { get; set; }
    public EmployeeDepartmentResponse? Department { get; set; }
    public List<ProjectOfEmployeeResponse>? Projects { get; set; }
}

public record EmployeeSalaryResponse(decimal Amount, Guid Id);

public record EmployeeDepartmentResponse(string Name, Guid Id);

public record ProjectOfEmployeeResponse(string Name, Guid Id, bool Enable);
