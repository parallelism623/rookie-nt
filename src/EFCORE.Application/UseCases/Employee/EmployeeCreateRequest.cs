
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.Employee;

public class EmployeeCreateRequest
{
    public string Name { get; set; } = default!;
    public Guid DepartmentId { get; set; }
    public DateOnly JoinedDate { get; set; }
    public decimal Amount { get; set; }

    public List<ProjectEmployeeCreateRequest>? ProjectEmployees { get; set; }
}

public class EmployeeCreateRequestValidator : AbstractValidator<EmployeeCreateRequest>
{
    public EmployeeCreateRequestValidator()
    {
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
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage(EmployeeValidationMessages.AmountInvalid);
    }
}

