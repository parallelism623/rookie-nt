
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.Employee;

public class EmployeeUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid DepartmentId { get; set; }
    public DateOnly JoinedDate { get; set; }
    public decimal? Amount { get; set; }
}

public class EmployeeUpdateRequestValidator : AbstractValidator<EmployeeUpdateRequest>
{
    public EmployeeUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(EmployeeValidationMessages.IdRequired);
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(EmployeeValidationMessages.NameRequired)
            .MaximumLength(100)
            .WithMessage(EmployeeValidationMessages.MaximumLengthName);
        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage(EmployeeValidationMessages.DepartmentIdRequired);
        RuleFor(x => x.JoinedDate)
            .NotEmpty()
            .WithMessage(EmployeeValidationMessages.JoinedDateRequired);
    }
}
