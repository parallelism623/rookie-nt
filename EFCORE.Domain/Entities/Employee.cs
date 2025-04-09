using EFCORE.Domain.Abstract;

namespace EFCORE.Domain.Entities;

public class Employee : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid? DepartmentId { get; set; }
    public DateOnly JoinedDate { get; set; }
    public Department? Department { get; set; } = default;
    public Salary Salary { get; set; } = default!;

    public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = default!;
}