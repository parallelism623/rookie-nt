using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public class ProjectEmployeeConfiguration : AuditableEntityConfiguration<ProjectEmployee>, 
                                                IEntityTypeConfiguration<ProjectEmployee>
{
    public override void Configure(EntityTypeBuilder<ProjectEmployee> builder)
    {
        builder.HasKey(pe => pe.Id);
        builder.HasIndex(pe => pe.EmployeeId);
        builder.HasIndex(pe => pe.ProjectId);
        builder.HasIndex(pe => new { pe.EmployeeId, pe.ProjectId });

        builder.HasOne(pe => pe.Employee)
               .WithMany(e => e.ProjectEmployees)
               .HasForeignKey(pe => pe.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pe => pe.Project)
               .WithMany(p => p.ProjectEmployees)
               .HasForeignKey(pe => pe.ProjectId)
               .OnDelete(DeleteBehavior.Cascade);
        base.Configure(builder);
    }
}