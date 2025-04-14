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
    public int Gender { get; set; }
    public DateTime CreateAt { get; set; }
    public Guid CreateBy { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int BirthYear => DateOfBirth.Year;
    public bool Graduated { get; set; } 
    public int Age => DateTime.Now.Year - DateOfBirth.Year;

}