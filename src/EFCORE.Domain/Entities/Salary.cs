using EFCORE.Domain.Abstract;

namespace EFCORE.Domain.Entities;

public class Salary : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public Employee Employee { get; set; } = default!;
    public Guid EmployeeId { get; set; }
    public decimal Amount { get; set; }
}