using EFCORE.Contract.Shared;

namespace EFCORE.Application.UseCases.Department;

public interface IDepartmentService
{
    Task<Result<DepartmentResponse>> GetByIdAsync(Guid id);
    Task<Result<string>> CreateAsync(DepartmentCreateRequest departmentCreateRequest);
    Task<Result<string>> UpdateAsync(DepartmentUpdateRequest departmentUpdateRequest);
    Task<Result<string>> DeleteAsync(Guid id);
}
