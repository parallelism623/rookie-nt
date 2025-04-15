
using FluentValidation.TestHelper;
using mvc_todolist.ModelViews;


public class PersonViewModelValidatorTests
{
    private PersonViewModelValidator _validator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _validator = new PersonViewModelValidator();
    }
    [Test]
    public void ValidPerson_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var person = new PersonViewModel
        {
            Id = Guid.NewGuid(),
            Username = "johnsmith",
            FirstName = "John",
            LastName = "Smith",
            Address = "123 Main Street",
            Password = "securePass",
            Gender = 1,
            CreateAt = DateTime.Now,
            CreateBy = Guid.NewGuid(),
            DateOfBirth = new DateTime(1990, 5, 1),
            Graduated = true
        };

        // Action
        var result = _validator.TestValidate(person);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void EmptyUsername_ShouldHaveValidationError()
    {
        // Arrange
        var person = new PersonViewModel
        {
            Id = Guid.NewGuid(),
            Username = "",
            FirstName = "John",
            LastName = "Smith",
            Address = "123 Main Street",
            Password = "securePass",
            Gender = 1,
            CreateAt = DateTime.Now,
            CreateBy = Guid.NewGuid(),
            DateOfBirth = new DateTime(1990, 5, 1),
            Graduated = true
        };

        // Action
        var result = _validator.TestValidate(person);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username)
              .WithErrorMessage("Username không được để trống.");
    }

    [Test]
    public void PasswordTooShort_ShouldHaveValidationError()
    {
        // Arrange
        var person = new PersonViewModel
        {
            Id = Guid.NewGuid(),
            Username = "johnsmith",
            FirstName = "John",
            LastName = "Smith",
            Address = "123 Main Street",
            Password = "123", 
            Gender = 1,
            CreateAt = DateTime.Now,
            CreateBy = Guid.NewGuid(),
            DateOfBirth = new DateTime(1990, 5, 1),
            Graduated = true
        };

        // Action
        var result = _validator.TestValidate(person);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password)
              .WithErrorMessage("Password tối thiểu 6 ký tự.");
    }

    [TestCase(3)]
    [TestCase(-1)]
    public void InvalidGender_ShouldHaveValidationError(int invalidGender)
    {
        // Arrange
        var person = new PersonViewModel
        {
            Id = Guid.NewGuid(),
            Username = "johnsmith",
            FirstName = "John",
            LastName = "Smith",
            Address = "123 Main Street",
            Password = "securePass",
            Gender = invalidGender,
            CreateAt = DateTime.Now,
            CreateBy = Guid.NewGuid(),
            DateOfBirth = new DateTime(1990, 5, 1),
            Graduated = true
        };

        // Action
        var result = _validator.TestValidate(person);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Gender)
              .WithErrorMessage("Giới tính phải là một trong các giá trị: 0 (Nam), 1 (Nữ), 2 (Khác).");
    }

    [Test]
    public void DateOfBirthInFuture_ShouldHaveValidationError()
    {
        // Arrange
        var person = new PersonViewModel
        {
            Id = Guid.NewGuid(),
            Username = "johnsmith",
            FirstName = "John",
            LastName = "Smith",
            Address = "123 Main Street",
            Password = "securePass",
            Gender = 1,
            CreateAt = DateTime.Now,
            CreateBy = Guid.NewGuid(),
            DateOfBirth = DateTime.Now.AddDays(1), 
            Graduated = true
        };

        // Action
        var result = _validator.TestValidate(person);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
              .WithErrorMessage("Ngày sinh không được lớn hơn ngày hiện tại.");
    }

    [Test]
    public void TooOldDateOfBirth_ShouldHaveValidationError()
    {
        // Arrange
        var tooOldDate = DateTime.Now.AddYears(-151);
        var person = new PersonViewModel
        {
            Id = Guid.NewGuid(),
            Username = "johnsmith",
            FirstName = "John",
            LastName = "Smith",
            Address = "123 Main Street",
            Password = "securePass",
            Gender = 1,
            CreateAt = DateTime.Now,
            CreateBy = Guid.NewGuid(),
            DateOfBirth = tooOldDate,
            Graduated = true
        };

        // Action
        var result = _validator.TestValidate(person);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
              .WithErrorMessage("Ngày sinh không hợp lý, tuổi vượt quá 150 năm.");
    }
}
