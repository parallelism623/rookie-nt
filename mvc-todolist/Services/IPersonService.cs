using mvc_todolist.Commons;
using mvc_todolist.Models.Entities;
using mvc_todolist.ModelViews;

namespace mvc_todolist.Services
{
    public interface IPersonService
    {
        Task CreatePersonAsync(PersonViewModel person);
        Task<List<PersonViewModel>?> GetPersonAsync(QueryParameters<Person> queryParameters);

        Task<PersonViewModel?> GetOldestPersonAsync();

        Task<PersonViewModel?> GetPersonByIdAsync(Guid id);

        Task UpdatePerson(PersonViewModel person);

        Task RemovePerson(Guid id);

    }
}
