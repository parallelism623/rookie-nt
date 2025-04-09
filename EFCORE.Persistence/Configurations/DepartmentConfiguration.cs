using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments")
               .HasIndex(d => d.Id);

        builder.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasData(
            new
            {
                Id = new Guid("9438216b-b2e7-4cbc-b684-a9ae5702259b"),
                Name = "Software Development"
            },
            new
            {
                Id = new Guid("d3f19cce-7846-43b9-8ba7-0bb3e1a3a818"),
                Name = "Finance"
            },
            new
            {
                Id = new Guid("935852bc-3b30-4003-85fe-cfd5d4198b23"),
                Name = "Accountant"
            },
            new
            {
                Id = new Guid("2a31296e-c29d-470f-a529-322e6218013c"),
                Name = "HR"
            }
        );
    }
}