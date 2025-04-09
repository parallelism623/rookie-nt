using EFCORE.Domain.Abstract;

namespace EFCORE.Domain.Entities
{
    public class Department : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public virtual ICollection<Employee> Employees { get; set; } = default!;
    }
}