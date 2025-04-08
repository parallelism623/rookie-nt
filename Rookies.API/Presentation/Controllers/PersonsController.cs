using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rookies.Application.UseCases.Persons.Commands;
using Rookies.Application.UseCases.Persons.Queries;
using Rookies.Contract.Messages;
using Rookies.Contract.Models;
using Rookies.Contract.Shared;

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

        return GetResponse(personQueryResponse, ResponseMessages.PersonsQuerySuccess);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePerson([FromBody] PersonCreateRequestModel model)
    {
        var personCreateCommandModel = new CreatePersonCommand(model);

        await _sender.Send(personCreateCommandModel);

        return GetResponse(ResponseMessages.PersonCreatedSuccess);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        var personDeleteCommandModel = new DeletePersonCommand(id);

        await _sender.Send(personDeleteCommandModel);

        return GetResponse(ResponseMessages.PersonDeletedSuccess);
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePerson([FromBody] PersonUpdateRequestModel model)
    {
        var personUpdateRequest = new UpdatePersonCommand(model);

        await _sender.Send(personUpdateRequest);

        return GetResponse(ResponseMessages.PersonUpdatedSuccess);
    }

}
