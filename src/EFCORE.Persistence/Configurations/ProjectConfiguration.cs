using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public class ProjectConfiguration : AuditableEntityConfiguration<Project>, IEntityTypeConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Project");

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        base.Configure(builder);
    }
}