namespace Rookies.Domain;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync();
}