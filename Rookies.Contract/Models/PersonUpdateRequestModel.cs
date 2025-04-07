using FluentValidation;
using Rookies.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Contract.Models;
public class PersonUpdateRequestModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required int Gender { get; set; }
    public required string BirthPlace { get; set; }
    public string? Address { get; set; }
}

public class PersonUpdateRequestModelValidator : AbstractValidator<PersonUpdateRequestModel>
{
    public PersonUpdateRequestModelValidator()
    {

    }
}
