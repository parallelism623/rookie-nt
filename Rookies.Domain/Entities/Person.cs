using Rookies.Domain.Abstractions;

namespace Rookies.Domain.Entities;

public class Person : IAuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
    public string BirthPlace { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } = default;
    public DateTime? ModifiedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}