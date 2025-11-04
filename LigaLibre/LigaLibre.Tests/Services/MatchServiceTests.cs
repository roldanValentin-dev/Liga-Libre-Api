using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Application.Services;
using LigaLibre.Domain.Enums;
using LigaLibre.Domain.Interfaces;
using Moq;
using MatchEntity = LigaLibre.Domain.Entities.Match;

namespace LigaLibre.Tests.Services;

public class MatchServiceTests
{
    private readonly Mock<IMatchRepository> _mockRepository;
    private readonly Mock<IRedisCacheService> _mockCache;
    private readonly Mock<ISqsService> _mockSqs;
    private readonly MatchService _service;

    public MatchServiceTests()
    {
        _mockRepository = new Mock<IMatchRepository>();
        _mockCache = new Mock<IRedisCacheService>();
        _mockSqs = new Mock<ISqsService>();
        _service = new MatchService(_mockRepository.Object, _mockCache.Object, _mockSqs.Object);
    }

    [Fact]
    public async Task GetAllMatchesAsync_ReturnsMatches()
    {
        var matches = new List<MatchEntity> { new() { Id = 1, Round = 1, HomeClubId = 1, AwayClubId = 2 } };
        _mockCache.Setup(c => c.GetAsync<IEnumerable<MatchDto>>("matches:all")).ReturnsAsync((IEnumerable<MatchDto>?)null);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(matches);

        var result = await _service.GetAllMatchesAsync();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(1, result.First().Id);
    }

    [Fact]
    public async Task GetMatchByIdAsync_ExistingId_ReturnsMatch()
    {
        var match = new MatchEntity { Id = 1, HomeClubId = 1, AwayClubId = 2, Round = 1, Stadium = "Estadio" };
        _mockCache.Setup(c => c.GetAsync<MatchDto>("matches:1")).ReturnsAsync((MatchDto?)null);
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(match);

        var result = await _service.GetMatchByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.Round);
        Assert.Equal("Estadio", result.Stadium);
    }

    [Fact]
    public async Task CreateMatchAsync_ValidDto_ReturnsMatch()
    {
        var createDto = new CreateMatchDto { HomeClubId = 1, AwayClubId = 2, Round = 1, Stadium = "Monumental", MatchDate = DateTime.Now };
        var match = new MatchEntity { Id = 1, HomeClubId = 1, AwayClubId = 2, Round = 1, Stadium = "Monumental" };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<MatchEntity>())).ReturnsAsync(match);

        var result = await _service.CreateMatchAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.HomeClubId);
        Assert.Equal(2, result.AwayClubId);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.MatchEvent, 0), Times.Once);
    }

    [Fact]
    public async Task UpdateMatchAsync_ExistingMatch_ReturnsUpdated()
    {
        var match = new MatchEntity { Id = 1, HomeClubId = 1, AwayClubId = 2, Round = 1, Status = MatchStatusEnum.Finished, HomeScore = 2, AwayScore = 1 };
        var updateDto = new UpdateMatchDto { HomeClubId = 1, AwayClubId = 2, Round = 1, Status = MatchStatusEnum.Finished, HomeScore = 2, AwayScore = 1 };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(match);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<MatchEntity>())).ReturnsAsync(match);

        var result = await _service.UpdateMatchAsync(1, updateDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(MatchStatusEnum.Finished, result.Status);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.MatchEvent, 0), Times.Once);
    }

    [Fact]
    public async Task DeleteMatchAsync_ExistingMatch_ReturnsTrue()
    {
        var match = new MatchEntity { Id = 1, HomeClubId = 1, AwayClubId = 2, Round = 1 };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(match);
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteMatchAsync(1);

        Assert.True(result);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.MatchEvent, 0), Times.Once);
    }
}
