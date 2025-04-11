using EFCORE.Contract.Shared;

namespace EFCORE.Application.UseCases.Employee;

public interface IEmployeeService
{
    Task<Result<EmployeeResponse>> GetByIdAsync(Guid id);
    Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithDepartmentAsync(QueryParameters queryParameters);

    Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithDepartmentRawQueryAsync(QueryParameters queryParameters);
    Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithProjectsAsync(QueryParameters queryParameters);
    Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithProjectsRawQueryAsync(QueryParameters queryParameters);
    Task<Result<EmployeeDetailResponse>> GetDetailsByIdAsync(Guid id);
    Task<Result<PagingResult<EmployeeDetailResponse>>> GetByJoinedDateAndSalary(EmployeeQueryParamerters queryParameters);
    Task<Result<PagingResult<EmployeeDetailResponse>>> GetByJoinedDateAndSalaryRawQuery(EmployeeQueryParamerters queryParameters);
    Task<Result<string>> CreateAsync(EmployeeCreateRequest employeeCreateRequest);
    Task<Result<string>> UpdateAsync(EmployeeUpdateRequest employeeUpdateRequest);
    Task<Result<string>> DeleteAsync(Guid id);
}
