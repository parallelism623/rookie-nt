using FluentValidation;
using Todo.Application.Commons.Enums;
using Todo.Application.Commons.Models;
using Todo.Contract.Messages;

namespace Todo.Application.Commons.Validations;
public class TodoItemUpdateRequestValidator : AbstractValidator<TodoItemUpdateRequest>
{
    public TodoItemUpdateRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(MessageValidation.IdCannotEmpty);
        RuleFor(x => x.Title).NotEmpty().WithMessage(MessageValidation.TitleCannotEmpty);
        RuleFor(x => x.Point).InclusiveBetween(1, 10).WithMessage(MessageValidation.PointShouldBeInRange);
        RuleFor(x => x.Priority).Must(p => Enum.IsDefined(typeof(PriorityEnum), p)).WithMessage(MessageValidation.PriorityShouldBeInRange);

    }
}
