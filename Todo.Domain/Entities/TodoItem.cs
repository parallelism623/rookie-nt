using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Abstractions;

namespace Todo.Domain.Entities
{
    public class TodoItem : IEntity<Guid>, IAuditableEntity<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
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

}
