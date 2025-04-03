using Todo.Application.Commons.Models;
using Todo.Domain.Entities;

namespace Todo.Application.Services;
public interface ITodoItemService
{
    List<TodoItem> TodoItems { get; set; }
    TodoItemResponse GetById(Guid Id);
    List<TodoItemResponse> GetAll();
    void Delete(Guid Id);
    void Update(TodoItemUpdateRequest item);
    void Add(TodoItemCreateRequest item);
    void BulkAdd(IEnumerable<TodoItemCreateRequest> items);
    void BulkDelete(IEnumerable<Guid> itemIds);
}
