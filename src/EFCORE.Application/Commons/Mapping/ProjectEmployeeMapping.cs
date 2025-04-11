
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Domain.Entities;

namespace EFCORE.Application.Commons.Mapping;

public static class ProjectEmployeeMapping
{
    public static ProjectEmployeeResponse ToProjectEmployeeResponse(this ProjectEmployee projectEmployee)
    {
        return new()
        {
            Id = projectEmployee.Id,
            ProjectId = projectEmployee.ProjectId,
            EmployeeId = projectEmployee.EmployeeId,
            ProjectName = projectEmployee.Project?.Name,
            EmployeeName = projectEmployee.Employee?.Name
        };
    }

    public static void ToProjectEmployee(this ProjectEmployeeUpdateRequest projectEmployeeUpdateRequest, ProjectEmployee projectEmployee)
    {
        projectEmployee.Id = (Guid)projectEmployeeUpdateRequest.Id!;
        projectEmployee.ProjectId = (Guid)projectEmployeeUpdateRequest.ProjectId!;
        projectEmployee.EmployeeId = (Guid)projectEmployeeUpdateRequest.EmployeeId!;
        projectEmployee.Enable = projectEmployeeUpdateRequest.Enable;
    }
}
