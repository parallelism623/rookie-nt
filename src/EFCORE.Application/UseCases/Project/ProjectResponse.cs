
using EFCORE.Application.UseCases.ProjectEmployee;

namespace EFCORE.Application.UseCases.Project;

public class ProjectResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public List<ProjectEmployeeResponse>? ProjectEmployeeResponses { get; set; }

}
