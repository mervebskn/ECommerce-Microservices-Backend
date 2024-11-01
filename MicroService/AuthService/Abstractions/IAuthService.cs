using Common.DTOs;

namespace AuthService.Abstractions
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
    }
}
