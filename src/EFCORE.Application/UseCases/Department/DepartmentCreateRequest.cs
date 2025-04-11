
using EFCORE.Application.UseCases.Employee;
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.Department;

public class DepartmentCreateRequest
{
    public string? Name { get; set; }
}


public class DepartmentCreateRequestValidator : AbstractValidator<DepartmentCreateRequest>
{
    private const string NameRegexPattern = @"^[\p{L}\d_\-\s]+$";
    public DepartmentCreateRequestValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage(DepartmentValidationMessages.NameRequired)
            .MaximumLength(100).WithMessage(DepartmentValidationMessages.MaximumLengthName)
            .Matches(NameRegexPattern).WithMessage(DepartmentValidationMessages.NameInvalidFormat);
    }
}