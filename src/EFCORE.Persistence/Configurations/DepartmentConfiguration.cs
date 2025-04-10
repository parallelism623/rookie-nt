using EFCORE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EFCORE.Persistence.Configurations;

public class DepartmentConfiguration : AuditableEntityConfiguration<Department>, IEntityTypeConfiguration<Department>
{
    public override void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.Property(d => d.Name)
               .IsRequired()
        .HasMaxLength(100);

        builder.HasData(
            new
            {
                Id = new Guid("9438216b-b2e7-4cbc-b684-a9ae5702259b"),
                Name = "Software Development",
                CreatedAt = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = new Guid("1d3c3a1f-2d45-4f73-92fd-8c0797cdf84f")
            },
            new
            {
                Id = new Guid("d3f19cce-7846-43b9-8ba7-0bb3e1a3a818"),
                Name = "Finance",
                CreatedAt = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = new Guid("e2c4e2c3-3f57-4f83-92ad-9c1797cdf84f")
            },
            new
            {
                Id = new Guid("935852bc-3b30-4003-85fe-cfd5d4198b23"),
                Name = "Accountant",
                CreatedAt = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = new Guid("f3a5d4b7-4d67-4e93-82bd-0c2797cdf84f")
            },
            new
            {
                Id = new Guid("2a31296e-c29d-470f-a529-322e6218013c"),
                Name = "HR",
                CreatedAt = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = new Guid("a4b6f5c8-5e77-4f83-73ad-1d3797cdf84f")
            }
        );

        base.Configure(builder);
    }
}