using Rookies.Application.Services.Crypto;
using Rookies.Contract.Messages;
using Rookies.Contract.Models;
using Rookies.Contract.Shared;
using Rookies.Domain.Entities;
using Rookies.Domain.Repositories;
using Rookies.Infrastructure.Services.Crypto;

namespace Rookies.Application.UseCases.Persons.Commands;
public record CreatePersonCommand(PersonCreateRequestModel Model) : IRequest<Result<string>> { }


public class CreatePersonCommandHandler(IPersonRepository personRepository,
                                        ILogger<CreatePersonCommandHandler> logger,
                                        ICryptoServiceStrategy cryptoService)
                                        : IRequestHandler<CreatePersonCommand, Result<string>>
{

    public async Task<Result<string>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            SetEncryptionAlgorithm(CryptoAlgorithm.RSA);

            var person = request.Model.Adapt<Person>();

            EncryptPersonInfo(person);

            logger.LogInformation(LoggingTemplateMessages.PersonCreatedWithIdSuccess, person.Id);

            await personRepository.UnitOfWork.SaveChangesAsync();

            return Result<string>.Success(ResponseMessages.PersonCreatedSuccess);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result<string>.Failure(ex.Message, 500);
        }
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
