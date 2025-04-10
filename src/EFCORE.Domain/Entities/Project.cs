using EFCORE.Domain.Abstract;

namespace EFCORE.Domain.Entities;

public class Project : AuditableEntity, IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = default!;
}