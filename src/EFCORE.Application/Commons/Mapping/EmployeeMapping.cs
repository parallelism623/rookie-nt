
using EFCORE.Application.UseCases.Employee;
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Domain.Entities;

namespace EFCORE.Application.Commons.Mapping;

public static class EmployeeMapping
{
    public static Employee ToEmployee(this EmployeeCreateRequest employeeCreateRequest)
    {
        return new()
        {
            Name = employeeCreateRequest.Name,
            DepartmentId = employeeCreateRequest.DepartmentId,
            JoinedDate = employeeCreateRequest.JoinedDate,
            Salary = new Salary
            {
                Amount = employeeCreateRequest.Amount
            }

        };
    }

    public static EmployeeResponse ToEmployeeResponse(this Employee employee)
    {
        return new()
        {
            Id = employee.Id,
            Name = employee.Name,
            JoinedDate = employee.JoinedDate,
            Amount = employee.Salary.Amount,
            DepartmentId = employee.DepartmentId
        };
    }

    private static ProjectOfEmployeeResponse ToEmployeeProjectResponse(this Project projectEmployee, bool enable)
    {
        return new(projectEmployee.Name, projectEmployee.Id, enable);
    }
    public static List<EmployeeResponse> ToEmployeeResponse(this IEnumerable<Employee> employees)
    {
        List<EmployeeResponse> result = new();
        foreach (var employee in employees)
        {
            var employeeResponse = employee.ToEmployeeResponse();
            if (employeeResponse != null)
            {
                result.Add(employeeResponse);
            }
        }
        return result;
    }


    public static void ToEmployee(this EmployeeUpdateRequest employeeUpdateRequest, Employee employee)
    {
        employee.Id = employeeUpdateRequest.Id;
        employee.Name = employeeUpdateRequest.Name;
        employee.DepartmentId = employeeUpdateRequest.DepartmentId;
        employee.JoinedDate = employeeUpdateRequest.JoinedDate;
        employee.Salary.Amount = employeeUpdateRequest.Amount;
    }

    public static EmployeeDetailResponse ToEmployeeDetailsResponse(this Employee employee)
    {
        return new EmployeeDetailResponse
        {
            Id = employee.Id,
            Name = employee.Name,
            JoinedDate = employee.JoinedDate,
            Projects = employee.ProjectEmployees?.Select(e => e.Project.ToEmployeeProjectResponse(e.Enable))?.ToList(),
            Department = new(employee.Department.Name, employee.Department.Id),
            Salary = new(employee.Salary.Amount, employee.Salary.Id)
        };
    }

    public static List<EmployeeDetailResponse> ToEmployeeDetailsResponses(this List<Employee> employee)
    {
        return employee.Select(x => x.ToEmployeeDetailsResponse()).ToList();
    }
}
