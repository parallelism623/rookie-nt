using Rookies.Contract.Models;
using Rookies.Contract.Shared;
using Rookies.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Persistence.Repositories;
public class PersonRepository : BaseRepository<Person, Guid>, IPersonRepository
{
    private readonly RookiesDbContext _context;
    public PersonRepository(RookiesDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PagingResult<Person>> GetAsync(PersonQueryParameters queryParameters)
    {
        IQueryable<Person> personsQueryable = _context.Persons.AsNoTracking();
        personsQueryable = FilterPersonQueryable(personsQueryable, queryParameters);
        
        int totalCount = await personsQueryable.CountAsync();

        var persons = await personsQueryable
                        .Skip(queryParameters.PageSize * (queryParameters.PageIndex - 1))
                        .Take(queryParameters.PageSize)
                        .ToListAsync();
        
        return new() { Items = persons, 
            PageIndex = queryParameters.PageIndex, 
            PageSize = Math.Min(persons.Count, queryParameters.PageSize), 
            TotalCount = totalCount };
    }

    private static IQueryable<Person> FilterPersonQueryable(IQueryable<Person> persons, PersonQueryParameters queryParameters)
    {
        if (!string.IsNullOrEmpty(queryParameters.Name))
        {
            persons = persons.Where(p => p.FirstName.Contains(queryParameters.Name)
                                      || p.LastName.Contains(queryParameters.Name));
        }
        if (queryParameters.Gender != null)
        {
            persons = persons.Where(p => p.Gender == queryParameters.Gender);
        }
        if (!string.IsNullOrEmpty(queryParameters.BirthPlace))
        {
            persons = persons.Where(p => p.BirthPlace.Contains(queryParameters.BirthPlace));
        }
        return persons;
    }


}
