using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;
using LigaLibre.Infrastructure.Data;
using LigaLibre.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LigaLibre.Tests.Repositories;

/// <summary>
/// Pruebas unitarias para MatchRepository
/// </summary>
public class MatchRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly MatchRepository _repository;
    private readonly Club _homeClub;
    private readonly Club _awayClub;
    private readonly Referee _referee;

    public MatchRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _repository = new MatchRepository(_context);

        _homeClub = new Club { Name = "Home Club", City = "City A", Email = "home@test.com", NumberOfPartners = 100 };
        _awayClub = new Club { Name = "Away Club", City = "City B", Email = "away@test.com", NumberOfPartners = 200 };
        _referee = new Referee { FirstName = "Juan", LastName = "Arbitro", LicenseNumber = "REF001", Category = RefereeCategoryEnum.National, IsActive = true };
        
        _context.Club.AddRange(_homeClub, _awayClub);
        _context.Referee.Add(_referee);
        _context.SaveChanges();
    }

    [Fact]
    public async Task CreateAsync_AddsMatch()
    {
        var match = new Match { HomeClubId = _homeClub.Id, AwayClubId = _awayClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow, Round = 1, Status = MatchStatusEnum.Scheduled };

        var result = await _repository.CreateAsync(match);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllMatches()
    {
        await _repository.CreateAsync(new Match { HomeClubId = _homeClub.Id, AwayClubId = _awayClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow, Round = 1, Status = MatchStatusEnum.Scheduled });
        await _repository.CreateAsync(new Match { HomeClubId = _awayClub.Id, AwayClubId = _homeClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow.AddDays(1), Round = 2, Status = MatchStatusEnum.Scheduled });

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsMatch()
    {
        var match = await _repository.CreateAsync(new Match { HomeClubId = _homeClub.Id, AwayClubId = _awayClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow, Round = 1, Status = MatchStatusEnum.Scheduled });

        var result = await _repository.GetByIdAsync(match.Id);

        Assert.NotNull(result);
        Assert.NotNull(result.HomeClub);
        Assert.NotNull(result.AwayClub);
        Assert.NotNull(result.Referee);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByClubAsync_ReturnsMatchesForClub()
    {
        await _repository.CreateAsync(new Match { HomeClubId = _homeClub.Id, AwayClubId = _awayClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow, Round = 1, Status = MatchStatusEnum.Scheduled });
        await _repository.CreateAsync(new Match { HomeClubId = _awayClub.Id, AwayClubId = _homeClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow.AddDays(1), Round = 2, Status = MatchStatusEnum.Scheduled });

        var result = await _repository.GetByClubAsync(_homeClub.Id);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByRoundAsync_ReturnsMatchesForRound()
    {
        await _repository.CreateAsync(new Match { HomeClubId = _homeClub.Id, AwayClubId = _awayClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow, Round = 1, Status = MatchStatusEnum.Scheduled });
        await _repository.CreateAsync(new Match { HomeClubId = _awayClub.Id, AwayClubId = _homeClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow.AddDays(1), Round = 1, Status = MatchStatusEnum.Scheduled });

        var result = await _repository.GetByRoundAsync(1);

        Assert.Equal(2, result.Count());
        Assert.All(result, m => Assert.Equal(1, m.Round));
    }

    [Fact]
    public async Task GetByStatusAsync_ReturnsMatchesWithStatus()
    {
        await _repository.CreateAsync(new Match { HomeClubId = _homeClub.Id, AwayClubId = _awayClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow, Round = 1, Status = MatchStatusEnum.Scheduled });
        await _repository.CreateAsync(new Match { HomeClubId = _awayClub.Id, AwayClubId = _homeClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow.AddDays(1), Round = 2, Status = MatchStatusEnum.Finished });

        var result = await _repository.GetByStatusAsync(MatchStatusEnum.Scheduled);

        Assert.Single(result);
        Assert.All(result, m => Assert.Equal(MatchStatusEnum.Scheduled, m.Status));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesMatch()
    {
        var match = await _repository.CreateAsync(new Match { HomeClubId = _homeClub.Id, AwayClubId = _awayClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow, Round = 1, Status = MatchStatusEnum.Scheduled });
        match.Status = MatchStatusEnum.Finished;
        match.HomeScore = 2;
        match.AwayScore = 1;

        var result = await _repository.UpdateAsync(match);

        Assert.Equal(MatchStatusEnum.Finished, result.Status);
        Assert.Equal(2, result.HomeScore);
        Assert.NotEqual(match.CreatedAt, result.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_ExistingMatch_ReturnsTrue()
    {
        var match = await _repository.CreateAsync(new Match { HomeClubId = _homeClub.Id, AwayClubId = _awayClub.Id, RefereeId = _referee.Id, MatchDate = DateTime.UtcNow, Round = 1, Status = MatchStatusEnum.Scheduled });

        var result = await _repository.DeleteAsync(match.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingMatch_ReturnsFalse()
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
