
using EFCORE.Application.UseCases.ProjectEmployee;
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.Project;

public class ProjectCreateRequest
{
    public string? Name { get; set; }
    public List<ProjectEmployeeCreateRequest>? ProjectEmployees { get; set; }
}

public class ProjectCreateRequestValidator : AbstractValidator<ProjectCreateRequest>
{
    private const string NameRegexPattern = @"^\p{L}+$";
    public ProjectCreateRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage(ProjectValidationMessage.NameRequired)
            .MaximumLength(100).WithMessage(ProjectValidationMessage.MaximumLengthName)
            .Matches(NameRegexPattern).WithMessage(ProjectValidationMessage.NameInvalidFormat);
    }
}
