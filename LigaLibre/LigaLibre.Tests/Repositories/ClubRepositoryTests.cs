using LigaLibre.Domain.Entities;
using LigaLibre.Infrastructure.Data;
using LigaLibre.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LigaLibre.Tests.Repositories;

public class ClubRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ClubRepository _repository;

    public ClubRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _repository = new ClubRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_AddsClub()
    {
        var club = new Club { Name = "River", City = "Buenos Aires", Email = "river@test.com", NumberOfPartners = 1000 };

        var result = await _repository.CreateAsync(club);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("River", result.Name);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllClubs()
    {
        await _repository.CreateAsync(new Club { Name = "Boca", City = "Buenos Aires", Email = "boca@test.com", NumberOfPartners = 1000 });
        await _repository.CreateAsync(new Club { Name = "Racing", City = "Avellaneda", Email = "racing@test.com", NumberOfPartners = 500 });

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsClub()
    {
        var club = await _repository.CreateAsync(new Club { Name = "Independiente", City = "Avellaneda", Email = "inde@test.com", NumberOfPartners = 800 });

        var result = await _repository.GetByIdAsync(club.Id);

        Assert.NotNull(result);
        Assert.Equal("Independiente", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesClub()
    {
        var club = await _repository.CreateAsync(new Club { Name = "San Lorenzo", City = "Buenos Aires", Email = "sanlo@test.com", NumberOfPartners = 600 });
        club.Name = "San Lorenzo Updated";

        var result = await _repository.UpdateAsync(club);

        Assert.Equal("San Lorenzo Updated", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingClub_ReturnsTrue()
    {
        var club = await _repository.CreateAsync(new Club { Name = "Velez", City = "Buenos Aires", Email = "velez@test.com", NumberOfPartners = 700 });

        var result = await _repository.DeleteAsync(club.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingClub_ReturnsFalse()
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
