namespace aspnetcore.Commons.Models
{
    public class LoginResponse
    {
        public string? AccessToken { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }

        public string? Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreateBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string PId { get; set; } = Guid.NewGuid().ToString();
    }
    public class LoginRequest
    {
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
