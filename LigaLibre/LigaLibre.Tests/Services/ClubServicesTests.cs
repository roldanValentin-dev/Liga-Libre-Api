using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Application.Services;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Interfaces;
using Moq;

namespace LigaLibre.Tests.Services;

public class ClubServicesTests
{
    private readonly Mock<IClubRepository> _mockRepository;
    private readonly Mock<ISqsService> _mockSqs;
    private readonly Mock<IRedisCacheService> _mockCache;
    private readonly ClubService _service;

    public ClubServicesTests()
    {
        _mockRepository = new Mock<IClubRepository>();
        _mockSqs = new Mock<ISqsService>();
        _mockCache = new Mock<IRedisCacheService>();
        _service = new ClubService(_mockRepository.Object, _mockSqs.Object, _mockCache.Object);
    }


    [Fact]
    public async Task GetClubByIdAsync_ExistingId_ReturnsClub()
    {
        var club = new Club { Id = 1, Name = "River", City = "Buenos aires" };
        _mockCache.Setup(c => c.GetAsync<ClubDto>("club:1")).ReturnsAsync((ClubDto?)null);
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(club);

        var result = await _service.GetClubByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(club.Id, result.Id);
        Assert.Equal(club.Name, result.Name);
        Assert.Equal(club.City, result.City);
    }
    [Fact]
    public async Task CreateClubAsync_ValidDto_ReturnsClub()
    {
        var createDto = new CreateClubDto
        {
            Name = "River Plate",
            City = "Buenos Aires",
            Email = "info@river.com",
            StadiumName = "Monumental",
            NumberOfPartners = 50000,
        };
        var club = new Club
        {
           Id = 1,
           Name = "River Plate",
           City = "Buenos Aires",
           Email = "info@river.com",
           StadiumName = "Monumental"
        };
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Club>())).ReturnsAsync(club);

        var result = await _service.CreateClubAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(club.Id, result.Id);
        Assert.Equal(club.Name, result.Name);
        Assert.Equal(club.City, result.City);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.ClubEvent, 0), Times.Once);
    }

    [Fact]
    public async Task GetAllClubsAsync_ReturnsClubs()
    {
        var clubs = new List<Club> { new() { Id = 1, Name = "Boca", City = "Buenos Aires" } };
        _mockCache.Setup(c => c.GetAsync<IEnumerable<ClubDto>>("clubs:all")).ReturnsAsync((IEnumerable<ClubDto>?)null);
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(clubs);

        var result = await _service.GetAllClubsAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateClubAsync_ExistingClub_ReturnsUpdated()
    {
        var club = new Club { Id = 1, Name = "River", City = "Buenos Aires" };
        var updateDto = new UpdateClubDto { Id = 1, Name = "River Plate", City = "Buenos Aires" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(club);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Club>())).ReturnsAsync(club);

        var result = await _service.UpdateClubAsync(1, updateDto);

        Assert.NotNull(result);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.ClubEvent, 0), Times.Once);
    }

    [Fact]
    public async Task DeleteClubAsync_ExistingClub_ReturnsTrue()
    {
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteClubAsync(1);

        Assert.True(result);
        _mockSqs.Verify(s => s.SendMessageAsync(It.IsAny<object>(), QueueNames.ClubEvent, 0), Times.Once);
    }
}
