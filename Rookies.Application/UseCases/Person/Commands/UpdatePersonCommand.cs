using Microsoft.Extensions.Logging;
using Rookies.Application.Services.Crypto;
using Rookies.Contract.Exceptions;
using Rookies.Contract.Messages;
using Rookies.Contract.Models;
using Rookies.Contract.Shared;
using Rookies.Domain.Entities;
using Rookies.Domain.Repositories;
using Rookies.Infrastructure.Services.Crypto;

namespace Rookies.Application.UseCases.Persons.Commands;

public record UpdatePersonCommand(PersonUpdateRequestModel Model) : IRequest<Result<string>> { }


public class UpdatePersonCommandHandler(IPersonRepository personRepository,
                                        ILogger<UpdatePersonCommandHandler> logger,
                                        ICryptoServiceStrategy cryptoServiceStrategy) 
                                        : IRequestHandler<UpdatePersonCommand, Result<string>>
{

    public async Task<Result<string>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            SetEncryptionAlgorithm(CryptoAlgorithm.RSA);
            if (request.Model.Id == null)
            {
                logger.LogError(ErrorMessages.PersonIdRequire);
                return Result<string>.Failure(ErrorMessages.PersonIdRequire);
            }
            var person = await personRepository.GetByIdAsync(request.Model.Id ?? Guid.Empty);

            if (person == null)
            {
                var messageError = string.Format(ErrorMessages.PersonNotFoundById, request.Model.Id);
                logger.LogError(messageError);
                return Result<string>.Failure(messageError, 404);
            }
            person = request.Model.Adapt(person);

            EncryptPersonInfo(person);

            personRepository.Update(person);

            logger.LogInformation(LoggingTemplateMessages.PersonUpdatedWithDataSuccess, person.Id, request.Model);

            await personRepository.UnitOfWork.SaveChangesAsync();

            return Result<string>.Success(ResponseMessages.PersonUpdatedSuccess);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result<string>.Failure(ex.Message, 500);
        }
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
