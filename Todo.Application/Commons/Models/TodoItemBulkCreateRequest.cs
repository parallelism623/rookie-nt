using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Application.Commons.Models;
public class TodoItemBulkCreateRequest
{
    public List<TodoItemCreateRequest> Items { get; set; }
}
