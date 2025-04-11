
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.Salary;

public class SalaryCreateRequest
{
    public decimal Amount { get; set; }
    public Guid? EmployeeId { get; set; }
}


public class SalaryCreatRequestValidator : AbstractValidator<SalaryCreateRequest>
{
    public SalaryCreatRequestValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage(SalaryValidationMessages.IdRequired);
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage(SalaryValidationMessages.AmountInvalidRange);
    }
}
