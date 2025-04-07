using Rookies.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Domain.Entities;
public class Person : IAuditableEntity, IEntity<Guid>
{
    public required Guid Id {get;set;}
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? FullName { get; set; } = default!;
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
    public required string BirthPlace { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } = default;
    public DateTime? ModifiedAt {get;set;}
    public Guid CreatedBy {get;set;}
    public Guid? ModifiedBy {get;set;}
    
}
