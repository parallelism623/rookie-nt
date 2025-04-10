
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.Salary;

public class SalaryUpdateRequest
{
    public Guid? Id { get; set; }    
    public decimal? Amount { get; set; }
    public Guid? EmployeeId { get; set; }
}

public class SalaryUpdateRequestValidator : AbstractValidator<SalaryUpdateRequest>
{
    public SalaryUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(SalaryValidationMessages.IdRequired);
        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage(SalaryValidationMessages.AmountRequired)
            .GreaterThan(0)
            .WithMessage(SalaryValidationMessages.AmountInvalidRange)
            .Must(a => a is decimal).WithMessage(SalaryValidationMessages.AmountInvalidFormat);
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage(SalaryValidationMessages.EmployeeIdRequired);
       
    }
}
