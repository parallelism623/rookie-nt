using Rookies.Contract.Messages;
using Rookies.Contract.Shared;
using Rookies.Domain.Repositories;

namespace Rookies.Application.UseCases.Persons.Commands;
public record DeletePersonCommand(Guid Id) : IRequest<Result<string>> { }

public class PersonDeleteCommandHandler(IPersonRepository personRepository,
                                        ILogger<PersonDeleteCommandHandler> logger)
                                        : IRequestHandler<DeletePersonCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Id == Guid.Empty)
            {
                logger.LogError(ErrorMessages.PersonIdShouldNotEmpty);
                return Result<string>.Failure(ErrorMessages.PersonIdShouldNotEmpty, 400);
            }
            var person = await personRepository.GetByIdAsync(request.Id);
            if (person is null)
            {
                var messageError = string.Format(ErrorMessages.PersonNotFoundById, request.Id);
                logger.LogError(messageError);
                return Result<string>.Failure(messageError, 404);
            }
            personRepository.Delete(person);

            logger.LogInformation(LoggingTemplateMessages.PersonDeletedWithIdSuccess, request.Id);

            await personRepository.UnitOfWork.SaveChangesAsync();

            return Result<string>.Success(ResponseMessages.PersonDeletedSuccess, 200);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result<string>.Failure(ex.Message, 500);
        }
    }
}