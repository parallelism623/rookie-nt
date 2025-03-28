using aspnetcore.Models;

namespace aspnetcore.Repositories.Interfaces
{
    public interface IAuthRepository : IRepositoryBase<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
