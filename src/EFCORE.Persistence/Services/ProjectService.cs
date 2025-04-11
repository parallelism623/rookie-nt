
using EFCORE.Application.Commons.Mapping;
using EFCORE.Application.UseCases.Project;
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Contract.Messages.ErrorMessages;
using EFCORE.Contract.Messages.ResponseMessages;
using EFCORE.Contract.Shared;
using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;

namespace EFCORE.Persistence.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public ProjectService(IProjectRepository projectRepository,
                          IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result<string>> CreateAsync(ProjectCreateRequest projectCreateRequest)
    {
        var employeeIds = projectCreateRequest.ProjectEmployees?
                                .GroupBy(x => x.EmployeeId)
                                .Select(x => (Guid)x.First().EmployeeId!).ToList();
        var project = projectCreateRequest.ToProject();
        _projectRepository.Add(project);

        if (employeeIds != null && employeeIds.Count != 0)
        {
            var employees = await _employeeRepository.GetByIdsAsync(employeeIds);
            if(employees == null || employees.Count != employeeIds?.Count)
            {
                return Result<string>.Failure(400, ProjectError.EmployeeIdsNotExists);
            }
            project.ProjectEmployees = new List<ProjectEmployee>();
            foreach (var employeeId in employeeIds)
            {
                project.ProjectEmployees.Add(new ProjectEmployee
                {
                    EmployeeId = employeeId,
                    Enable = projectCreateRequest.ProjectEmployees?
                                                 .FirstOrDefault(e => e.EmployeeId == employeeId)?
                                                 .Enable ?? false
                }); 
            }
        }

        await _projectRepository.SaveChangesAsync();
        return Result<string>.Success(ProjectResponseMessage.CreatedSuccess);
    }

    public async Task<Result<string>> DeleteAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null)
        {
            return Result<string>.Failure(400, ProjectError.NotFound);
        }

        _projectRepository.Delete(project);
        await _projectRepository.SaveChangesAsync();

        return Result<string>.Success(ProjectResponseMessage.DeletedSuccess);
    }

    public async Task<Result<ProjectResponse>> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id, p => p.ProjectEmployees);
        if (project == null)
        {
            return Result<ProjectResponse>.Failure(400, ProjectError.NotFound);
        }
        var employeeIds = project.ProjectEmployees.Select(x => x.EmployeeId).ToList();

        var employees = await _employeeRepository.GetByIdsAsync(employeeIds);

        var projectResponse = new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            ProjectEmployeeResponses = employees?.Select(x => new ProjectEmployeeResponse
            {
                EmployeeId = x.Id,
                EmployeeName = x.Name,
            }).ToList() ?? null
        };
        return Result<ProjectResponse>.Success(projectResponse);
    }

    public async Task<Result<string>> UpdateAsync(ProjectUpdateRequest projectUpdateRequest)
    {
        var project = await _projectRepository.GetByIdAsync((Guid)projectUpdateRequest.Id!);
        if (project == null)
        {
            return Result<string>.Failure(400, ProjectError.NotFound);
        }
        project.Name = projectUpdateRequest.Name!;

        _projectRepository.Update(project);

        await _projectRepository.SaveChangesAsync();

        return Result<string>.Success(ProjectResponseMessage.UpdatedSuccess);
    }
}
