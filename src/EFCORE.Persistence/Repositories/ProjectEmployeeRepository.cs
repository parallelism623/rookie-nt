
using EFCORE.Domain.Entities;
using EFCORE.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Persistence.Repositories;

public class ProjectEmployeeRepository : BaseRepository<ProjectEmployee, Guid>, IProjectEmployeeRepository
{
    public ProjectEmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<bool> IsExistsAsync(Guid employeeId, Guid projectId)
    {
        return _context.ProjectEmployees
            .AnyAsync(pe => pe.EmployeeId == employeeId && pe.ProjectId == projectId);
    }
}

