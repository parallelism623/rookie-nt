
using EFCORE.Application.Commons.Mapping;
using EFCORE.Application.UseCases.Salary;
using EFCORE.Contract.Messages.ErrorMessages;
using EFCORE.Contract.Messages.ResponseMessages;
using EFCORE.Contract.Shared;
using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFCORE.Persistence.Services;

public class SalaryService : ISalaryService
{
    private readonly ISalaryRepository _salaryRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public SalaryService(ISalaryRepository salaryRepository,
                        IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
        _salaryRepository = salaryRepository;
    }

    public async Task<Result<string>> CreateAsync(SalaryCreateRequest salaryCreateRequest)
    {
        var employee = await _employeeRepository.GetByIdAsync((Guid)salaryCreateRequest.EmployeeId!, e => e.Salary);
        if(employee == null)
        {
            return Result<string>.Failure(400, SalaryErrors.EmployeeIdInvalid);
        }
        if(employee.Salary != null)
        {
            return Result<string>.Failure(400, SalaryErrors.EmployeeHasSalary);
        }

        var salary = new Salary
        {
            Amount = salaryCreateRequest.Amount,
            EmployeeId = employee.Id
        };

        _salaryRepository.Add(salary);
        await _salaryRepository.SaveChangesAsync();
        return Result<string>.Success(SalaryResponseMessages.CreatedSuccess);
    }

    public async Task<Result<SalaryResponse>> GetByIdAsync(Guid id)
    {
        var salary = await _salaryRepository.GetByIdAsync(id, s => s.Employee);
        if (salary == null)
        {
            return Result<SalaryResponse>.Failure(400, SalaryErrors.NotFound);
        }
        return Result<SalaryResponse>.Success(salary.ToSalaryResponse());
    }


    public async Task<Result<string>> UpdateAsync(SalaryUpdateRequest salaryUpdateRequest)
    {
        var salary = await _salaryRepository.GetByIdAsync((Guid)salaryUpdateRequest.Id!, s => s.Employee);
        if(salary == null)
        {
            return Result<string>.Failure(400, SalaryErrors.NotFound);
        }
        if(salary.EmployeeId != salaryUpdateRequest.EmployeeId)
        {
            return Result<string>.Failure(400, SalaryErrors.EmployeeIdInvalid);
        }
        salary.Amount = (decimal)salaryUpdateRequest.Amount!;

        _salaryRepository.Update(salary);

        await _salaryRepository.SaveChangesAsync();
        return Result<string>.Success(SalaryResponseMessages.UpdatedSuccess);
    }
}
