namespace Rookies.Contract.Models;

public class PersonResponseModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? FullName { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string BirthPlace { get; set; }
    public string? Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}