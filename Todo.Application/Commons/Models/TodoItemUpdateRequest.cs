using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Commons.Models;
public class TodoItemUpdateRequest
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; } = default;
    public int Point { get; set; }
    public bool Status { get; set; }
    public string? Assignee { get; set; }
    public DateTime? DueFrom { get; set; }
    public DateTime? DueTo { get; set; }
    public int Priority { get; set; }
}
