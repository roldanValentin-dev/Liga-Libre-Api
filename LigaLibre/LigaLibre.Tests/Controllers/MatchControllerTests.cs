using FluentValidation;
using FluentValidation.Results;
using LigaLibre.API.Controllers;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LigaLibre.Tests.Controllers;

public class MatchControllerTests
{
    private readonly Mock<IMatchService> _mockService;
    private readonly Mock<IValidator<CreateMatchDto>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdateMatchDto>> _mockUpdateValidator;
    private readonly MatchController _controller;

    public MatchControllerTests()
    {
        _mockService = new Mock<IMatchService>();
        _mockCreateValidator = new Mock<IValidator<CreateMatchDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateMatchDto>>();
        _controller = new MatchController(_mockService.Object, _mockCreateValidator.Object, _mockUpdateValidator.Object);
    }

    [Fact]
    public async Task GetAllMatches_ReturnsOkResult()
    {
        var matches = new List<MatchDto> { new() { Id = 1, Round = 1 } };
        _mockService.Setup(s => s.GetAllMatchesAsync()).ReturnsAsync(matches);

        var result = await _controller.GetAllMatches();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(matches, okResult.Value);
    }

    [Fact]
    public async Task GetMatchById_ExistingId_ReturnsOk()
    {
        var match = new MatchDto { Id = 1, Round = 1 };
        _mockService.Setup(s => s.GetMatchByIdAsync(1)).ReturnsAsync(match);

        var result = await _controller.GetMatchById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(match, okResult.Value);
    }

    [Fact]
    public async Task GetMatchById_NonExistingId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetMatchByIdAsync(1)).ReturnsAsync((MatchDto?)null);

        var result = await _controller.GetMatchById(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateMatch_ReturnsCreated()
    {
        var createDto = new CreateMatchDto { HomeClubId = 1, AwayClubId = 2, Round = 1 };
        var match = new MatchDto { Id = 1, Round = 1 };
        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.CreateMatchAsync(createDto)).ReturnsAsync(match);

        var result = await _controller.CreateMatch(createDto);

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, statusResult.StatusCode);
    }

    [Fact]
    public async Task UpdateMatch_ExistingMatch_ReturnsOk()
    {
        var updateDto = new UpdateMatchDto { HomeClubId = 1, AwayClubId = 2, Round = 1 };
        var match = new MatchDto { Id = 1, Round = 1 };
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.UpdateMatchAsync(1, updateDto)).ReturnsAsync(match);

        var result = await _controller.UpdateMatch(1, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(match, okResult.Value);
    }

    [Fact]
    public async Task DeleteMatch_ReturnsOk()
    {
        _mockService.Setup(s => s.DeleteMatchAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteMatch(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value!);
    }

    [Fact]
    public async Task GetMatchesByClub_ReturnsOk()
    {
        var matches = new List<MatchDto> { new() { Id = 1, Round = 1 } };
        _mockService.Setup(s => s.GetMatchesByClubAsync(1)).ReturnsAsync(matches);

        var result = await _controller.GetMatchesByClub(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(matches, okResult.Value);
    }

    [Fact]
    public async Task GetMatchesByRound_ReturnsOk()
    {
        var matches = new List<MatchDto> { new() { Id = 1, Round = 1 } };
        _mockService.Setup(s => s.GetMatchesByRoundAsync(1)).ReturnsAsync(matches);

        var result = await _controller.GetMatchesByRound(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(matches, okResult.Value);
    }

    [Fact]
    public async Task GetMatchesByStatus_ReturnsOk()
    {
        var matches = new List<MatchDto> { new() { Id = 1, Round = 1 } };
        _mockService.Setup(s => s.GetMatchesByStatusAsync(It.IsAny<MatchStatusEnum>())).ReturnsAsync(matches);

        var result = await _controller.GetMatchesByStatus(MatchStatusEnum.Scheduled);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(matches, okResult.Value);
    }

    [Fact]
    public async Task UpdateMatch_NonExistingMatch_ReturnsNotFound()
    {
        var updateDto = new UpdateMatchDto { HomeClubId = 1, AwayClubId = 2, Round = 1 };
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.UpdateMatchAsync(999, updateDto)).ReturnsAsync((MatchDto?)null);

        var result = await _controller.UpdateMatch(999, updateDto);

        Assert.IsType<NotFoundResult>(result);
    }
}
