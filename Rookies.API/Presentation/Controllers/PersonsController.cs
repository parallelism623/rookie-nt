using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rookies.Application.UseCases.Persons.Commands;
using Rookies.Application.UseCases.Persons.Queries;
using Rookies.Contract.Models;

namespace Rookies.API.Presentation.Controllers;

[Route("[controller]")]
public class PersonsController : ApiBaseController
{
    private readonly IMediator _sender;
    public PersonsController(IMediator sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetsAsync([FromQuery] PersonQueryParameters queryParameters)
    {
        var personQueryRequest = new GetPersonsQuery(queryParameters);

        var personQueryResponse = await _sender.Send(personQueryRequest);

        return Ok(personQueryResponse);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] PersonCreateRequestModel model)
    {
        var personCreateCommandModel = new CreatePersonCommand(model);

        await _sender.Send(personCreateCommandModel);

        return Ok("Tạo mới người dùng thành công");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var personDeleteCommandModel = new DeletePersonCommand(id);

        await _sender.Send(personDeleteCommandModel);

        return Ok("Xóa người dùng thành công");
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePerson([FromBody] PersonUpdateRequestModel model)
    {
        var personUpdateRequest = new UpdatePersonCommand(model);

        await _sender.Send(personUpdateRequest);

        return Ok("Cập nhật người dùng thành công");
    }
}
