using EFCORE.Contract.Shared;

namespace EFCORE.Application.UseCases.ProjectEmployee;

public interface IProjectEmployeeSerivce
{
    Task<Result<ProjectEmployeeResponse>> GetByIdAsync(Guid id);
    Task<Result<string>> CreateAsync(ProjectEmployeeCreateRequest projectEmployeeCreateRequest);
    Task<Result<string>> DeleteAsync(Guid id);
}
