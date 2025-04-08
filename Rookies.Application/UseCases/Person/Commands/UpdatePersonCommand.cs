using Rookies.Application.Services.Crypto;
using Rookies.Contract.Exceptions;
using Rookies.Contract.Messages;
using Rookies.Contract.Models;
using Rookies.Domain.Entities;
using Rookies.Domain.Repositories;
using Rookies.Infrastructure.Services.Crypto;

namespace Rookies.Application.UseCases.Persons.Commands;

public record UpdatePersonCommand(PersonUpdateRequestModel Model) : IRequest { }


public class UpdatePersonCommandHandler(IPersonRepository personRepository,
                                        ILogger<UpdatePersonCommandHandler> logger,
                                        ICryptoServiceStrategy cryptoServiceStrategy) 
                                        : IRequestHandler<UpdatePersonCommand>
{

    public async Task Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        SetEncryptionAlgorithm(CryptoAlgorithm.RSA);
        if (request.Model.Id == null)
        {
            throw new BadRequestException(ErrorMessages.PersonIdRequire);
        }
        var person = await personRepository.GetByIdAsync(request.Model.Id ?? Guid.Empty);

        if (person == null)
        {
                throw new BadRequestException(string.Format(ErrorMessages.PersonNotFoundById, request.Model.Id));
        }
        person = request.Model.Adapt(person);

        EncryptPersonInfo(person);

        personRepository.Update(person);

        logger.LogInformation(LoggingTemplateMessages.PersonUpdatedWithDataSuccess, person.Id,  request.Model);

        await personRepository.UnitOfWork.SaveChangesAsync();
    }
    private void SetEncryptionAlgorithm(string algo)
    {
        cryptoServiceStrategy.SetCryptoAlgorithm(algo);
    }


    private void EncryptPersonInfo(Person person)
    {
        person.PhoneNumber = cryptoServiceStrategy.Encrypt(person.PhoneNumber);
        person.Email = cryptoServiceStrategy.Encrypt(person.Email);
    }
}
