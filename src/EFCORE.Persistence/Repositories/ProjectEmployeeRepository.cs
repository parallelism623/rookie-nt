
using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;

namespace EFCORE.Persistence.Repositories;

public class ProjectEmployeeRepository : BaseRepository<ProjectEmployee, Guid>, IProjectEmployeeRepository
{
    public ProjectEmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }
}

