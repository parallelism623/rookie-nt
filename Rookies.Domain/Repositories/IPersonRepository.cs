using Rookies.Contract.Models;
using Rookies.Contract.Shared;
using Rookies.Domain.Abstractions;
using Rookies.Domain.Entities;

namespace Rookies.Domain.Repositories;

public interface IPersonRepository : IBaseRepository<Person, Guid>
{
    Task<PagingResult<Person>> GetAsync(PersonQueryParameters queryParameters);

    Task<Person?> GetByEmailAsync(string email);
}