using FluentValidation;
using Rookies.Domain.Enums;

namespace Rookies.Contract.Models;
public class PersonCreateRequestModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required int Gender { get; set; }
    public required string BirthPlace { get; set; }
    public string? Address { get; set; }
}

public class PersonCreateRequestModelValidator : AbstractValidator<PersonCreateRequestModel>
{
    public PersonCreateRequestModelValidator()
    {
       RuleFor(x => x.FirstName.Length).ExclusiveBetween(1, 50)
             .WithMessage("First name must be between 1 and 50 characters.");
       RuleFor(x => x.LastName.Length).ExclusiveBetween(1, 50)
            .WithMessage("Last name must be between 1 and 50 characters.");
       RuleFor(x => x.Gender).Must(g => Enum.IsDefined(typeof(PersonGender), g))
            .WithMessage("Gender is invalid");
       RuleFor(x => x.FirstName).Matches(@"^[\p{L}\p{M}\s.'-]+$")
            .WithMessage("First name is invalid");
       RuleFor(x => x.LastName).Matches(@"^[\p{L}\p{M}\s.'-]+$")
            .WithMessage("Last name is invalid");
       RuleFor(x => x.BirthPlace.Length).ExclusiveBetween(1, 100)
            .WithMessage("Last name must be between 1 and 100 characters.");
    }
}
