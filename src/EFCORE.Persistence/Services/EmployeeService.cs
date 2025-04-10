
using EFCORE.Application.Commons.Mapping;
using EFCORE.Application.UseCases.Employee;
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Contract.Messages.ErrorMessages;
using EFCORE.Contract.Messages.ResponseMessages;
using EFCORE.Contract.Shared;
using EFCORE.Domain.Abstract;
using EFCORE.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Persistence.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepostory;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectEmployeeRepository _projectEmployeeRepository;
    public EmployeeService(IEmployeeRepository employeeRepostory,
                           IDepartmentRepository departmentRepository,
                           IProjectRepository projectRepository,
                           IProjectEmployeeRepository projectEmployeeRepository)
    {
        _projectRepository = projectRepository;
        _employeeRepostory = employeeRepostory;
        _departmentRepository = departmentRepository;
        _projectEmployeeRepository = projectEmployeeRepository;
    }

    public async Task<Result<string>> CreateAsync(EmployeeCreateRequest employeeCreateRequest)
    {
        if (!await IsDepartmentExists(employeeCreateRequest.DepartmentId))
        {
            return Result<string>.Failure(400, EmployeeError.DepartmentNotFound);
        }
        if (!await IsProjectIdsValid(employeeCreateRequest.ProjectEmployees))
        {
            return Result<string>.Failure(400, EmployeeError.ProjectNotFound);
        }
        var employee = employeeCreateRequest.ToEmployee();
        var projectIds = GetProjectIdsFromProjectEmployees(employeeCreateRequest.ProjectEmployees);
        foreach (var projectId in projectIds)
        {
            employee.ProjectEmployees.Add(new()
            {
                ProjectId = projectId,
                Enable = employeeCreateRequest.ProjectEmployees?
                                              .FirstOrDefault(e => e.ProjectId == projectId)?
                                              .Enable ?? false
            });
        }
        _employeeRepostory.Add(employee);
        await _employeeRepostory.SaveChangesAsync();
        return Result<string>.Success(EmployeeResponseMessages.CreatedSuccess);
    }

    public async Task<Result<string>> DeleteAsync(Guid id)
    {
        var employee = await _employeeRepostory.GetByIdAsync(id);
        if (employee == null)
        {
            return Result<string>.Failure(400, EmployeeError.NotFound);
        }
        _employeeRepostory.Delete(employee);
        await _employeeRepostory.SaveChangesAsync();
        return Result<string>.Success(EmployeeResponseMessages.DeletedSuccess);
    }

    public async Task<Result<EmployeeResponse>> GetByIdAsync(Guid id)
    {
        var employee = await _employeeRepostory.GetByIdAsync(id, e => e.Salary);
        if (employee == null)
        {
            return Result<EmployeeResponse>.Failure(400, EmployeeError.NotFound);
        }

        return Result<EmployeeResponse>.Success(employee.ToEmployeeResponse());
    }

    public async Task<Result<EmployeeDetailResponse>> GetDetailsByIdAsync(Guid id)
    {
        var employee = await _employeeRepostory.GetEmployeeDetailByIdAsync(id);
        if (employee == null)
        {
            return Result<EmployeeDetailResponse>.Failure(400, EmployeeError.NotFound);
        }
        return Result<EmployeeDetailResponse>.Success(employee.ToEmployeeDetailsResponse());
    }

    public async Task<Result<string>> UpdateAsync(EmployeeUpdateRequest employeeUpdateRequest)
    {
        var employee = await _employeeRepostory.GetByIdAsync(employeeUpdateRequest.Id!, e => e.Salary);
        if (employee == null)
        {
            return Result<string>.Failure(400, EmployeeError.NotFound);
        }

        employeeUpdateRequest.ToEmployee(employee);
        _employeeRepostory.Update(employee);
        await _employeeRepostory.SaveChangesAsync();
        return Result<string>.Success(EmployeeResponseMessages.UpdatedSuccess);
    }

    public async Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithDepartmentAsync(QueryParameters queryParamerters)
    {
        var query = _employeeRepostory.GetAll().Include(e => e.Department);

        var totalCount = await query.CountAsync();
        var employees = await query
                .Skip(queryParamerters.PageSize * (queryParamerters.PageIndex - 1))
                .Take(queryParamerters.PageSize).ToListAsync();

        PagingResult<EmployeeDetailResponse> pagingResult = new()
        {
            Items = employees.ToEmployeeDetailsResponses(),
            PageSize = Math.Min(employees.Count, queryParamerters.PageSize),
            PageIndex = queryParamerters.PageIndex,
            TotalCount = totalCount
        };
        return Result<PagingResult<EmployeeDetailResponse>>.Success(pagingResult);
    }

    public async Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithProjectsAsync(QueryParameters queryParamerters)
    {
        var employeeQuery = _employeeRepostory.GetAll();
        var projectQuery = _projectRepository.GetAll();
        var projectEmployeeQuery = _projectEmployeeRepository.GetAll();

        var projectWithEmployee = projectQuery
                                    .Join(
                                        projectEmployeeQuery,         
                                        project => project.Id,             
                                        pe => pe.ProjectId,                
                                        (project, pe) => new
                                        {
                                            pe.EmployeeId,             
                                            Project = project,
                                            Enable = pe.Enable
                                        }
                                    );
        var query = employeeQuery
            .GroupJoin(
                projectWithEmployee,            
                employee => employee.Id,     
                x => x.EmployeeId,              
                (employee, peGroup) => new { employee, peGroup }
            )
            .SelectMany(
                group => group.peGroup.DefaultIfEmpty(),
                (group, pe) => new EmployeeDetailResponse
                {
                    Id = group.employee.Id,
                    Name = group.employee.Name,
                    JoinedDate = group.employee.JoinedDate,
                    Projects = group.peGroup
                                    .Select(x => 
                                    new ProjectOfEmployeeResponse(x.Project.Name, x.Project.Id, x.Enable)).ToList()
                }
            );

        var totalCount = await query.CountAsync();
        var employees = await query.Skip(queryParamerters.PageSize * (queryParamerters.PageIndex - 1))
                                .Take(queryParamerters.PageSize).ToListAsync();
        PagingResult<EmployeeDetailResponse> pagingResult = new()
        {
            Items = employees,
            PageSize = Math.Min(employees.Count, queryParamerters.PageSize),
            PageIndex = queryParamerters.PageIndex,
            TotalCount = totalCount
        };
        return Result<PagingResult<EmployeeDetailResponse>>.Success(pagingResult);

    }
    public async Task<Result<PagingResult<EmployeeResponse>>> GetByJoinedDateAndSalary(QueryParameters queryParameters)
    {
        var query = _employeeRepostory.GetByJoinedDateAndSalary();

        var totalCount = await query.CountAsync();

        var employees = await query.Skip(queryParameters.PageSize * (queryParameters.PageIndex - 1))
                                .Take(queryParameters.PageSize).ToListAsync();

        PagingResult<EmployeeResponse> pagingResult = new()
        {
            Items = employees.ToEmployeeResponse(),
            TotalCount = totalCount,
            PageIndex = queryParameters.PageIndex,
            PageSize = queryParameters.PageSize
        };
        return Result<PagingResult<EmployeeResponse>>.Success(pagingResult);
    }

    private async Task<bool> IsDepartmentExists(Guid departmentId)
    {
        var department = await _departmentRepository.GetByIdAsync(departmentId);
        return department != null;
    }

    private async Task<bool> IsProjectIdsValid(List<ProjectEmployeeCreateRequest>? projectEmployees)
    {
        if (projectEmployees == null || projectEmployees.Count == 0)
        {
            return true;
        }
        var projectIds = GetProjectIdsFromProjectEmployees(projectEmployees);

        var projects = await _projectRepository.GetByIdsAsync(projectIds);

        if (projects == null || projects.Count != projectIds?.Count)
        {
            return false;
        }
        return true;
    }
    private static List<Guid> GetProjectIdsFromProjectEmployees(List<ProjectEmployeeCreateRequest>? projectEmployees)
    {
        if (projectEmployees == null || projectEmployees.Count == 0)
        {
            return new List<Guid>();
        }
        var projectIds = projectEmployees
                        .GroupBy(x => x.ProjectId)
                        .Select(x => x.First().ProjectId).ToList();
        return projectIds;
    }

}
