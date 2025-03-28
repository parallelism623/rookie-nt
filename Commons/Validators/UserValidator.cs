

using aspnetcore.Commons.Exceptions;
using aspnetcore.Commons.Models;
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
                throw new BadRequestException("Tên đăng nhập hoặc mật khẩu trống");
            } 
            if(request.Password.Length < 8 || request.Password.Length > 32)
            {
                throw new BadRequestException("Độ dài mật khẩu từ 8 - 32 kí tự");
            }   
            if(!Regex.IsMatch(request.Username, _patternUsername))
            {
                throw new BadRequestException("Tên đăng nhập không hợp lệ (a-zA-Z)");
            }    
            if(!Regex.IsMatch(request.Password, _patternPassword))
            {
                throw new BadRequestException("Mật khẩu phải chứa ít nhất một kí tự thường, kí tự viết hoa, số và kí tự đặc biệt");
            }    
        }
    }
}
