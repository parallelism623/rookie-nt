using mvc_todolist.Repositories.Implements;

namespace mvc_todolist.Repositories.Interfaces;

public interface IUnitOfWork
{
    IPersonRepository PersonRepository { get; }
    Task SaveChangesAsync();
    void SaveChange();
}