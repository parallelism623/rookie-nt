
using EFCORE.Application.Commons.Mapping;
using EFCORE.Application.UseCases.Salary;
using EFCORE.Contract.Messages.ErrorMessages;
using EFCORE.Contract.Messages.ResponseMessages;
using EFCORE.Contract.Shared;
using EFCORE.Domain.Repositories;

namespace EFCORE.Persistence.Services;

public class SalaryService : ISalaryService
{
    private readonly ISalaryRepository _salaryRepository;
    public SalaryService(ISalaryRepository salaryRepository)
    {
        _salaryRepository = salaryRepository;
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
        if(salary.EmployeeId != salary.Id)
        {
            return Result<string>.Failure(400, SalaryErrors.EmployeeIdInvalid);
        }
        salary.Amount = (decimal)salaryUpdateRequest.Amount!;

        _salaryRepository.Update(salary);

        await _salaryRepository.SaveChangesAsync();
        return Result<string>.Success(SalaryResponseMessages.UpdatedSuccess);
    }
}
