using Rookies.Contract.Models;
using Rookies.Domain.Entities;
using Rookies.Domain.Repositories;

namespace Rookies.Application.UseCases.Persons.Commands;

public record UpdatePersonCommand(PersonUpdateRequestModel Model) : IRequest { }


public class UpdatePersonCommandHandler(IPersonRepository personRepository,
                                        ILogger<UpdatePersonCommandHandler> logger) 
                                        : IRequestHandler<UpdatePersonCommand>
{

    public async Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = request.Model.Adapt<Person>();

        personRepository.Update(person);

        logger.LogInformation("Person with id {Id} updated - Data {@Data}.", person.Id,  request.Model);

        await personRepository.UnitOfWork.SaveChangesAsync();
    }
}
