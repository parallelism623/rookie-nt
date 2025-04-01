using mvc_todolist.Models.Entities;

namespace mvc_todolist.Models.ModelViews;

public class PersonViewModel : IEntity, IDateTimeEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string FullName => LastName + " " + FirstName;
    public int Gender { get; set; }
    public DateTime DateOfBirth { get;set;}
    public int BirthYear { get; set; }
    public int Age { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid CreateBy { get; set; }
    public Guid? ModifiedBy { get; set; }
}