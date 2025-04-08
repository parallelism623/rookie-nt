
using Rookies.Contract.Exceptions;
using Rookies.Contract.Messages;
using Rookies.Domain.Repositories;

namespace Rookies.Application.UseCases.Persons.Commands;
public record DeletePersonCommand(Guid Id) : IRequest { }


public class PersonDeleteCommandHandler(IPersonRepository personRepository,
                                        ILogger<PersonDeleteCommandHandler> logger) 
                                        : IRequestHandler<DeletePersonCommand>
{
    public async Task Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        if(request.Id == Guid.Empty)
        {
            throw new BadRequestException(ErrorMessages.PersonIdShouldNotEmpty);
        }    
        var person = await personRepository.GetByIdAsync(request.Id);
        if (person is null)
        {
            throw new NotFoundException(string.Format(ErrorMessages.PersonNotFoundById, request.Id));
        }
        personRepository.Delete(person);

        logger.LogInformation(LoggingTemplateMessages.PersonDeletedWithIdSuccess, request.Id);

        await personRepository.UnitOfWork.SaveChangesAsync();
    }
}
