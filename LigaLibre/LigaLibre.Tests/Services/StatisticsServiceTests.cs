using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Application.Services;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;
using LigaLibre.Domain.Interfaces;
using Moq;
using MatchEntity = LigaLibre.Domain.Entities.Match;
using static LigaLibre.Application.DTOs.LeagueStatisticsDto;

namespace LigaLibre.Tests.Services;

public class StatisticsServiceTests
{
    private readonly Mock<IMatchRepository> _mockMatchRepo;
    private readonly Mock<IPlayerRepository> _mockPlayerRepo;
    private readonly Mock<IClubRepository> _mockClubRepo;
    private readonly Mock<IRedisCacheService> _mockCache;
    private readonly StatisticsService _service;

    public StatisticsServiceTests()
    {
        _mockMatchRepo = new Mock<IMatchRepository>();
        _mockPlayerRepo = new Mock<IPlayerRepository>();
        _mockClubRepo = new Mock<IClubRepository>();
        _mockCache = new Mock<IRedisCacheService>();
        _service = new StatisticsService(_mockMatchRepo.Object, _mockPlayerRepo.Object, _mockClubRepo.Object, _mockCache.Object);
    }

    [Fact]
    public async Task GetLeaguesStatisticsDtoAsync_ReturnsStatistics()
    {
        var matches = new List<MatchEntity> { new() { Id = 1, Status = MatchStatusEnum.Finished, HomeScore = 2, AwayScore = 1 } };
        var players = new List<Player> { new() { Id = 1, Goals = 10, Club = new Club { Name = "Club1" } } };
        var clubs = new List<Club> { new() { Id = 1, Name = "Club1" } };

        _mockCache.Setup(c => c.GetAsync<LeagueStatisticsDto>("statistics:league")).ReturnsAsync((LeagueStatisticsDto?)null);
        _mockMatchRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(matches);
        _mockPlayerRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(players);
        _mockClubRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(clubs);

        var result = await _service.GetLeaguesStatisticsDtoAsync();

        Assert.NotNull(result);
        Assert.Equal(1, result.TotalMatches);
        _mockCache.Verify(c => c.SetAsync("statistics:league", It.IsAny<LeagueStatisticsDto>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task GetMatchesStatisticsDtoAsync_ReturnsStatistics()
    {
        var matches = new List<MatchEntity> { new() { Id = 1, Status = MatchStatusEnum.Finished } };
        _mockCache.Setup(c => c.GetAsync<MatchStatisticsDto>("statistics:matches")).ReturnsAsync((MatchStatisticsDto?)null);
        _mockMatchRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(matches);

        var result = await _service.GetMatchesStatisticsDtoAsync();

        Assert.NotNull(result);
        Assert.Equal(1, result.TotalMatches);
    }

    [Fact]
    public async Task GetPlayersStatisticsDtoAsync_ReturnsStatistics()
    {
        var players = new List<Player> { new() { Id = 1, Goals = 5, Age = 25, IsActive = true, Position = "Delantero" } };
        _mockCache.Setup(c => c.GetAsync<PlayerStatisticsDto>("statistics:players")).ReturnsAsync((PlayerStatisticsDto?)null);
        _mockPlayerRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(players);

        var result = await _service.GetPlayersStatisticsDtoAsync();

        Assert.NotNull(result);
        Assert.Equal(1, result.TotalPlayers);
    }
}
