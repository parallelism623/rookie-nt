

using aspnetcore.Commons.Exceptions;
using aspnetcore.Commons.Models;
using aspnetcore.Commons.Resources;
using System.Text.RegularExpressions;

namespace aspnetcore.Commons.Validators
{
    public static class UserValidator
    {
        private static string _patternUsername = @"^[a-zA-Z]+$";
        private static string _patternPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_=+-]).{8,32}$";
        public static void ValidateLoginRequest(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new BadRequestException(Labels.UsernameOrPasswordEmpty);
            }
            if (request.Password.Length < 8 || request.Password.Length > 32)
            {
                throw new BadRequestException(Labels.PasswordLengthShouldInRange);
            }
            if (!Regex.IsMatch(request.Username, _patternUsername))
            {
                throw new BadRequestException(Labels.UsernameInvalid);
            }
            if (!Regex.IsMatch(request.Password, _patternPassword))
            {
                throw new BadRequestException(Labels.PasswordShouldBePattern);
            }
        }
    }
}
