using FluentValidation;
using FluentValidation.Results;
using LigaLibre.API.Controllers;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LigaLibre.Tests.Controllers;

public class PlayerControllerTests
{
    private readonly Mock<IPlayerService> _mockService;
    private readonly Mock<IValidator<CreatePlayerDto>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdatePlayerDto>> _mockUpdateValidator;
    private readonly PlayerController _controller;

    public PlayerControllerTests()
    {
        _mockService = new Mock<IPlayerService>();
        _mockCreateValidator = new Mock<IValidator<CreatePlayerDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdatePlayerDto>>();
        _controller = new PlayerController(_mockService.Object, _mockCreateValidator.Object, _mockUpdateValidator.Object);
    }

    [Fact]
    public async Task GetAllPlayers_ReturnsOkResult()
    {
        var players = new List<PlayerDto> { new() { Id = 1, FirstName = "Lionel" } };
        _mockService.Setup(s => s.GetAllPlayers()).ReturnsAsync(players);

        var result = await _controller.GetAllPlayers();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(players, okResult.Value);
    }

    [Fact]
    public async Task CreatePlayers_ValidDto_ReturnsCreated()
    {
        var createDto = new CreatePlayerDto { FirstName = "Cristiano", ClubId = 1 };
        var player = new PlayerDto { Id = 1, FirstName = "Cristiano" };
        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.CreatePlayerAsync(createDto)).ReturnsAsync(player);

        var result = await _controller.CreatePlayers(createDto);

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, statusResult.StatusCode);
    }

    [Fact]
    public async Task CreatePlayers_InvalidDto_ReturnsBadRequest()
    {
        var createDto = new CreatePlayerDto();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("FirstName", "Required") });
        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(validationResult);

        var result = await _controller.CreatePlayers(createDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdatePlayer_ValidDto_ReturnsOk()
    {
        var updateDto = new UpdatePlayerDto { Id = 1, FirstName = "Leo" };
        var player = new PlayerDto { Id = 1, FirstName = "Leo" };
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.UpdatePlayerAsync(1, updateDto)).ReturnsAsync(player);

        var result = await _controller.UpdatePlayer(1, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(player, okResult.Value);
    }

    [Fact]
    public async Task DeletePlayers_ReturnsOk()
    {
        _mockService.Setup(s => s.DeletePlayerAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeletePlayers(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value!);
    }

    [Fact]
    public async Task GetPlayersByClub_ReturnsOk()
    {
        var players = new List<PlayerDto> { new() { Id = 1, FirstName = "Lionel" } };
        _mockService.Setup(s => s.GetPlayerByClubAsync(1)).ReturnsAsync(players);

        var result = await _controller.GetPlayersByClub(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(players, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk()
    {
        var player = new PlayerDto { Id = 1, FirstName = "Lionel" };
        _mockService.Setup(s => s.GetPlayerByIdAsync(1)).ReturnsAsync(player);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(player, okResult.Value);
    }
}
