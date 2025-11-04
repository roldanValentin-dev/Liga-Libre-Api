using LigaLibre.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;

namespace LigaLibre.Tests.Controllers;

/// <summary>
/// Pruebas unitarias para HealthController
/// </summary>
public class HealthControllerTests
{
    private readonly Mock<HealthCheckService> _mockHealthCheck;
    private readonly HealthController _controller;

    public HealthControllerTests()
    {
        _mockHealthCheck = new Mock<HealthCheckService>();
        _controller = new HealthController(_mockHealthCheck.Object);
    }

    [Fact]
    public async Task Get_HealthyStatus_ReturnsOk()
    {
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["Database"] = new HealthReportEntry(HealthStatus.Healthy, "DB is healthy", TimeSpan.FromMilliseconds(100), null, null)
            },
            TimeSpan.FromMilliseconds(100));

        _mockHealthCheck.Setup(h => h.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), default))
            .ReturnsAsync(healthReport);

        var result = await _controller.Get();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Get_UnhealthyStatus_Returns503()
    {
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["Database"] = new HealthReportEntry(HealthStatus.Unhealthy, "DB is down", TimeSpan.FromMilliseconds(100), null, null)
            },
            TimeSpan.FromMilliseconds(100));

        _mockHealthCheck.Setup(h => h.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), default))
            .ReturnsAsync(healthReport);

        var result = await _controller.Get();

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, statusResult.StatusCode);
    }

    [Fact]
    public async Task Get_DegradedStatus_Returns503()
    {
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["Database"] = new HealthReportEntry(HealthStatus.Degraded, "DB is slow", TimeSpan.FromMilliseconds(500), null, null)
            },
            TimeSpan.FromMilliseconds(500));

        _mockHealthCheck.Setup(h => h.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), default))
            .ReturnsAsync(healthReport);

        var result = await _controller.Get();

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, statusResult.StatusCode);
    }

    [Fact]
    public async Task Get_ReturnsCorrectResponseStructure()
    {
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["Database"] = new HealthReportEntry(HealthStatus.Healthy, "DB is healthy", TimeSpan.FromMilliseconds(100), null, null),
                ["Redis"] = new HealthReportEntry(HealthStatus.Healthy, "Redis is healthy", TimeSpan.FromMilliseconds(50), null, null)
            },
            TimeSpan.FromMilliseconds(150));

        _mockHealthCheck.Setup(h => h.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), default))
            .ReturnsAsync(healthReport);

        var result = await _controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task Get_MultipleHealthyChecks_ReturnsOk()
    {
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["Database"] = new HealthReportEntry(HealthStatus.Healthy, "DB OK", TimeSpan.FromMilliseconds(50), null, null),
                ["Redis"] = new HealthReportEntry(HealthStatus.Healthy, "Redis OK", TimeSpan.FromMilliseconds(30), null, null),
                ["SQS"] = new HealthReportEntry(HealthStatus.Healthy, "SQS OK", TimeSpan.FromMilliseconds(20), null, null)
            },
            TimeSpan.FromMilliseconds(100));

        _mockHealthCheck.Setup(h => h.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), default))
            .ReturnsAsync(healthReport);

        var result = await _controller.Get();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Get_MixedHealthStatus_Returns503()
    {
        var healthReport = new HealthReport(
            new Dictionary<string, HealthReportEntry>
            {
                ["Database"] = new HealthReportEntry(HealthStatus.Healthy, "DB OK", TimeSpan.FromMilliseconds(50), null, null),
                ["Redis"] = new HealthReportEntry(HealthStatus.Unhealthy, "Redis down", TimeSpan.FromMilliseconds(100), null, null)
            },
            TimeSpan.FromMilliseconds(150));

        _mockHealthCheck.Setup(h => h.CheckHealthAsync(It.IsAny<Func<HealthCheckRegistration, bool>>(), default))
            .ReturnsAsync(healthReport);

        var result = await _controller.Get();

        var statusResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(503, statusResult.StatusCode);
    }
}
