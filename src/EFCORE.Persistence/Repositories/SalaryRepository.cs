
using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;

namespace EFCORE.Persistence.Repositories;

public class SalaryRepository : BaseRepository<Salary, Guid>, ISalaryRepository
{
    public SalaryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
