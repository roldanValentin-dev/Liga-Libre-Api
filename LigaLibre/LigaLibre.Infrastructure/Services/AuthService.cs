
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LigaLibre.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var isValidPassword = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!isValidPassword)
                throw new UnauthorizedAccessException("Invalid email or password");

            var token = await GenerateJwtTokenAsync(user.Email!);
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles
            };

        }
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto register)
        {
            var existingUser = await _userManager.FindByEmailAsync(register.Email);
            if (existingUser != null)
                throw new UnauthorizedAccessException("User already exists");

            var user = new ApplicationUser
            {
                Email = register.Email,
                UserName = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
                throw new ArgumentException(string.Join(", ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");

            var token = await GenerateJwtTokenAsync(user.Email!);
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles
            };

        }
        public async Task<string> GenerateJwtTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("User not found.");
            // Lógica para generar el token JWT
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Email, user.Email!),
                new (ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new (ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
            };

            // Aquí agregarías la lógica para crear y firmar el token JWT usando los claims
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
