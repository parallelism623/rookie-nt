using EFCORE.Contract.Shared;

namespace EFCORE.Application.UseCases.Employee;

public interface IEmployeeService
{
    Task<Result<EmployeeResponse>> GetByIdAsync(Guid id);
    Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithDepartmentAsync(QueryParameters queryParamerters);
    Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithProjectsAsync(QueryParameters queryParamerters);
    Task<Result<EmployeeDetailResponse>> GetDetailsByIdAsync(Guid id);
    Task<Result<PagingResult<EmployeeResponse>>> GetByJoinedDateAndSalary(QueryParameters queryParameters);
    Task<Result<string>> CreateAsync(EmployeeCreateRequest employeeCreateRequest);
    Task<Result<string>> UpdateAsync(EmployeeUpdateRequest employeeUpdateRequest);
    Task<Result<string>> DeleteAsync(Guid id);
}
