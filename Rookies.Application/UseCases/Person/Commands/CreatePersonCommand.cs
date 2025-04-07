using Rookies.Contract.Models;
using Rookies.Domain.Entities;
using Rookies.Domain.Repositories;

namespace Rookies.Application.UseCases.Persons.Commands;
public record CreatePersonCommand(PersonCreateRequestModel Model) : IRequest { }


public class CreatePersonCommandHandler(IPersonRepository personRepository,
                                        ILogger<CreatePersonCommandHandler> logger) 
                                        : IRequestHandler<CreatePersonCommand>
{

    public async Task Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = request.Model.Adapt<Person>();

        personRepository.Add(person);

        logger.LogInformation("Person with id {Id} created.", person.Id);

        await personRepository.UnitOfWork.SaveChangesAsync();
    }
}
