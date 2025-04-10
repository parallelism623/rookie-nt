using EFCORE.Application.UseCases.Project;
using EFCORE.Domain.Entities;

namespace EFCORE.Application.Commons.Mapping;

public static class ProjectMapping
{
    public static Project ToProject(this ProjectCreateRequest projectCreateRequest)
    {
        return new()
        {
            Name = projectCreateRequest.Name!
        };
    }
}
