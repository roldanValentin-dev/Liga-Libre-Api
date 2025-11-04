using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Application.Services;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Interfaces;
using Moq;

namespace LigaLibre.Tests.Services;

public class PlayerServiceTests
{
    private readonly Mock<IPlayerRepository> _mockRepository;
    private readonly Mock<IRedisCacheService> _mockCache;
    private readonly Mock<ISqsService> _mockSqs;
    private readonly PlayerService _service;

    public PlayerServiceTests()
    {
        _mockRepository = new Mock<IPlayerRepository>();
        _mockCache = new Mock<IRedisCacheService>();
        _mockSqs = new Mock<ISqsService>();
        _service = new PlayerService(_mockRepository.Object, _mockCache.Object, _mockSqs.Object);
    }

    [Fact]
    public async Task GetAllPlayers_ReturnsPlayers()
    {
        var players = new List<Player> { new() { Id = 1, FirstName = "Lionel", LastName = "Messi", ClubId = 1 } };
        _mockCache.Setup(c => c.GetAsync<IEnumerable<PlayerDto>>("players:all")).ReturnsAsync((IEnumerable<PlayerDto>?)null);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(players);

        var result = await _service.GetAllPlayers();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Lionel", result.First().FirstName);
    }

    [Fact]
    public async Task GetPlayerByIdAsync_ExistingId_ReturnsPlayer()
    {
        var player = new Player { Id = 1, FirstName = "Lionel", LastName = "Messi", ClubId = 1, Position = "Delantero" };
        _mockCache.Setup(c => c.GetAsync<PlayerDto>("players:1")).ReturnsAsync((PlayerDto?)null);
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(player);

        var result = await _service.GetPlayerByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Lionel", result.FirstName);
        Assert.Equal("Messi", result.LastName);
    }

    [Fact]
    public async Task CreatePlayerAsync_ValidDto_ReturnsPlayer()
    {
        var createDto = new CreatePlayerDto { FirstName = "Cristiano", LastName = "Ronaldo", ClubId = 1, Position = "Delantero", Age = 38 };
        var player = new Player { Id = 1, FirstName = "Cristiano", LastName = "Ronaldo", ClubId = 1, Position = "Delantero" };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Player>())).ReturnsAsync(player);

        var result = await _service.CreatePlayerAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Cristiano", result.FirstName);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.PlayerEvent, 0), Times.Once);
    }

    [Fact]
    public async Task UpdatePlayerAsync_ExistingPlayer_ReturnsUpdated()
    {
        var player = new Player { Id = 1, FirstName = "Leo", LastName = "Messi", ClubId = 1, Goals = 15 };
        var updateDto = new UpdatePlayerDto { Id = 1, FirstName = "Leo", LastName = "Messi", ClubId = 1, Position = "Delantero" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(player);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Player>())).ReturnsAsync(player);

        var result = await _service.UpdatePlayerAsync(1, updateDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Leo", result.FirstName);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.PlayerEvent, 0), Times.Once);
    }

    [Fact]
    public async Task DeletePlayerAsync_ExistingPlayer_ReturnsTrue()
    {
        var player = new Player { Id = 1, ClubId = 1 };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(player);
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeletePlayerAsync(1);

        Assert.True(result);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.PlayerEvent, 0), Times.Once);
    }
}
