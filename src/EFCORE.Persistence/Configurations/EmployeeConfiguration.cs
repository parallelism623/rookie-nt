using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public class EmployeeConfiguration : AuditableEntityConfiguration<Employee>, IEntityTypeConfiguration<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasOne(e => e.Department)
               .WithMany(e => e.Employees)
               .HasForeignKey(e => e.DepartmentId)
               .OnDelete(DeleteBehavior.Cascade);
        base.Configure(builder);
    }
}