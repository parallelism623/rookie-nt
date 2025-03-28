using aspnetcore.Commons.Models;
using aspnetcore.Minimals;

namespace aspnetcore.Services.Interfaces
{
    public interface IAuthServices
    {
        ValueTask<LoginResponse> LoginAsync(LoginRequest request);
    }
}
