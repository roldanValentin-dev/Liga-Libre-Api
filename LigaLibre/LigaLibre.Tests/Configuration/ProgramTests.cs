using Microsoft.AspNetCore.Mvc.Testing;

namespace LigaLibre.Tests.Configuration;

/// <summary>
/// Pruebas unitarias para Program
/// </summary>
public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ProgramTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Program_CreatesApplication()
    {
        Assert.NotNull(_factory);
        Assert.NotNull(_factory.Services);
    }

    [Fact]
    public async Task Program_HealthCheckEndpoint_IsConfigured()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/health");

        Assert.NotNull(response);
    }
}
