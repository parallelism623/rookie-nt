namespace EFCORE.Domain.Entities;

public class ProjectEmployee
{
    public Guid ProjectId { get; set; }
    public Guid EmployeeId { get; set; }
    public Project Project { get; set; } = default!;
    public Employee Employee { get; set; } = default!;
    public bool Enable { get; set; }
}