
namespace aspnetcore.Models
{
    public class User : IEntity, IDatetimeEntity
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Address { get; set; } = default;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateTime CreatedDate { get; set; } = default!;
        public DateTime? ModifiedDate { get; set; } = default;
        public Guid CreateBy { get; set; } = default!;
        public Guid? ModifiedBy { get; set; } = default;
        public Guid Id { get; set; } = default!;
    }
}
