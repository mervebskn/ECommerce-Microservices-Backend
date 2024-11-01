using AuthService.Abstractions;
using AuthService.Entities;
using AuthService.Repositories;
using AuthService.Utils;
using Common.DTOs;
using Common.Exceptions;
using Common.Utils;
using System.Security.Claims;

namespace AuthService.Services
{
    public class AuthenticationService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtUtils _jwtUtils;

        public AuthenticationService(IUserRepository userRepository, JwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetByUsername(registerDto.Username);
            if (existingUser != null)
                throw new AuthException("Username already exists.", 400);

            var user = new User
            {
                Username = registerDto.Username,
                // PasswordHasher kullanarak basit bir hash oluşturma
                PasswordHash = PasswordHasher.HashPassword(registerDto.Password),
                Email = registerDto.Email
            };

            await _userRepository.Create(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            return _jwtUtils.GenerateToken(user.Username, claims);
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetByUsername(loginDto.Username);
            if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
                throw new AuthException("Invalid credentials.", 401);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            return _jwtUtils.GenerateToken(user.Username, claims);
        }
    }
}
