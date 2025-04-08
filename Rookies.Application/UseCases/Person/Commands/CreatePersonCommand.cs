using Rookies.Application.Services.Crypto;
using Rookies.Contract.Messages;
using Rookies.Contract.Models;
using Rookies.Domain.Entities;
using Rookies.Domain.Repositories;
using Rookies.Infrastructure.Services.Crypto;

namespace Rookies.Application.UseCases.Persons.Commands;
public record CreatePersonCommand(PersonCreateRequestModel Model) : IRequest { }


public class CreatePersonCommandHandler(IPersonRepository personRepository,
                                        ILogger<CreatePersonCommandHandler> logger,
                                        ICryptoServiceStrategy cryptoService)
                                        : IRequestHandler<CreatePersonCommand>
{

    public async Task Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        SetEncryptionAlgorithm(CryptoAlgorithm.RSA);

        var person = request.Model.Adapt<Person>();

        EncryptPersonInfo(person);

        logger.LogInformation(LoggingTemplateMessages.PersonCreatedWithIdSuccess, person.Id);

        await personRepository.UnitOfWork.SaveChangesAsync();
    }

    private void SetEncryptionAlgorithm(string algo)
    {
        cryptoService.SetCryptoAlgorithm(algo);
    }


    private void EncryptPersonInfo(Person person)
    {
        person.PhoneNumber = cryptoService.Encrypt(person.PhoneNumber);
        person.Email = cryptoService.Encrypt(person.Email);
    }
}
