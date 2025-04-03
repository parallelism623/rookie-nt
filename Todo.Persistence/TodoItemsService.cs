using Mapster;
using Todo.Application;
using Todo.Application.Commons.Exceptions;
using Todo.Application.Commons.Models;
using Todo.Domain.Entities;

namespace Todo.Persistence
{
    public class TodoItemService : ITodoItemService
    {
        public List<TodoItem> TodoItems { get; set; } = new List<TodoItem>
        {
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000101"),
                CreatedAt = DateTime.Now.AddDays(-10),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #1",
                Description = "Fix homepage layout bug",
                Point = 3,
                Assignee = "alice",
                DueFrom = DateTime.Now.AddDays(1),
                DueTo = DateTime.Now.AddDays(3),
                Priority = 1
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000102"),
                CreatedAt = DateTime.Now.AddDays(-8),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #2",
                Description = "Implement login feature",
                Point = 5,
                Assignee = "bob",
                DueFrom = DateTime.Now.AddDays(2),
                DueTo = DateTime.Now.AddDays(5),
                Priority = 2
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000103"),
                CreatedAt = DateTime.Now.AddDays(-7),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #3",
                Description = null,
                Point = 2,
                Assignee = "charlie",
                DueFrom = DateTime.Now.AddDays(-1),
                DueTo = DateTime.Now.AddDays(1),
                Priority = 1
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000104"),
                CreatedAt = DateTime.Now.AddDays(-6),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #4",
                Description = "Refactor user service",
                Point = 8,
                Assignee = "david",
                DueFrom = DateTime.Now.AddDays(-2),
                DueTo = DateTime.Now.AddDays(2),
                Priority = 3
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000105"),
                CreatedAt = DateTime.Now.AddDays(-5),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #5",
                Description = "Code review for main branch",
                Point = 3,
                Assignee = "eve",
                DueFrom = null,
                DueTo = null,
                Priority = 2
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000106"),
                CreatedAt = DateTime.Now.AddDays(-4),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #6",
                Description = "Setup CI/CD pipeline",
                Point = 5,
                Assignee = null,
                DueFrom = DateTime.Now.AddDays(3),
                DueTo = DateTime.Now.AddDays(6),
                Priority = 1
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000107"),
                CreatedAt = DateTime.Now.AddDays(-3),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #7",
                Description = "Add logging to services",
                Point = 4,
                Assignee = "frank",
                DueFrom = DateTime.Now.AddDays(-3),
                DueTo = DateTime.Now.AddDays(-1),
                Priority = 3
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000108"),
                CreatedAt = DateTime.Now.AddDays(-2),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000008"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #8",
                Description = "Fix performance bottleneck",
                Point = 6,
                Assignee = "alice",
                DueFrom = DateTime.Now.AddDays(-1),
                DueTo = DateTime.Now.AddDays(1),
                Priority = 1
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000109"),
                CreatedAt = DateTime.Now.AddDays(-2),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000009"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #9",
                Description = "Prepare sprint demo",
                Point = 2,
                Assignee = "bob",
                DueFrom = DateTime.Now.AddDays(0),
                DueTo = DateTime.Now.AddDays(2),
                Priority = 2
            },
            new TodoItem
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000110"),
                CreatedAt = DateTime.Now.AddDays(-1),
                CreateBy = Guid.Parse("00000000-0000-0000-0000-000000000010"),
                ModifiedAt = null,
                ModifiedBy = Guid.Empty,
                Title = "Task #10",
                Description = "Investigate memory leak",
                Point = 7,
                Assignee = "charlie",
                DueFrom = DateTime.Now.AddDays(1),
                DueTo = DateTime.Now.AddDays(4),
                Priority = 3
            }
        };
        public void Add(TodoItemCreateRequest model)
        {
            TodoItems.Add(model.Adapt<TodoItem>());
        }

        public void BulkAdd(IEnumerable<TodoItemCreateRequest> models)
        {
            TodoItems.AddRange(models.ToList().Adapt<List<TodoItem>>());
        }

        public void BulkDelete(IEnumerable<Guid> itemIds)
        {
            var todoItems = TodoItems.Where(c => itemIds.Contains(c.Id));

            
            if(todoItems.Count() != itemIds.Count())
            {
                throw new Exception("Không thể xóa item không tồn tại");
            }

            TodoItems.RemoveAll(t => todoItems.Where(c => c.Id == t.Id).Any());
        }

        public void Delete(Guid Id)
        {
            var item = TodoItems.Where(t => t.Id == Id).FirstOrDefault();
            if (item == null)
            {
                throw new Exception("Không thể xóa item không tồn tại");
            }
            TodoItems.Remove(item!);
        }

        public List<TodoItemResponse> GetAll()
        {
            return TodoItems.Adapt<List<TodoItemResponse>>();
        }

        public TodoItemResponse GetById(Guid Id)
        {
            return TodoItems.Where(c => c.Id == Id).FirstOrDefault().Adapt<TodoItemResponse>()
                ?? throw new BadRequestException("Item không tồn tại");
        }

        public void Update(TodoItemUpdateRequest item)
        {
            var itemTodo = TodoItems.Where(c => c.Id == item.Id).FirstOrDefault();
            if(itemTodo == null)
            {
                throw new BadRequestException("Không tồn tại item");
            }    
            item.Adapt(itemTodo);
        }
    }
}
