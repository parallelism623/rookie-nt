using mvc_todolist.Models.DbContexts;
using mvc_todolist.Models.Entities;
using mvc_todolist.Models.ModelViews;
using mvc_todolist.Repositories.Interfaces;

namespace mvc_todolist.Repositories.Implements
{
    public class PersonRepository : RepositoryBase<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
