using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.Department;

public class DepartmentUpdateRequest
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}


public class DepartmentUpdateValidator : AbstractValidator<DepartmentUpdateRequest>
{
    private const string NameRegexPattern = @"^\p{L}+$";
    public DepartmentUpdateValidator()
    {
        RuleFor(d => d.Id)
            .NotEmpty().WithMessage(DepartmentValidationMessages.IdRequired);
        RuleFor(d => d.Name)
            .NotEmpty().WithMessage(DepartmentValidationMessages.NameRequired)
            .MaximumLength(100).WithMessage(DepartmentValidationMessages.MaximumLengthName)
            .Matches(NameRegexPattern).WithMessage(DepartmentValidationMessages.NameInvalidFormat); 
    }
}