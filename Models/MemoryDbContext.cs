namespace aspnetcore.Models
{
    public class MemoryDbContext
    {
        public List<User> Users { get; set; } = GenerateUsers();
        private static List<User> GenerateUsers()
        {
            var result =  Enumerable.Range(1, 10).Select(i => new User
            {
                Id = Guid.NewGuid(),
                Username = $"user{i}",
                Password = $"password{i}",
                Email = $"user{i}@example.com",
                Address = i % 2 == 0 ? $"Address {i}" : null, 
                FirstName = $"First{i}",
                LastName = $"Last{i}",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = i % 3 == 0 ? DateTime.UtcNow.AddDays(-i) : null, 
                CreateBy = Guid.NewGuid(),
                ModifiedBy = i % 3 == 0 ? Guid.NewGuid() : null
            }).ToList();
            result.Add(new User
            {
                Id = Guid.NewGuid(),
                Username = $"admin",
                Password = $"admin123*A",
                Email = $"admin@rookie-nt.com",
                Address = "239 Xuan Thuy street, Cau Giay, Ha Noi",
                FirstName = $"Anh Hieu",
                LastName = $"Truong",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                CreateBy = Guid.NewGuid(),
                ModifiedBy = Guid.NewGuid()
            });
            return result;
        }
    }
}
