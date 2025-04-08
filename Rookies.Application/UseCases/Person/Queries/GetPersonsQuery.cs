using Rookies.Contract.Messages;
using Rookies.Contract.Models;
using Rookies.Contract.Shared;
using Rookies.Domain.Repositories;

namespace Rookies.Application.UseCases.Persons.Queries;
public record GetPersonsQuery(PersonQueryParameters QueryParameters) 
    : IRequest<PagingResult<PersonResponseModel>>
{ }

public class GetPersonsQueryHandler(IPersonRepository personRepository,
                                    ILogger<GetPersonsQueryHandler> logger)
    : IRequestHandler<GetPersonsQuery, PagingResult<PersonResponseModel>>
{

    public async Task<PagingResult<PersonResponseModel>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
    {
        var persons = await personRepository.GetAsync(request.QueryParameters);

        logger.LogInformation(LoggingTemplateMessages.PersonQuerySuccess, persons.TotalCount);

        return persons.Adapt<PagingResult<PersonResponseModel>>();
    }
}
