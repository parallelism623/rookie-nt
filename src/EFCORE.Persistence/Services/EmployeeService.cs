
using EFCORE.Application.Commons.Mapping;
using EFCORE.Application.UseCases.Employee;
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Contract.Messages.ErrorMessages;
using EFCORE.Contract.Messages.ResponseMessages;
using EFCORE.Contract.Shared;
using EFCORE.Domain.Abstract;
using EFCORE.Domain.Repositories;
using EFCORE.Persistence.Helpers;
using EFCORE.Persistence.Specifications;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EFCORE.Persistence.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepostory;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IProjectEmployeeRepository _projectEmployeeRepository;
    private readonly ITransactionManager _transactionManager;
    public EmployeeService(IEmployeeRepository employeeRepostory,
                           IDepartmentRepository departmentRepository,
                           IProjectRepository projectRepository,
                           IProjectEmployeeRepository projectEmployeeRepository,
                           ITransactionManager transactionManager)
    {
        _projectRepository = projectRepository;
        _employeeRepostory = employeeRepostory;
        _departmentRepository = departmentRepository;
        _projectEmployeeRepository = projectEmployeeRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<string>> CreateAsync(EmployeeCreateRequest employeeCreateRequest)
    {
        try
        {
            await _transactionManager.BeginTransactionAsync();
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
            await _transactionManager.CommitTransactionAsync();
            _transactionManager.DisposeTransaction();
            return Result<string>.Success(EmployeeResponseMessages.CreatedSuccess);
        }
        catch
        {
            await _transactionManager.RollbackAsync();
            _transactionManager.DisposeTransaction();
            throw;
        }
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

    public async Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithDepartmentAsync(QueryParameters queryParameters)
    {
        var query = _employeeRepostory.GetAll().Include(e => e.Department);

        var totalCount = await query.CountAsync();
        var employees = await query
                .Skip(queryParameters.PageSize * (queryParameters.PageIndex - 1))
                .Take(queryParameters.PageSize).ToListAsync();

        PagingResult<EmployeeDetailResponse> pagingResult = new()
        {
            Items = employees.ToEmployeeDetailsResponses(),
            PageSize = Math.Min(employees.Count, queryParameters.PageSize),
            PageIndex = queryParameters.PageIndex,
            TotalCount = totalCount
        };
        return Result<PagingResult<EmployeeDetailResponse>>.Success(pagingResult);
    }

    public async Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithProjectsAsync(QueryParameters queryParameters)
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
                                            EmployeeId = pe.EmployeeId,             
                                            Project = project,
                                            Enable = pe.Enable
                                        }
                                    );
        var query = employeeQuery.GroupJoin(projectWithEmployee,
                                            employee => employee.Id,
                                            pe => pe.EmployeeId,
                                            (employee, pe) => new { Employee = employee, Projects = pe });

        var totalCount = await query.CountAsync();
        var employeesWithProjects = await query
            .Skip(queryParameters.PageSize * (queryParameters.PageIndex - 1))
            .Take(queryParameters.PageSize)
            .ToListAsync();

        var employees = employeesWithProjects.Select(ep =>
        {
            return new EmployeeDetailResponse
            {
                Id = ep.Employee.Id,
                Name = ep.Employee.Name,
                JoinedDate = ep.Employee.JoinedDate,
                Projects = ep.Projects
                                    .Select(x =>
                                    new ProjectOfEmployeeResponse(x.Project.Name, x.Project.Id, x.Enable)).ToList()
            };
        }).ToList();
        PagingResult<EmployeeDetailResponse> pagingResult = new()
        {
            Items = employees,
            PageSize = Math.Min(employees.Count, queryParameters.PageSize),
            PageIndex = queryParameters.PageIndex,
            TotalCount = totalCount
        };
        return Result<PagingResult<EmployeeDetailResponse>>.Success(pagingResult);

    }
    public async Task<Result<PagingResult<EmployeeDetailResponse>>> GetByJoinedDateAndSalary(EmployeeQueryParamerters queryParameters)
    {
        var query = _employeeRepostory.GetAll();
        query = query.Where(x => (queryParameters.JoinedFromDate == null || x.JoinedDate >= queryParameters.JoinedFromDate)
                                 && (queryParameters.MinSalary == null || x.Salary.Amount >= queryParameters.MinSalary))
                     .Include(e => e.Salary)
                     .Include(e => e.Department);
        var totalCount = await query.CountAsync();

        var employees = await query.Skip(queryParameters.PageSize * (queryParameters.PageIndex - 1))
                             .Take(queryParameters.PageSize)
                             .ToListAsync();

        PagingResult<EmployeeDetailResponse> pagingResult = new()
        {
            Items = employees.ToEmployeeDetailsResponses(),
            TotalCount = totalCount,
            PageIndex = queryParameters.PageIndex,
            PageSize = queryParameters.PageSize
        };
        return Result<PagingResult<EmployeeDetailResponse>>.Success(pagingResult);
    }


    public async Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithDepartmentRawQueryAsync(QueryParameters queryParameters)
    {
        var connection = _employeeRepostory.GetDbConnection();
        
        var connectionString = connection.ConnectionString;
        var newConnection = new SqlConnection(connectionString);
        await newConnection.OpenAsync();
        using (var command = newConnection.CreateCommand())
        {
            command.CommandText = EmployeeRawQueryHelper.EmployeesWithDepartmentQuery;

            var pageIndexParameters = command.CreateParameter();
            pageIndexParameters.ParameterName = "@PageIndex";
            pageIndexParameters.Value = queryParameters.PageIndex;
            command.Parameters.Add(pageIndexParameters);

            var pageSizeParameters = command.CreateParameter();
            pageSizeParameters.ParameterName = "@PageSize";
            pageSizeParameters.Value = queryParameters.PageSize;
            command.Parameters.Add(pageSizeParameters);

            using (var reader = await command.ExecuteReaderAsync())
            {
                var employees = new List<EmployeeDetailResponse>();

                while (await reader.ReadAsync())
                {
                    employees.Add(new EmployeeDetailResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("EmployeeId")),
                        Name = reader.GetString(reader.GetOrdinal("EmployeeName")),
                        Department = new(reader.GetString(reader.GetOrdinal("DepartmentName")), reader.GetGuid(reader.GetOrdinal("DepartmentId")))
                    });
                }

                await reader.NextResultAsync();
                int totalCount = 0;
                if (await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }

                PagingResult<EmployeeDetailResponse> pagingResult = new()
                {
                    Items = employees,
                    PageIndex = queryParameters.PageIndex,
                    PageSize = queryParameters.PageSize,
                    TotalCount = totalCount
                };

                return Result<PagingResult<EmployeeDetailResponse>>.Success(pagingResult);
            }
        }
        
    }

    public async Task<Result<PagingResult<EmployeeDetailResponse>>> GetWithProjectsRawQueryAsync(QueryParameters queryParameters)
    {
        var connection = _employeeRepostory.GetDbConnection();
        
        var connectionString = connection.ConnectionString;
        var newConnection = new SqlConnection(connectionString);
        await newConnection.OpenAsync();
        using (var command = newConnection.CreateCommand())
        {
            command.CommandText = EmployeeRawQueryHelper.EmployeeWithProjects;

            var pageIndexParameters = command.CreateParameter();
            pageIndexParameters.ParameterName = "@PageIndex";
            pageIndexParameters.Value = queryParameters.PageIndex;
            command.Parameters.Add(pageIndexParameters);

            var pageSizeParameters = command.CreateParameter();
            pageSizeParameters.ParameterName = "@PageSize";
            pageSizeParameters.Value = queryParameters.PageSize;
            command.Parameters.Add(pageSizeParameters);

            using (var reader = await command.ExecuteReaderAsync())
            {
                var employees = new List<EmployeeDetailResponse>();

                while (await reader.ReadAsync())
                {
                    var projectsJson = reader.GetString(reader.GetOrdinal("ProjectsJson"));
                    employees.Add(new EmployeeDetailResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("EmployeeId")),
                        Name = reader.GetString(reader.GetOrdinal("EmployeeName")),
                        Projects = !string.IsNullOrEmpty(projectsJson)
                                    ? JsonSerializer.Deserialize<List<ProjectOfEmployeeResponse>>(projectsJson)
                                    : new List<ProjectOfEmployeeResponse>(),
                        JoinedDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("JoinedDate")))
                    });
                }

                await reader.NextResultAsync();
                int totalCount = 0;
                if (await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }

                PagingResult<EmployeeDetailResponse> pagingResult = new()
                {
                    Items = employees,
                    PageIndex = queryParameters.PageIndex,
                    PageSize = queryParameters.PageSize,
                    TotalCount = totalCount
                };

                return Result<PagingResult<EmployeeDetailResponse>>.Success(pagingResult);
            }
        }
        
    }

    public async Task<Result<PagingResult<EmployeeDetailResponse>>> GetByJoinedDateAndSalaryRawQuery(EmployeeQueryParamerters queryParameters)
    {
        var connection = _employeeRepostory.GetDbConnection();
        
        var connectionString = connection.ConnectionString;
        var newConnection = new SqlConnection(connectionString);
        await newConnection.OpenAsync();
        using (var command = newConnection.CreateCommand())
        {
            command.CommandText = EmployeeRawQueryHelper.EmployeeWithConditionSalaryAndJoinedDate;

            var pageIndexParameters = command.CreateParameter();
            pageIndexParameters.ParameterName = "@PageIndex";
            pageIndexParameters.Value = queryParameters.PageIndex;
            command.Parameters.Add(pageIndexParameters);

            var pageSizeParameters = command.CreateParameter();
            pageSizeParameters.ParameterName = "@PageSize";
            pageSizeParameters.Value = queryParameters.PageSize;
            command.Parameters.Add(pageSizeParameters);

            var minSalaryParameters = command.CreateParameter();
            minSalaryParameters.ParameterName = "@MinSalary";
            minSalaryParameters.Value = queryParameters.MinSalary == null ? DBNull.Value : queryParameters.MinSalary;
            command.Parameters.Add(minSalaryParameters);

            var joinedFromDatedParameters = command.CreateParameter();
            joinedFromDatedParameters.ParameterName = "@JoinedFromDate";
            joinedFromDatedParameters.Value = queryParameters.JoinedFromDate == null ? DBNull.Value : queryParameters.JoinedFromDate;
            command.Parameters.Add(joinedFromDatedParameters);

            using (var reader = await command.ExecuteReaderAsync())
            {
                var employees = new List<EmployeeDetailResponse>();

                while (await reader.ReadAsync())
                {
                    employees.Add(new EmployeeDetailResponse
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("EmployeeId")),
                        Name = reader.GetString(reader.GetOrdinal("EmployeeName")),
                        Salary = new(reader.GetDecimal(reader.GetOrdinal("Amount")), reader.GetGuid(reader.GetOrdinal("SalaryId"))),
                        JoinedDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("JoinedDate"))),
                    });
                }

                await reader.NextResultAsync();
                int totalCount = 0;
                if (await reader.ReadAsync())
                {
                    totalCount = reader.GetInt32(0);
                }

                PagingResult<EmployeeDetailResponse> pagingResult = new()
                {
                    Items = employees,
                    PageIndex = queryParameters.PageIndex,
                    PageSize = queryParameters.PageSize,
                    TotalCount = totalCount
                };

                return Result<PagingResult<EmployeeDetailResponse>>.Success(pagingResult);
            }
        }
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
                        .Select(x => (Guid)x.First().ProjectId!).ToList();
        return projectIds;
    }

}
