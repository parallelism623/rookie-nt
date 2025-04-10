
namespace EFCORE.Application.UseCases.ProjectEmployee;

public class ProjectEmployeeUpdateRequest
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }
    public bool Enable { get; set; }
}
