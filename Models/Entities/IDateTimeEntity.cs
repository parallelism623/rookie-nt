namespace mvc_todolist.Models.Entities;

public interface IDateTimeEntity
{
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid CreateBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}
