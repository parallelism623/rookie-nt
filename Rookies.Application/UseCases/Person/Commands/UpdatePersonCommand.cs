using Rookies.Contract.Exceptions;
using Rookies.Contract.Messages;
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
        if(request.Model.Id == null)
        {
            throw new BadRequestException(ErrorMessages.PersonIdRequire);
        }
        var person = await personRepository.GetByIdAsync(request.Model.Id ?? Guid.Empty);

        if (person == null)
        {
                throw new BadRequestException(string.Format(ErrorMessages.PersonNotFoundById, request.Model.Id));
        }
        person = request.Model.Adapt(person);
        personRepository.Update(person);

        logger.LogInformation(LoggingTemplateMessages.PersonUpdatedWithDataSuccess, person.Id,  request.Model);

        await personRepository.UnitOfWork.SaveChangesAsync();
    }
}
