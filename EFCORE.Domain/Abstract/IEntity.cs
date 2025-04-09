namespace EFCORE.Domain.Abstract;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}