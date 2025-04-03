using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Todo.Application;
using Todo.Application.Commons.Exceptions;
using Todo.Application.Commons.Models;

namespace todolist_architecture;


[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly ITodoItemService _services;
    public TodoItemsController(ITodoItemService services)
    {
        _services = services;
    }

    [HttpGet]
    [Route("todo-items")]
    public IActionResult GetAll()
    {
        return Ok(_services.GetAll());
    }


    [HttpGet]
    [Route("todo-items/{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok(_services.GetById(id));
    }

    [HttpPost]
    [Route("todo-items")]
    public IActionResult Create(TodoItemCreateRequest dto)
    {
        ValidationModel();
        _services.Add(dto);
        return Ok("Thêm mới sản phẩm thành công");
    }
    [HttpPost]
    [Route("todo-items/bulk-add")]
    public IActionResult Create(List<TodoItemCreateRequest> dtos)
    {
        ValidationModel();
        _services.BulkAdd(dtos);
        return Ok("Thêm mới sản phẩm thành công");
    }

    [HttpPut]
    [Route("todo-items")]
    public IActionResult Update([FromBody] TodoItemUpdateRequest model)
    {
        ValidationModel();
        _services.Update(model);
        return Ok("Cập nhật sản phẩm thành công");
    }
    [HttpDelete]
    [Route("todo-items/{id}")]
    public IActionResult Delete(Guid id)
    {
        _services.Delete(id);
        return Ok("Xóa sản phẩm thành công");
    }

    [HttpDelete]
    [Route("todo-items")]
    public IActionResult Delete([FromBody] List<Guid> itemIds)
    {
        ValidationModel();
        _services.BulkDelete(itemIds);
        return Ok("Xóa các sản phẩm thành công");
    }
    private void ValidationModel()
    {
        if (!ModelState.IsValid)
        {
            throw new ValidationException(ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => x.Value.Errors.First().ErrorMessage)
                .FirstOrDefault());
        }
    }
}
