using Rookies.Application.Services.Crypto;
using Rookies.Contract.Messages;
using Rookies.Contract.Models;
using Rookies.Contract.Shared;
using Rookies.Domain.Entities;
using Rookies.Domain.Repositories;
using Rookies.Infrastructure.Services.Crypto;

namespace Rookies.Application.UseCases.Persons.Queries;
public record GetPersonsQuery(PersonQueryParameters QueryParameters) 
    : IRequest<Result<PagingResult<PersonResponseModel>>>
{ }

public class GetPersonsQueryHandler(IPersonRepository personRepository,
                                    ILogger<GetPersonsQueryHandler> logger,
                                    ICryptoServiceStrategy cryptoServiceStrategy)
    : IRequestHandler<GetPersonsQuery, Result<PagingResult<PersonResponseModel>>>
{
    public async Task<Result<PagingResult<PersonResponseModel>>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            SetEncryptionAlgorithm(CryptoAlgorithm.RSA);

            var persons = await personRepository.GetAsync(request.QueryParameters);

            DecryptPersonsInfo(persons.Items);
            logger.LogInformation(LoggingTemplateMessages.PersonQuerySuccess, persons.TotalCount);


            return Result<PagingResult<PersonResponseModel>>.Success(persons.Adapt<PagingResult<PersonResponseModel>>());
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new(null ,500, false, new Error("ServerInternalError", ex.Message));
        }
    }
    private void SetEncryptionAlgorithm(string algo)
    {
        cryptoServiceStrategy.SetCryptoAlgorithm(algo);
    }


    private void DecryptPersonsInfo(IEnumerable<Person>? persons)
    {
        if(persons == null || !persons.Any())
        {
            return;
        }
        foreach(var p in persons)
        {
            DecryptPersonInfo(p);
        }
    }

    private void DecryptPersonInfo(Person person)
    {
        person.PhoneNumber = cryptoServiceStrategy.Decrypt(person.PhoneNumber);
        person.Email = cryptoServiceStrategy.Decrypt(person.Email);
    }

}
