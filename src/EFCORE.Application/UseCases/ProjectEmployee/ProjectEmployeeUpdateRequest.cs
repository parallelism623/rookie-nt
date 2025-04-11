
using EFCORE.Contract.Messages.ValidationMessages;
using FluentValidation;

namespace EFCORE.Application.UseCases.ProjectEmployee;

public class ProjectEmployeeUpdateRequest
{
    public Guid? Id { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? ProjectId { get; set; }
    public bool Enable { get; set; }
}


public class ProjectEmployeeUpdateRequestValidator : AbstractValidator<ProjectEmployeeUpdateRequest>
{
    public ProjectEmployeeUpdateRequestValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage(ProjectEmployeeValidationMessages.IdRequired); 
        RuleFor(c => c.EmployeeId)
            .NotEmpty().WithMessage(ProjectEmployeeValidationMessages.EmployeeIdRequired);
        RuleFor(c => c.ProjectId)
            .NotEmpty().WithMessage(ProjectEmployeeValidationMessages.ProjectIdRequired);
    }
}