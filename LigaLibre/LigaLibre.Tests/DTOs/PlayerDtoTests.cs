using LigaLibre.Application.DTOs;

namespace LigaLibre.Tests.DTOs;

public class PlayerDtoTests
{
    [Fact]
    public void PlayerDto_DefaultValues_AreCorrect()
    {
        var dto = new PlayerDto();

        Assert.Equal(0, dto.Id);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(0, dto.Goals);
    }

    [Fact]
    public void PlayerDto_SetProperties_WorksCorrectly()
    {
        var dto = new PlayerDto
        {
            Id = 1,
            FirstName = "Lionel",
            LastName = "Messi",
            Age = 36,
            Position = "Delantero",
            Goals = 800,
            Assists = 350,
            ClubId = 1
        };

        Assert.Equal(1, dto.Id);
        Assert.Equal("Lionel", dto.FirstName);
        Assert.Equal(800, dto.Goals);
    }
}

public class CreatePlayerDtoTests
{
    [Fact]
    public void CreatePlayerDto_SetProperties_WorksCorrectly()
    {
        var dto = new CreatePlayerDto
        {
            FirstName = "Cristiano",
            LastName = "Ronaldo",
            Age = 38,
            Position = "Delantero",
            JerseyNumber = 7,
            ClubId = 1
        };

        Assert.Equal("Cristiano", dto.FirstName);
        Assert.Equal(7, dto.JerseyNumber);
    }
}
