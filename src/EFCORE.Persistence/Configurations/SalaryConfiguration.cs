using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public class SalaryConfiguration : AuditableEntityConfiguration<Salary>, IEntityTypeConfiguration<Salary>
{
    public override void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.ToTable("Salary");

        builder.Property(s => s.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();
        builder.HasIndex(s => s.EmployeeId);

        builder.HasOne(s => s.Employee)
               .WithOne(e => e.Salary)
               .HasForeignKey<Salary>(s => s.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
        base.Configure(builder);
    }
}