using Microsoft.EntityFrameworkCore;
using mvc_todolist.Models.DbContexts;
using mvc_todolist.Repositories.Interfaces;

namespace mvc_todolist.Repositories.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IPersonRepository? _personRepository;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IPersonRepository PersonRepository
        {
            get
            {
                if (_personRepository == null)
                {
                    _personRepository = new PersonRepository(_context);
                }
                return _personRepository;
            }
        }
        public void SaveChange()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
