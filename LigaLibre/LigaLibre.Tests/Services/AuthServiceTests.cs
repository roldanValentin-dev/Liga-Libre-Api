using LigaLibre.Application.DTOs;
using LigaLibre.Domain.Entities;
using LigaLibre.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LigaLibre.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _mockConfiguration = new Mock<IConfiguration>();
        
        _mockConfiguration.Setup(c => c["JWT:Key"]).Returns("SuperSecretKeyForJWTTokenGeneration123456");
        _mockConfiguration.Setup(c => c["JWT:Issuer"]).Returns("TestIssuer");
        _mockConfiguration.Setup(c => c["JWT:Audience"]).Returns("TestAudience");
        
        _service = new AuthService(_mockUserManager.Object, _mockConfiguration.Object);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResponse()
    {
        var user = new ApplicationUser { Email = "test@test.com", FirstName = "John", LastName = "Doe", Id = "1" };
        var loginDto = new LoginDto { Email = "test@test.com", Password = "Password123!" };
        
        _mockUserManager.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(true);
        _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

        var result = await _service.LoginAsync(loginDto);

        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task LoginAsync_InvalidEmail_ThrowsUnauthorizedException()
    {
        var loginDto = new LoginDto { Email = "invalid@test.com", Password = "Password123!" };
        _mockUserManager.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync((ApplicationUser?)null);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(loginDto));
    }

    [Fact]
    public async Task LoginAsync_InvalidPassword_ThrowsUnauthorizedException()
    {
        var user = new ApplicationUser { Email = "test@test.com" };
        var loginDto = new LoginDto { Email = "test@test.com", Password = "WrongPassword" };
        
        _mockUserManager.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.LoginAsync(loginDto));
    }

    [Fact]
    public async Task RegisterAsync_ValidData_ReturnsAuthResponse()
    {
        var registerDto = new RegisterDto { Email = "new@test.com", Password = "Password123!", FirstName = "Jane", LastName = "Smith" };
        var user = new ApplicationUser { Email = "new@test.com", FirstName = "Jane", LastName = "Smith", Id = "2" };
        
        _mockUserManager.SetupSequence(u => u.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync((ApplicationUser?)null)
            .ReturnsAsync(user);
        _mockUserManager.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), registerDto.Password)).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), "User")).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(u => u.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string> { "User" });

        var result = await _service.RegisterAsync(registerDto);

        Assert.NotNull(result);
        Assert.Equal("new@test.com", result.Email);
        Assert.NotEmpty(result.Token);
    }

    [Fact]
    public async Task RegisterAsync_ExistingUser_ThrowsUnauthorizedException()
    {
        var user = new ApplicationUser { Email = "existing@test.com" };
        var registerDto = new RegisterDto { Email = "existing@test.com", Password = "Password123!" };
        
        _mockUserManager.Setup(u => u.FindByEmailAsync(registerDto.Email)).ReturnsAsync(user);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.RegisterAsync(registerDto));
    }

    [Fact]
    public async Task GenerateJwtTokenAsync_ValidEmail_ReturnsToken()
    {
        var user = new ApplicationUser { Email = "test@test.com", FirstName = "John", LastName = "Doe", Id = "1" };
        
        _mockUserManager.Setup(u => u.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        _mockUserManager.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

        var token = await _service.GenerateJwtTokenAsync(user.Email);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task GenerateJwtTokenAsync_InvalidEmail_ThrowsArgumentException()
    {
        _mockUserManager.Setup(u => u.FindByEmailAsync("invalid@test.com")).ReturnsAsync((ApplicationUser?)null);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.GenerateJwtTokenAsync("invalid@test.com"));
    }
}
