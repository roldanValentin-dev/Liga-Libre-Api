using LigaLibre.API.Controllers;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static LigaLibre.Application.DTOs.LeagueStatisticsDto;

namespace LigaLibre.Tests.Controllers;

public class StatisticsControllerTests
{
    private readonly Mock<IStatisticsService> _mockService;
    private readonly StatisticsController _controller;

    public StatisticsControllerTests()
    {
        _mockService = new Mock<IStatisticsService>();
        _controller = new StatisticsController(_mockService.Object);
    }

    [Fact]
    public async Task GetLeaguesStatistics_ReturnsOkResult()
    {
        var stats = new LeagueStatisticsDto { TotalMatches = 10, TotalClubs = 5 };
        _mockService.Setup(s => s.GetLeaguesStatisticsDtoAsync()).ReturnsAsync(stats);

        var result = await _controller.GetLeaguesStatistics();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(stats, okResult.Value);
    }

    [Fact]
    public async Task GetMatchesStatistics_ReturnsOkResult()
    {
        var stats = new MatchStatisticsDto { TotalMatches = 10, FinishedMatches = 5 };
        _mockService.Setup(s => s.GetMatchesStatisticsDtoAsync()).ReturnsAsync(stats);

        var result = await _controller.GetMatchesStatistics();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(stats, okResult.Value);
    }

    [Fact]
    public async Task GetPlayersStatistics_ReturnsOkResult()
    {
        var stats = new PlayerStatisticsDto { TotalPlayers = 100, ActivePlayers = 90 };
        _mockService.Setup(s => s.GetPlayersStatisticsDtoAsync()).ReturnsAsync(stats);

        var result = await _controller.GetPlayersStatistics();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(stats, okResult.Value);
    }
}
