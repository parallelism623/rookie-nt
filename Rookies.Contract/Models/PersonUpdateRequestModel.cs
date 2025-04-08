using FluentValidation;
using Rookies.Contract.Messages;
using Rookies.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Rookies.Contract.Models;
public class PersonUpdateRequestModel
{
    [Required(ErrorMessage = ErrorMessages.PersonIdRequire)]
    public Guid? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
    public string? BirthPlace { get; set; }
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}

public class PersonUpdateRequestModelValidator : AbstractValidator<PersonUpdateRequestModel>
{
    public PersonUpdateRequestModelValidator()
    {
        RuleFor(x => x.FirstName).NotNull()
            .MinimumLength(1)
            .WithMessage(ErrorMessages.ErrorMessageInvalidLengthFirstName)
            .MaximumLength(50)
            .WithMessage(ErrorMessages.ErrorMessageInvalidLengthFirstName);
        RuleFor(x => x.LastName).NotNull()
            .MinimumLength(1)
            .WithMessage(ErrorMessages.ErrorMessageInvalidLengthLastName)
            .MaximumLength(50)
            .WithMessage(ErrorMessages.ErrorMessageInvalidLengthLastName);
        RuleFor(x => x.Gender).Must(g => Enum.IsDefined(typeof(PersonGender), g))
             .WithMessage(ErrorMessages.GenderInvalid);
        RuleFor(x => x.FirstName).Matches(@"^[\p{L}\p{M}\s.'-]+$")
             .WithMessage(ErrorMessages.FirstNameInvalid);
        RuleFor(x => x.LastName).Matches(@"^[\p{L}\p{M}\s.'-]+$")
             .WithMessage(ErrorMessages.LastNameInvalid);
        RuleFor(x => x.BirthPlace).NotNull()
             .MinimumLength(1)
             .WithMessage(ErrorMessages.BirthPlaceInvalidLength)
             .MaximumLength(100)
             .WithMessage(ErrorMessages.BirthPlaceInvalidLength);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ErrorMessages.EmailShouldNotBeEmpty)
            .EmailAddress().WithMessage(ErrorMessages.EmailAddressInvalid);
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(ErrorMessages.PhoneNumberShouldNotBeEmpty)
            .Matches(@"^\+?[0-9]{10,15}$").WithMessage(ErrorMessages.PhoneNumberInvalid);
    }
}
