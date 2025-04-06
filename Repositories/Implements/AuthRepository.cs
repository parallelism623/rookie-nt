using aspnetcore.Models;
using aspnetcore.Repositories.Interfaces;
using System.Security.Cryptography;

namespace aspnetcore.Repositories.Implements
{
    public class AuthRepository : IAuthRepository
    {
        private readonly MemoryDbContext _memoryDbContext;
        public AuthRepository(MemoryDbContext memoryDbContext)
        {
            _memoryDbContext = memoryDbContext;
        }

        public async Task Add(User entity)
        {
            await Task.Delay(GetRandomMiliSecondsDelay());
            _memoryDbContext.Users.Append(entity);
        }

        public async Task Delete(User user)
        {
            await Task.Delay(GetRandomMiliSecondsDelay());
   
            _memoryDbContext.Users.Remove(user);
                
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            await Task.Delay(GetRandomMiliSecondsDelay());
            return _memoryDbContext.Users;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            await Task.Delay(GetRandomMiliSecondsDelay());
            return _memoryDbContext.Users?.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            await Task.Delay(GetRandomMiliSecondsDelay());
            return _memoryDbContext.Users?.FirstOrDefault(u => u.Username == username);
        }

        public async Task Update(User entity)
        {
            await Task.Delay(GetRandomMiliSecondsDelay());
            var user = _memoryDbContext.Users.FirstOrDefault(u => u.Id == entity.Id);
            user = entity; 
        }

        private int GetRandomMiliSecondsDelay()
        {
            return RandomNumberGenerator.GetInt32(1, 1001);
        }
      
    }
}
