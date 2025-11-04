using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;
using LigaLibre.Infrastructure.Data;
using LigaLibre.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LigaLibre.Tests.Repositories;

/// <summary>
/// Pruebas unitarias para RefereeRepository
/// </summary>
public class RefereeRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly RefereeRepository _repository;

    public RefereeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        _context = new ApplicationDbContext(options);
        _repository = new RefereeRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_AddsReferee()
    {
        var referee = new Referee { FirstName = "Juan", LastName = "Perez", LicenseNumber = "REF001", Category = RefereeCategoryEnum.National, IsActive = true };

        var result = await _repository.CreateAsync(referee);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("Juan", result.FirstName);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllReferees()
    {
        await _repository.CreateAsync(new Referee { FirstName = "Carlos", LastName = "Lopez", LicenseNumber = "REF002", Category = RefereeCategoryEnum.Regional, IsActive = true });
        await _repository.CreateAsync(new Referee { FirstName = "Pedro", LastName = "Garcia", LicenseNumber = "REF003", Category = RefereeCategoryEnum.National, IsActive = false });

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsReferee()
    {
        var referee = await _repository.CreateAsync(new Referee { FirstName = "Luis", LastName = "Martinez", LicenseNumber = "REF004", Category = RefereeCategoryEnum.National, IsActive = true });

        var result = await _repository.GetByIdAsync(referee.Id);

        Assert.NotNull(result);
        Assert.Equal("Luis", result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByLicenseNumberAsync_ExistingLicense_ReturnsReferee()
    {
        await _repository.CreateAsync(new Referee { FirstName = "Diego", LastName = "Fernandez", LicenseNumber = "REF005", Category = RefereeCategoryEnum.National, IsActive = true });

        var result = await _repository.GetByLicenseNumberAsync("REF005");

        Assert.NotNull(result);
        Assert.Equal("Diego", result.FirstName);
    }

    [Fact]
    public async Task GetByLicenseNumberAsync_NonExistingLicense_ReturnsNull()
    {
        var result = await _repository.GetByLicenseNumberAsync("NONEXISTENT");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetActivesAsync_ReturnsOnlyActiveReferees()
    {
        await _repository.CreateAsync(new Referee { FirstName = "Martin", LastName = "Gomez", LicenseNumber = "REF006", Category = RefereeCategoryEnum.National, IsActive = true });
        await _repository.CreateAsync(new Referee { FirstName = "Javier", LastName = "Rodriguez", LicenseNumber = "REF007", Category = RefereeCategoryEnum.Regional, IsActive = false });
        await _repository.CreateAsync(new Referee { FirstName = "Roberto", LastName = "Sanchez", LicenseNumber = "REF008", Category = RefereeCategoryEnum.National, IsActive = true });

        var result = await _repository.GetActivesAsync();

        Assert.Equal(2, result.Count());
        Assert.All(result, r => Assert.True(r.IsActive));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesReferee()
    {
        var referee = await _repository.CreateAsync(new Referee { FirstName = "Andres", LastName = "Torres", LicenseNumber = "REF009", Category = RefereeCategoryEnum.National, IsActive = true });
        referee.FirstName = "Andres Updated";
        referee.IsActive = false;

        var result = await _repository.UpdateAsync(referee);

        Assert.NotNull(result);
        Assert.Equal("Andres Updated", result.FirstName);
        Assert.False(result.IsActive);
        Assert.NotEqual(referee.CreatedAt, result.UpdatedAt);
    }

    [Fact]
    public async Task DeleteAsync_ExistingReferee_ReturnsTrue()
    {
        var referee = await _repository.CreateAsync(new Referee { FirstName = "Miguel", LastName = "Ramirez", LicenseNumber = "REF010", Category = RefereeCategoryEnum.Regional, IsActive = true });

        var result = await _repository.DeleteAsync(referee.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingReferee_ReturnsFalse()
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
