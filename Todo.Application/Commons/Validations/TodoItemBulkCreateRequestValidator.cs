using FluentValidation;
using Todo.Application.Commons.Enums;
using Todo.Application.Commons.Models;
using Todo.Contract.Messages;

namespace Todo.Application.Commons.Validations;
public class TodoItemBulkCreateRequestValidator : AbstractValidator<TodoItemBulkCreateRequest>
{
    public TodoItemBulkCreateRequestValidator()
    {
        RuleForEach(c => c.Items)
           .Must(x => !string.IsNullOrEmpty(x.Title) && x.Title.Length > 0).WithMessage(MessageValidation.TitleCannotEmpty)
           .Must(x => x.Point >= 1 && x.Point <= 10).WithMessage(MessageValidation.PointShouldBeInRange)
           .Must(p => Enum.IsDefined(typeof(PriorityEnum), p.Priority)).WithMessage(MessageValidation.PriorityShouldBeInRange);
    }
}
