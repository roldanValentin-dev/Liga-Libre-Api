using LigaLibre.Domain.Entities;
using LigaLibre.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LigaLibre.Tests.Configuration;

public class ApplicationDbContextTests
{
    [Fact]
    public void ApplicationDbContext_HasDbSets()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);

        Assert.NotNull(context.Club);
        Assert.NotNull(context.Player);
        Assert.NotNull(context.Match);
        Assert.NotNull(context.Referee);
    }

    [Fact]
    public void ApplicationDbContext_CanAddClub()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var club = new Club { Name = "Test Club", City = "Test City" };
        
        context.Club.Add(club);
        context.SaveChanges();

        Assert.Equal(1, context.Club.Count());
    }

    [Fact]
    public void ApplicationDbContext_CanAddPlayer()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var player = new Player { FirstName = "Test", LastName = "Player", ClubId = 1 };
        
        context.Player.Add(player);
        context.SaveChanges();

        Assert.Equal(1, context.Player.Count());
    }
}
