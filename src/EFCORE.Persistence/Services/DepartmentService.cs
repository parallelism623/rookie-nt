using EFCORE.Application.Commons.Mapping;
using EFCORE.Application.UseCases.Department;
using EFCORE.Contract.Messages.ErrorMessages;
using EFCORE.Contract.Messages.ResponseMessages;
using EFCORE.Contract.Shared;
using EFCORE.Domain.Abstract;
using EFCORE.Domain.Repositories;
using Microsoft.Extensions.Logging;


namespace EFCORE.Persistence.Services;


public class DepartmentService(IDepartmentRepository departmentRepository,
                                IEmployeeRepository employeeRepository) : IDepartmentService
{
    public async Task<Result<string>> CreateAsync(DepartmentCreateRequest departmentCreateRequest)
    {
      
        var department = departmentCreateRequest.ToDepartment();

        if(departmentCreateRequest.EmployeeIds != null && departmentCreateRequest.EmployeeIds.Count > 0)
        {
            var employees = await employeeRepository.GetByIdsAsync(departmentCreateRequest.EmployeeIds);

            if (employees != null && employees.Count > 0)
            {
                foreach (var employee in employees)
                {
                    department.Employees.Add(employee);
                }
            }
        }
        departmentRepository.Add(department);

        await departmentRepository.SaveChangesAsync();

        return Result<string>.Success(DepartmentResponseMessages.CreatedSuccess);
        
    }

    public async Task<Result<string>> DeleteAsync(Guid id)
    {

        var department = await departmentRepository.GetByIdAsync(id);
        if(department == null)
        {
            return Result<string>.Failure(400, DepartmentError.NotFound);
        }

        departmentRepository.Delete(department);

        await departmentRepository.SaveChangesAsync();
        return Result<string>.Success(DepartmentResponseMessages.DeletedSuccess);
        
    }

    public async Task<Result<DepartmentResponse>> GetByIdAsync(Guid id)
    {
        var department = await departmentRepository.GetByIdAsync(id);
        if(department == null)
        {
            return Result<DepartmentResponse>.Failure(400, DepartmentError.NotFound);
        }

        return Result<DepartmentResponse>.Success(department.ToDepartmentResponse());
    }

    public async Task<Result<string>> UpdateAsync(DepartmentUpdateRequest departmentUpdateRequest)
    {
        var department = await departmentRepository.GetByIdAsync((Guid)departmentUpdateRequest.Id!);
        if (department == null)
        {
            return Result<string>.Failure(400, DepartmentError.NotFound);
        }

        departmentUpdateRequest.ToDepartment(department);
        await departmentRepository.SaveChangesAsync();
        return Result<string>.Success(DepartmentResponseMessages.UpdatedSuccess);
    }
}
