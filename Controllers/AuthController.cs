using aspnetcore.Commons;
using aspnetcore.Commons.Models;
using aspnetcore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.Minimals
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;
        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async ValueTask<Result<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var response = await _authService.LoginAsync(loginRequest);
            return Result<LoginResponse>.Create(response, "Đăng nhập thành công");
        }

    }

}



