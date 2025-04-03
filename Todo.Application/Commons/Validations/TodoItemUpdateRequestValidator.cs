using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Commons.Models;

namespace Todo.Application.Commons.Validations;
public class TodoItemUpdateRequestValidator : AbstractValidator<TodoItemUpdateRequest>
{
    public TodoItemUpdateRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id item không được trống");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề là bắt buộc.");
        RuleFor(x => x.Point).InclusiveBetween(1, 10).WithMessage("Điểm nên nằm trong khoảng 1 - 10");
        RuleFor(x => x.Priority).InclusiveBetween(0, 2).WithMessage("Priority nên có giá trị thấp/vừa/cao");

    }
}
