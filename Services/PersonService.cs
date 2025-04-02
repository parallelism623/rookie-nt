using Mapster;
using mvc_todolist.Commons;
using mvc_todolist.Models.Entities;
using mvc_todolist.ModelViews;
using mvc_todolist.Repositories.Interfaces;

namespace mvc_todolist.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {   
            _unitOfWork = unitOfWork;  
        }

        public async Task CreatePersonAsync(PersonViewModel person)
        {
            
            _unitOfWork.PersonRepository.Add(person.Adapt<Person>());
            await _unitOfWork.SaveChangesAsync();
            
        }

        public async Task<PersonViewModel> GetOldestPersonAsync()
        {
            var oldestPerson = (await _unitOfWork.PersonRepository.GetAsync(orderBy: p => p.OrderBy(p => p.Age))).FirstOrDefault();

            var personViewModels = oldestPerson.Adapt<PersonViewModel>();
            return personViewModels;
        }

        public async Task<List<PersonViewModel>> GetPersonAsync(QueryParameters<Person> queryParameters)
        {
            var persons = await _unitOfWork.PersonRepository.GetAsync(filter: queryParameters.FilterExpression());

            return persons.Adapt<List<PersonViewModel>>();
        }

        public async Task<PersonViewModel> GetPersonByIdAsync(Guid id)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
            return person.Adapt<PersonViewModel>();
        }

        public async Task RemovePerson(Guid id)
        {
            var person = await _unitOfWork.PersonRepository.GetByIdAsync(id);
            _unitOfWork.PersonRepository.Remove(person);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePerson(PersonViewModel person)
        {
            _unitOfWork.PersonRepository.Update(person.Adapt<Person>());
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
