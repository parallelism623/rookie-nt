
using Rookies.Contract.Exceptions;
using Rookies.Domain.Repositories;

namespace Rookies.Application.UseCases.Persons.Commands;
public record DeletePersonCommand(Guid Id) : IRequest { }


public class PersonDeleteCommandHandler(IPersonRepository personRepository,
                                        ILogger<PersonDeleteCommandHandler> logger) 
                                        : IRequestHandler<DeletePersonCommand>
{
    public async Task Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await personRepository.GetByIdAsync(request.Id);
        if (person is null)
        {
            throw new NotFoundException($"Person with id {request.Id} not found.");
        }
        personRepository.Delete(person);

        logger.LogInformation("Person with id {Id} deleted.", request.Id);

        await personRepository.UnitOfWork.SaveChangesAsync();
    }
}
