namespace EFCORE.Domain.Abstract;
public interface IUnitOfWork
{
    Task SaveChangesEntitiesAsync();
}
