using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
{
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.ToTable("Salary")
               .HasIndex(s => s.Id);

        builder.Property(s => s.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();
        builder.HasIndex(s => s.EmployeeId);
        builder.HasOne(s => s.Employee)
               .WithOne(e => e.Salary)
               .HasForeignKey<Salary>(s => s.Id)
               .OnDelete(DeleteBehavior.Cascade);
    }
}