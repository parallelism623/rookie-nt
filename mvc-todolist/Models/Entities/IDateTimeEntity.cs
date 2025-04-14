namespace mvc_todolist.Models.Entities;

public interface IDateTimeEntity
{
    public DateTime CreateAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid CreateBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}
