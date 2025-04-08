using Microsoft.AspNetCore.Mvc;
using System.Net;
using Todo.Application.Commons.Models;
using Todo.Application.Services;
using Todo.Contract.Messages;

namespace todolist_architecture.Controllers;


[ApiController]
[Route("[controller]")]
public class TodoItemsController : ApiControllerBase
{
    private readonly ITodoItemService _services;
    public TodoItemsController(ITodoItemService services)
    {
        _services = services;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_services.GetAll());
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok(_services.GetById(id));
    }

    [HttpPost]
    public IActionResult Create(TodoItemCreateRequest dto)
    {
        ValidationModel();
        _services.Add(dto);
        return Success(MessageApplication.TodoItemBeAddedSuccess, HttpStatusCode.OK.ToString());
    }
    [HttpPost]
    [Route("bulk-add")]
    public IActionResult Create([FromBody] TodoItemBulkCreateRequest dtos)
    {
        ValidationModel();
        _services.BulkAdd(dtos.Items);
        return Success(MessageApplication.TodoItemBeAddedSuccess, HttpStatusCode.OK.ToString());
    }

    [HttpPut]
    public IActionResult Update([FromBody] TodoItemUpdateRequest model)
    {
        ValidationModel();
        _services.Update(model);
        return Success(MessageApplication.TodoItemBeUpdatedSuccess, HttpStatusCode.OK.ToString());
    }
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _services.Delete(id);
        return Success(MessageApplication.TodoItemBeDeletedSuccess, HttpStatusCode.OK.ToString());
    }

    [HttpDelete]
    public IActionResult Delete([FromBody] List<Guid> itemIds)
    {
        ValidationModel();
        _services.BulkDelete(itemIds);
        return Success(MessageApplication.TodoItemBeDeletedSuccess, HttpStatusCode.OK.ToString());
    }

}
