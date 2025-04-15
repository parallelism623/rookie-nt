using FluentValidation;
using mvc_todolist.Models.Entities;

namespace mvc_todolist.ModelViews;

public class PersonViewModel
{
    public Guid Id { get; set; }
   
    public string Username { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string FullName => LastName + " " + FirstName;
    public string Password { get; set; } = default!;
    public int Gender { get; set; }
    public DateTime CreateAt { get; set; }
    public Guid CreateBy { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int BirthYear => DateOfBirth.Year;
    public bool Graduated { get; set; } 
    public int Age => DateTime.Now.Year - DateOfBirth.Year;

}

public class PersonViewModelValidator : AbstractValidator<PersonViewModel>
{
    public PersonViewModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username không được để trống.")
            .MaximumLength(50).WithMessage("Username tối đa 50 ký tự.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(50).WithMessage("Tên tối đa 50 ký tự.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Họ không được để trống.")
            .MaximumLength(50).WithMessage("Họ tối đa 50 ký tự.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Địa chỉ không được để trống.")
            .MaximumLength(100).WithMessage("Địa chỉ tối đa 100 ký tự.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password không được để trống.")
            .MinimumLength(6).WithMessage("Password tối thiểu 6 ký tự.");

        // Chỉ chấp nhận giá trị 0, 1 hoặc 2 cho Gender
        RuleFor(x => x.Gender)
            .InclusiveBetween(0, 2)
            .WithMessage("Giới tính phải là một trong các giá trị: 0 (Nam), 1 (Nữ), 2 (Khác).");

        RuleFor(x => x.CreateAt)
            .NotEmpty().WithMessage("Ngày tạo không được để trống.");

        RuleFor(x => x.CreateBy)
            .NotEmpty().WithMessage("Người tạo không được để trống.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Ngày sinh không được để trống.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Ngày sinh không được lớn hơn ngày hiện tại.")
            .Must(date => (DateTime.Now - date).TotalDays / 365.25 <= 150)
            .WithMessage("Ngày sinh không hợp lý, tuổi vượt quá 150 năm.");
    }
}