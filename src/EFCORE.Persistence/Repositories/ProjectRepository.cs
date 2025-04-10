
using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Persistence.Repositories;

public class ProjectRepository : BaseRepository<Project, Guid>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {

    }
    public Task<List<Project>?> GetByIdsAsync(List<Guid> ids)
    {
        return _context.Projects?.AsNoTracking().Where(e => ids.Contains(e.Id))?.ToListAsync()!;
    }
}
