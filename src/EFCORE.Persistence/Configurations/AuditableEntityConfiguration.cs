
using EFCORE.Domain.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCORE.Persistence.Configurations;

public abstract class AuditableEntityConfiguration<T>
    where T : AuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(a => a.CreatedAt)
               .ValueGeneratedOnAdd();
        builder.Property(a => a.ModifiedAt)
               .ValueGeneratedOnAddOrUpdate();  
    }
}
