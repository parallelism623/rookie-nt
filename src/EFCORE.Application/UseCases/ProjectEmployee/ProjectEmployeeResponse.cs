
namespace EFCORE.Application.UseCases.ProjectEmployee;

public class ProjectEmployeeResponse
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid EmployeeId { get; set; }
    public string? ProjectName { get; set; }
    public string? EmployeeName { get; set; }

}
