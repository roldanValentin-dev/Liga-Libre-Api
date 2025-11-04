using LigaLibre.Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;

namespace LigaLibre.Tests.Services;

public class RedisCacheServiceTests
{
    private readonly Mock<IDistributedCache> _mockCache;
    private readonly RedisCacheService _service;

    public RedisCacheServiceTests()
    {
        _mockCache = new Mock<IDistributedCache>();
        _service = new RedisCacheService(_mockCache.Object);
    }

    [Fact]
    public async Task GetAsync_ExistingKey_ReturnsValue()
    {
        var testData = new { Id = 1, Name = "Test" };
        var json = System.Text.Json.JsonSerializer.Serialize(testData);
        _mockCache.Setup(c => c.GetAsync("test:key", default)).ReturnsAsync(Encoding.UTF8.GetBytes(json));

        var result = await _service.GetAsync<dynamic>("test:key");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_NonExistingKey_ReturnsDefault()
    {
        _mockCache.Setup(c => c.GetAsync("test:key", default)).ReturnsAsync((byte[]?)null);

        var result = await _service.GetAsync<string>("test:key");

        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_ValidData_CallsCache()
    {
        var testData = new { Id = 1, Name = "Test" };

        await _service.SetAsync("test:key", testData, TimeSpan.FromMinutes(5));

        _mockCache.Verify(c => c.SetAsync("test:key", It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ValidKey_CallsCache()
    {
        await _service.RemoveAsync("test:key");

        _mockCache.Verify(c => c.RemoveAsync("test:key", default), Times.Once);
    }

    [Fact]
    public async Task RemovePatternAsync_ClubsPattern_RemovesClubsAll()
    {
        await _service.RemovePatternAsync("clubs:*");

        _mockCache.Verify(c => c.RemoveAsync("clubs:all", default), Times.Once);
    }
}
