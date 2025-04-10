using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Contract.Shared;

namespace EFCORE.Application.UseCases.Salary;

public interface ISalaryService
{
    Task<Result<SalaryResponse>> GetByIdAsync(Guid id);
    Task<Result<string>> UpdateAsync(SalaryUpdateRequest salaryUpdateRequest);
}
