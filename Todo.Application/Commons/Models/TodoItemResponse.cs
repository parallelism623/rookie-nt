namespace Todo.Application.Commons.Models;
public class TodoItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; } = default;
    public int Point { get; set; }
    public bool Status { get; set; }
    public string? Assignee { get; set; }
    public DateTime? DueFrom { get; set; }
    public DateTime? DueTo { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreateBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid ModifiedBy { get; set; }
}
