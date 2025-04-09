using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public class ProjectEmployeeConfiguration : IEntityTypeConfiguration<ProjectEmployee>
{
    public void Configure(EntityTypeBuilder<ProjectEmployee> builder)
    {
        builder.HasKey(pe => new { pe.EmployeeId, pe.ProjectId });
        builder.HasIndex(pe => new { pe.EmployeeId, pe.ProjectId });
        builder.HasOne(pe => pe.Employee)
               .WithMany(e => e.ProjectEmployees)
               .HasForeignKey(pe => pe.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(pe => pe.Project)
               .WithMany(p => p.ProjectEmployees)
               .HasForeignKey(pe => pe.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}