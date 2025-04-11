
using EFCORE.Application.UseCases.Department;
using EFCORE.Domain.Entities;

namespace EFCORE.Application.Commons.Mapping;

public static class DepartmentMapping
{
    public static Department ToDepartment(this DepartmentCreateRequest departmentCreateRequest)
    {
        return new() { Name = departmentCreateRequest.Name! };
    }

    public static Department ToDepartment(this DepartmentUpdateRequest departmentUpdateRequest, Department department)
    {
        department.Name = departmentUpdateRequest.Name!;
        return department;
    }

    public static DepartmentResponse ToDepartmentResponse(this Department department)
    {
        return new() 
        { 
            Name = department.Name, 
            Id = department.Id, 
            Employees = department.Employees?.ToEmployeesResponse()
        };
    }

}
