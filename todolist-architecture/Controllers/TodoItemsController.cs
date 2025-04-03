using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Todo.Application.Commons.Exceptions;
using Todo.Application.Commons.Models;
using Todo.Application.Services;
using Todo.Contract.Messages;

namespace todolist_architecture.Controllers;


[ApiController]
public class TodoItemsController : ApiControllerBase
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
        return Success(MessageApplication.TodoItemBeAddedSuccess, HttpStatusCode.OK.ToString());
    }
    [HttpPost]
    [Route("todo-items/bulk-add")]
    public IActionResult Create(List<TodoItemCreateRequest> dtos)
    {
        ValidationModel();
        _services.BulkAdd(dtos);
        return Success(MessageApplication.TodoItemBeAddedSuccess, HttpStatusCode.OK.ToString());
    }

    [HttpPut]
    [Route("todo-items")]
    public IActionResult Update([FromBody] TodoItemUpdateRequest model)
    {
        ValidationModel();
        _services.Update(model);
        return Success(MessageApplication.TodoItemBeUpdatedSuccess, HttpStatusCode.OK.ToString());
    }
    [HttpDelete]
    [Route("todo-items/{id}")]
    public IActionResult Delete(Guid id)
    {
        _services.Delete(id);
        return Success(MessageApplication.TodoItemBeDeletedSuccess, HttpStatusCode.OK.ToString());
    }

    [HttpDelete]
    [Route("todo-items")]
    public IActionResult Delete([FromBody] List<Guid> itemIds)
    {
        ValidationModel();
        _services.BulkDelete(itemIds);
        return Success(MessageApplication.TodoItemBeDeletedSuccess, HttpStatusCode.OK.ToString());
    }

}
