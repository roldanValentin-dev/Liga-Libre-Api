using LigaLibre.Domain.Entities;

namespace LigaLibre.Tests.Entities;

public class PlayerTests
{
    [Fact]
    public void Player_DefaultValues_AreCorrect()
    {
        var player = new Player();

        Assert.Equal(string.Empty, player.FirstName);
        Assert.Equal(string.Empty, player.LastName);
        Assert.Equal(0, player.Goals);
        Assert.Equal(0, player.Assists);
        Assert.True(player.IsActive);
    }

    [Fact]
    public void Player_SetProperties_WorksCorrectly()
    {
        var player = new Player
        {
            FirstName = "Lionel",
            LastName = "Messi",
            Age = 36,
            Position = "Delantero",
            JerseyNumber = 10,
            Goals = 800,
            Assists = 350,
            ClubId = 1
        };

        Assert.Equal("Lionel", player.FirstName);
        Assert.Equal("Messi", player.LastName);
        Assert.Equal(36, player.Age);
        Assert.Equal(800, player.Goals);
    }
}
