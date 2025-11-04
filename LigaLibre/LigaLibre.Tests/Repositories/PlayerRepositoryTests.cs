using LigaLibre.Domain.Entities;
using LigaLibre.Infrastructure.Data;
using LigaLibre.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LigaLibre.Tests.Repositories;

/// <summary>
/// Pruebas unitarias para PlayerRepository
/// </summary>
public class PlayerRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly PlayerRepository _repository;
    private readonly Club _testClub;

    public PlayerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _repository = new PlayerRepository(_context);

        _testClub = new Club { Name = "Test Club", City = "Test City", Email = "test@test.com", NumberOfPartners = 100 };
        _context.Club.Add(_testClub);
        _context.SaveChanges();
    }

    [Fact]
    public async Task CreateAsync_AddsPlayer()
    {
        var player = new Player { FirstName = "Juan", LastName = "Perez", Position = "Delantero", ClubId = _testClub.Id };

        var result = await _repository.CreateAsync(player);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Juan", result.FirstName);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllPlayers()
    {
        await _repository.CreateAsync(new Player { FirstName = "Carlos", LastName = "Lopez", Position = "Mediocampista", ClubId = _testClub.Id });
        await _repository.CreateAsync(new Player { FirstName = "Pedro", LastName = "Garcia", Position = "Defensor", ClubId = _testClub.Id });

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsPlayer()
    {
        var player = await _repository.CreateAsync(new Player { FirstName = "Luis", LastName = "Martinez", Position = "Arquero", ClubId = _testClub.Id });

        var result = await _repository.GetByIdAsync(player.Id);

        Assert.NotNull(result);
        Assert.Equal("Luis", result.FirstName);
        Assert.NotNull(result.Club);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByClubIdAsync_ReturnsPlayersFromClub()
    {
        await _repository.CreateAsync(new Player { FirstName = "Diego", LastName = "Fernandez", Position = "Delantero", ClubId = _testClub.Id });
        await _repository.CreateAsync(new Player { FirstName = "Martin", LastName = "Gomez", Position = "Mediocampista", ClubId = _testClub.Id });

        var result = await _repository.GetByClubIdAsync(_testClub.Id);

        Assert.Equal(2, result.Count());
        Assert.All(result, p => Assert.Equal(_testClub.Id, p.ClubId));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesPlayer()
    {
        var player = await _repository.CreateAsync(new Player { FirstName = "Javier", LastName = "Rodriguez", Position = "Defensor", ClubId = _testClub.Id });
        player.FirstName = "Javier Updated";

        var result = await _repository.UpdateAsync(player);

        Assert.Equal("Javier Updated", result.FirstName);
        Assert.NotEqual(player.CreatedAt, result.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_ExistingPlayer_ReturnsTrue()
    {
        var player = await _repository.CreateAsync(new Player { FirstName = "Roberto", LastName = "Sanchez", Position = "Delantero", ClubId = _testClub.Id });

        var result = await _repository.DeleteAsync(player.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingPlayer_ReturnsFalse()
    {
        var result = await _repository.DeleteAsync(999);

        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
