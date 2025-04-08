using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Contract.Models;
public class PersonResponseModel
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? FullName { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public required string Gender { get; set; }
    public required string BirthPlace { get; set; }
    public string? Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}
