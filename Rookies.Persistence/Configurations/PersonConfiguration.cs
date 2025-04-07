using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookies.Persistence.Configurations;
public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.CreatedAt)
               .HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.ModifiedAt)
               .HasDefaultValueSql("GETDATE()");
        builder.Property(p => p.FullName)
               .HasComputedColumnSql("CONCAT(FirstName, ' ', LastName)");
    }
}
