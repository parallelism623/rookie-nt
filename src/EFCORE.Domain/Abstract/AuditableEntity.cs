
namespace EFCORE.Domain.Abstract;

public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; } = null;
    public Guid CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; } = null;
}
