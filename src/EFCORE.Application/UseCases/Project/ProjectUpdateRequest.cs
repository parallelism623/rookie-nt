
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.Project;

public class ProjectUpdateRequest
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}


public class ProjectUpdateRequestValidator : AbstractValidator<ProjectUpdateRequest>
{
    private const string NameRegexPattern = @"^\p{L}+$";
    public ProjectUpdateRequestValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage(ProjectValidationMessage.IdRequired);
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage(ProjectValidationMessage.NameRequired)
            .MaximumLength(100).WithMessage(ProjectValidationMessage.MaximumLengthName)
            .Matches(NameRegexPattern).WithMessage(ProjectValidationMessage.NameInvalidFormat);
    }
}
