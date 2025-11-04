using LigaLibre.Domain.Entities;

namespace LigaLibre.Tests.Entities;

public class ClubTests
{
    [Fact]
    public void Club_DefaultValues_AreCorrect()
    {
        var club = new Club();

        Assert.Equal(string.Empty, club.Name);
        Assert.Equal(string.Empty, club.City);
    }

    [Fact]
    public void Club_SetProperties_WorksCorrectly()
    {
        var club = new Club
        {
            Name = "River Plate",
            City = "Buenos Aires",
            StadiumName = "Monumental",
            NumberOfPartners = 50000,
            Email = "info@river.com",
            Phone = "123456789",
            Address = "Av. Figueroa Alcorta 7597"
        };

        Assert.Equal("River Plate", club.Name);
        Assert.Equal("Buenos Aires", club.City);
        Assert.Equal("Monumental", club.StadiumName);
        Assert.Equal(50000, club.NumberOfPartners);
    }

    [Fact]
    public void Club_Players_InitializesEmpty()
    {
        var club = new Club();

        Assert.NotNull(club.Players);
        Assert.Empty(club.Players);
    }
}
