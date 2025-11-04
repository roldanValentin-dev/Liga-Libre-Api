using FluentValidation;
using LigaLibre.Application;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Application.Services;
using LigaLibre.Domain.Interfaces;
using LigaLibre.Infrastructure;
using LigaLibre.Infrastructure.Repositories;
using LigaLibre.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LigaLibre.Tests.Configuration;

/// <summary>
/// Pruebas unitarias para DependencyInjections
/// </summary>
public class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_RegistersAllServices()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        Assert.Contains(services, s => s.ServiceType == typeof(IPlayerService));
        Assert.Contains(services, s => s.ServiceType == typeof(IMatchService));
        Assert.Contains(services, s => s.ServiceType == typeof(IClubService));
        Assert.Contains(services, s => s.ServiceType == typeof(IStatisticsService));
    }

    [Fact]
    public void AddApplication_RegistersValidators()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        Assert.Contains(services, s => s.ServiceType == typeof(IValidator<CreatePlayerDto>));
        Assert.Contains(services, s => s.ServiceType == typeof(IValidator<CreateClubDto>));
    }

    [Fact]
    public void AddApplication_ServicesAreScoped()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        var clubService = services.FirstOrDefault(s => s.ServiceType == typeof(IClubService));
        var playerService = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerService));
        var matchService = services.FirstOrDefault(s => s.ServiceType == typeof(IMatchService));
        var statisticsService = services.FirstOrDefault(s => s.ServiceType == typeof(IStatisticsService));

        Assert.NotNull(clubService);
        Assert.Equal(ServiceLifetime.Scoped, clubService.Lifetime);
        Assert.NotNull(playerService);
        Assert.Equal(ServiceLifetime.Scoped, playerService.Lifetime);
        Assert.NotNull(matchService);
        Assert.Equal(ServiceLifetime.Scoped, matchService.Lifetime);
        Assert.NotNull(statisticsService);
        Assert.Equal(ServiceLifetime.Scoped, statisticsService.Lifetime);
    }

    [Fact]
    public void AddInfrastructure_RegistersRepositories()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=test;Database=test;",
                ["ConnectionStrings:redis"] = "localhost:6379"
            }!)
            .Build();

        services.AddInfrastructure(configuration);

        Assert.Contains(services, s => s.ServiceType == typeof(IPlayerRepository));
        Assert.Contains(services, s => s.ServiceType == typeof(IMatchRepository));
        Assert.Contains(services, s => s.ServiceType == typeof(IClubRepository));
    }

    [Fact]
    public void AddInfrastructure_RegistersServices()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=test;Database=test;",
                ["ConnectionStrings:redis"] = "localhost:6379"
            }!)
            .Build();

        services.AddInfrastructure(configuration);

        Assert.Contains(services, s => s.ServiceType == typeof(IAuthService));
        Assert.Contains(services, s => s.ServiceType == typeof(ISqsService));
        Assert.Contains(services, s => s.ServiceType == typeof(IRedisCacheService));
    }

    [Fact]
    public void AddInfrastructure_RepositoriesAreScoped()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=test;Database=test;",
                ["ConnectionStrings:redis"] = "localhost:6379"
            }!)
            .Build();

        services.AddInfrastructure(configuration);

        var clubRepo = services.FirstOrDefault(s => s.ServiceType == typeof(IClubRepository));
        var playerRepo = services.FirstOrDefault(s => s.ServiceType == typeof(IPlayerRepository));
        var matchRepo = services.FirstOrDefault(s => s.ServiceType == typeof(IMatchRepository));

        Assert.NotNull(clubRepo);
        Assert.Equal(ServiceLifetime.Scoped, clubRepo.Lifetime);
        Assert.NotNull(playerRepo);
        Assert.Equal(ServiceLifetime.Scoped, playerRepo.Lifetime);
        Assert.NotNull(matchRepo);
        Assert.Equal(ServiceLifetime.Scoped, matchRepo.Lifetime);
    }

    [Fact]
    public void AddInfrastructure_ConfiguresRedisCache()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=test;Database=test;",
                ["ConnectionStrings:redis"] = "localhost:6379"
            }!)
            .Build();

        services.AddInfrastructure(configuration);

        Assert.Contains(services, s => s.ServiceType.Name.Contains("IDistributedCache"));
    }

    [Fact]
    public void AddInfrastructure_ConfiguresIdentity()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Server=test;Database=test;",
                ["ConnectionStrings:redis"] = "localhost:6379"
            }!)
            .Build();

        services.AddInfrastructure(configuration);

        Assert.Contains(services, s => s.ServiceType.Name.Contains("UserManager"));
        Assert.Contains(services, s => s.ServiceType.Name.Contains("SignInManager"));
    }
}
