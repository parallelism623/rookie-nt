using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee")
               .HasIndex(e => e.Id);
        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(100);
        builder.HasOne(e => e.Department)
               .WithMany(e => e.Employees)
               .HasForeignKey(e => e.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}