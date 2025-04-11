
using EFCORE.Application.Commons.Mapping;
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Contract.Messages.ErrorMessages;
using EFCORE.Contract.Messages.ResponseMessages;
using EFCORE.Contract.Shared;
using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;

namespace EFCORE.Persistence.Services;

public class ProjectEmployeeSerivce : IProjectEmployeeSerivce
{
    private readonly IProjectEmployeeRepository _projectEmployeeRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public ProjectEmployeeSerivce(IProjectEmployeeRepository projectEmployeeRepository,
                                  IEmployeeRepository employeeRepository,
                                  IProjectRepository projectRepository)
    {
        _employeeRepository = employeeRepository;
        _projectRepository = projectRepository;
        _projectEmployeeRepository = projectEmployeeRepository;
    }
    public async Task<Result<string>> CreateAsync(ProjectEmployeeCreateRequest projectEmployeeCreateRequest)
    {
        if(await IsExistsEmployeeInProject((Guid)projectEmployeeCreateRequest.EmployeeId!,(Guid)projectEmployeeCreateRequest.ProjectId!))
        {
            return Result<string>.Failure(400, ProjectEmployeeErrors.EmployeeAlreadyExistsInProject);
        }
        if(!await IsExistsEmployee((Guid)projectEmployeeCreateRequest.EmployeeId!))
        {
            return Result<string>.Failure(400, ProjectEmployeeErrors.EmployeeNotFound);
        }
        if(!await IsExistsProject((Guid)projectEmployeeCreateRequest.ProjectId!))
        {
            return Result<string>.Failure(400, ProjectEmployeeErrors.ProjectNotFound);
        }
        var projectEmployee = new ProjectEmployee
        {
            EmployeeId = (Guid)projectEmployeeCreateRequest.EmployeeId!,
            ProjectId = (Guid)projectEmployeeCreateRequest.ProjectId!,
            Enable = projectEmployeeCreateRequest.Enable,
        };

        _projectEmployeeRepository.Add(projectEmployee);

        await _projectEmployeeRepository.SaveChangesAsync();

        return Result<string>.Success(ProjectEmployeeResponseMessages.CreatedSuccess);
    }

    public async Task<Result<string>> DeleteAsync(Guid id)
    {
        var projectEmployee = await _projectEmployeeRepository.GetByIdAsync(id);
        if (projectEmployee == null)
        {
            return Result<string>.Failure(400, ProjectEmployeeErrors.NotFound);
        }

        _projectEmployeeRepository.Delete(projectEmployee);
        await _projectEmployeeRepository.SaveChangesAsync();

        return Result<string>.Success(ProjectEmployeeResponseMessages.DeletedSuccess);
    }

    public async Task<Result<ProjectEmployeeResponse>> GetByIdAsync(Guid id)
    {
        var projectEmployee = await _projectEmployeeRepository.GetByIdAsync(id, pe => pe.Employee, pe => pe.Project);

        return Result<ProjectEmployeeResponse>.Success(projectEmployee?.ToProjectEmployeeResponse() ?? default!);
    }

    private Task<bool> IsExistsEmployeeInProject(Guid employeeId, Guid projectId)
    {
        return _projectEmployeeRepository.IsExistsAsync(employeeId, projectId);
    }

    private async Task<bool> IsExistsProject(Guid projectId)
    {
        var project = await _projectRepository.GetByIdAsync(projectId);
        return project != null;
    }

    private async Task<bool> IsExistsEmployee(Guid employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        return employee != null;
    }

    public async Task<Result<string>> UpdateAsync(ProjectEmployeeUpdateRequest projectEmployeeUpdateRequest)
    {
        var projectEmployee = await _projectEmployeeRepository.GetByIdAsync((Guid)projectEmployeeUpdateRequest.Id!);
        if (projectEmployee == null)
        {
            return Result<string>.Failure(400, ProjectEmployeeErrors.NotFound);
        }
        if (!await IsExistsEmployee((Guid)projectEmployeeUpdateRequest.EmployeeId!))
        {
            return Result<string>.Failure(400, ProjectEmployeeErrors.EmployeeNotFound);
        }
        if (!await IsExistsProject((Guid)projectEmployeeUpdateRequest.ProjectId!))
        {
            return Result<string>.Failure(400, ProjectEmployeeErrors.ProjectNotFound);
        }
        projectEmployeeUpdateRequest.ToProjectEmployee(projectEmployee);

        _projectEmployeeRepository.Update(projectEmployee);

        await _projectEmployeeRepository.SaveChangesAsync();

        return Result<string>.Success(ProjectEmployeeResponseMessages.UpdatedSucess);
    }
}
