namespace mvc_todolist.Models.Entities
{
    public class Person : IEntity, IDateTimeEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Guid CreateBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public int Gender { get; set; }  
        public int Age { get; set; }
        public bool Graduated { get; set; }
        public DateTime DateOfBirth { get; set; } = default!;
        public int BirthYear { get; set; }
        public string GetGenderString => Enum.GetName(typeof(Gender), Gender) ?? default!;

    }
}
