using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Application.Services;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;
using LigaLibre.Domain.Interfaces;
using Moq;

namespace LigaLibre.Tests.Services;

/// <summary>
/// Pruebas unitarias para RefereeServices
/// </summary>
public class RefereeServicesTests
{
    private readonly Mock<IRefereeRepository> _mockRepository;
    private readonly Mock<IRedisCacheService> _mockCache;
    private readonly Mock<ISqsService> _mockSqs;
    private readonly RefereeServices _service;

    public RefereeServicesTests()
    {
        _mockRepository = new Mock<IRefereeRepository>();
        _mockCache = new Mock<IRedisCacheService>();
        _mockSqs = new Mock<ISqsService>();
        _service = new RefereeServices(_mockRepository.Object, _mockCache.Object, _mockSqs.Object);
    }

    [Fact]
    public async Task GetAllRefereeAsync_ReturnsReferees()
    {
        var referees = new List<Referee> { new() { Id = 1, FirstName = "Juan", LastName = "Perez" } };
        _mockCache.Setup(c => c.GetAsync<IEnumerable<RefereeDto>>("referee:all")).ReturnsAsync((IEnumerable<RefereeDto>?)null);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(referees);

        var result = await _service.GetAllRefereeAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetActiveRefereeAsync_ReturnsActiveReferees()
    {
        var referees = new List<Referee> { new() { Id = 1, FirstName = "Juan", IsActive = true } };
        _mockCache.Setup(c => c.GetAsync<IEnumerable<RefereeDto>>("referee:active")).ReturnsAsync((IEnumerable<RefereeDto>?)null);
        _mockRepository.Setup(r => r.GetActivesAsync()).ReturnsAsync(referees);

        var result = await _service.GetActiveRefereeAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetRefereeByIdAsync_ExistingId_ReturnsReferee()
    {
        var referee = new Referee { Id = 1, FirstName = "Juan", LastName = "Perez" };
        _mockCache.Setup(c => c.GetAsync<RefereeDto>("referee:1")).ReturnsAsync((RefereeDto?)null);
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(referee);

        var result = await _service.GetRefereeByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateRefereeAsync_ValidDto_ReturnsReferee()
    {
        var createDto = new CreateRefereeDto { FirstName = "Juan", LastName = "Perez", LicenseNumber = "REF001", Category = RefereeCategoryEnum.National };
        var referee = new Referee { Id = 1, FirstName = "Juan", LastName = "Perez" };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Referee>())).ReturnsAsync(referee);

        var result = await _service.CreateRefereeAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.RefereeEvent, 0), Times.Once);
    }

    [Fact]
    public async Task UpdateRefereeAsync_ExistingReferee_ReturnsUpdated()
    {
        var referee = new Referee { Id = 1, FirstName = "Juan", LastName = "Perez" };
        var updateDto = new UpdateRefereeDto { Id = 1, FirstName = "Juan", LastName = "Perez", LicenseNumber = "REF001", Category = RefereeCategoryEnum.National };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(referee);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Referee>())).ReturnsAsync(referee);

        var result = await _service.UpdateRefereeAsync(1, updateDto);

        Assert.NotNull(result);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.RefereeEvent, 0), Times.Once);
    }

    [Fact]
    public async Task DeleteRefereeAsync_ExistingReferee_ReturnsTrue()
    {
        var referee = new Referee { Id = 1 };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(referee);
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteRefereeAsync(1);

        Assert.True(result);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.RefereeEvent, 0), Times.Once);
    }

    [Fact]
    public async Task DeleteRefereeAsync_NonExistingReferee_ReturnsFalse()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Referee?)null);

        var result = await _service.DeleteRefereeAsync(999);

        Assert.False(result);
    }
}
