using EFCORE.Contract.Shared;

namespace EFCORE.Application.UseCases.Project;

public interface IProjectService
{
    Task<Result<ProjectResponse>> GetByIdAsync(Guid id);
    Task<Result<string>> CreateAsync(ProjectCreateRequest projectCreateRequest);
    Task<Result<string>> UpdateAsync(ProjectUpdateRequest projectUpdateRequest);
    Task<Result<string>> DeleteAsync(Guid id);
}
