using mvc_todolist.Models.Entities;

namespace mvc_todolist.ModelViews;

public class PersonViewModel
{
    public Guid Id { get; set; }
   
    public string Username { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string FullName => LastName + " " + FirstName;
    public string Password { get; set; } = default!;
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public Guid CreateBy { get; set; } = Guid.NewGuid();
    public int Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int BirthYear => DateOfBirth.Year;
    public int Age => DateTime.Now.Year - DateOfBirth.Year;
}