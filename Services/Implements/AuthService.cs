using aspnetcore.Commons.Exceptions;
using aspnetcore.Commons.Models;
using aspnetcore.Commons.Resources;
using aspnetcore.Commons.Validators;
using aspnetcore.Models;
using aspnetcore.Repositories.Interfaces;
using aspnetcore.Services.Interfaces;
using System.Globalization;
using System.Text;
using System.Text.Json;
namespace aspnetcore.Services.Implements
{
    public class AuthService : IAuthServices
    {
        private readonly IAuthRepository _authRepository;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IAuthRepository authRepository, 
            ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _logger = logger;
        }

        public async ValueTask<LoginResponse> LoginAsync(LoginRequest request)
        {
            Console.WriteLine(CultureInfo.CurrentUICulture.Name);
            UserValidator.ValidateLoginRequest(request);
            var user = await _authRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new BadRequestException(Labels.UsernamePasswordIncorrect);
            }
            if(user.Password != request.Password)
            {
                throw new BadRequestException(Labels.UsernamePasswordIncorrect);
            }
            _logger.LogInformation("User information: {@UserInfo}", user);
            return new LoginResponse
            {
                AccessToken = GetAccessToken(user),
                FullName = user.FirstName + user.LastName,
                PId = Guid.NewGuid().ToString(),
                CreateBy = user.CreateBy,
                CreatedDate = user.CreatedDate,
                ModifiedBy = user.ModifiedBy,
                ModifiedDate = user.ModifiedDate,
                Email = user.Email,
                Address = user.Address,
            };
        }

        private string GetAccessToken(User user)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
