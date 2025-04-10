
using EFCORE.Domain.Abstract;
using EFCORE.Domain.Entities;

namespace EFCORE.Persistence.Repositories;

public class DepartmentRepository : BaseRepository<Department, Guid>, IDepartmentRepository
{
    public DepartmentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
