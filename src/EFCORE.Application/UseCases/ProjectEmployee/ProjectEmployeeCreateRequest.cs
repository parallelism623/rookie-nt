
namespace EFCORE.Application.UseCases.ProjectEmployee;

public class ProjectEmployeeCreateRequest
{
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }
    public bool Enable { get; set; }
}
