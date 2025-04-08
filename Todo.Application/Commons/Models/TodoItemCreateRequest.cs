namespace Todo.Application.Commons.Models;
public class TodoItemCreateRequest
{
    public required string Title { get; set; }
   
    public string? Description { get; set; } = default;
    public int Point { get; set; } 
    public bool Status { get; set; }
    public string? Assignee { get; set; }   
    public DateTime? DueFrom { get; set; }
    public DateTime? DueTo { get; set; }
    public int Priority { get; set; }
}
