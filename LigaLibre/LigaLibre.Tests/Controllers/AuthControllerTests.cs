using LigaLibre.API.Controllers;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LigaLibre.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockService = new Mock<IAuthService>();
        _controller = new AuthController(_mockService.Object);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        var loginDto = new LoginDto { Email = "test@test.com", Password = "Password123!" };
        var authResponse = new AuthResponseDto { Token = "token", Email = "test@test.com" };
        _mockService.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(authResponse);

        var result = await _controller.Login(loginDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(authResponse, okResult.Value);
    }

    [Fact]
    public async Task Register_ValidData_ReturnsOk()
    {
        var registerDto = new RegisterDto { Email = "new@test.com", Password = "Password123!", FirstName = "John", LastName = "Doe" };
        var authResponse = new AuthResponseDto { Token = "token", Email = "new@test.com", FirstName = "John", LastName = "Doe" };
        _mockService.Setup(s => s.RegisterAsync(registerDto)).ReturnsAsync(authResponse);

        var result = await _controller.Register(registerDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(authResponse, okResult.Value);
    }
}
