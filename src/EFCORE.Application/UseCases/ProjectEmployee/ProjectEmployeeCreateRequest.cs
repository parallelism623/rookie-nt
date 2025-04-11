
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.ProjectEmployee;

public class ProjectEmployeeCreateRequest
{
    public Guid? EmployeeId { get; set; }
    public Guid? ProjectId { get; set; }
    public bool Enable { get; set; }
}

public class ProjectEmployeeCreateRequestValidator : AbstractValidator<ProjectEmployeeCreateRequest>
{
    public ProjectEmployeeCreateRequestValidator()
    {
        RuleFor(c => c.EmployeeId)
            .NotEmpty().WithMessage(ProjectEmployeeValidationMessages.EmployeeIdRequired);
        RuleFor(c => c.ProjectId)
            .NotEmpty().WithMessage(ProjectEmployeeValidationMessages.ProjectIdRequired);
    }
}
