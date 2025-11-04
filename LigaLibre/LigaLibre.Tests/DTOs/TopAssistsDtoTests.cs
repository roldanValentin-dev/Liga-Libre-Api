using static LigaLibre.Application.DTOs.LeagueStatisticsDto;

namespace LigaLibre.Tests.DTOs;

public class TopAssistsDtoTests
{
    [Fact]
    public void TopAssistsDto_DefaultValues_AreCorrect()
    {
        var dto = new TopAssistsDto();

        Assert.Equal(string.Empty, dto.PlayerName);
        Assert.Equal(string.Empty, dto.ClubName);
        Assert.Equal(0, dto.Assists);
        Assert.Equal(0, dto.Goals);
    }

    [Fact]
    public void TopAssistsDto_SetProperties_WorksCorrectly()
    {
        var dto = new TopAssistsDto
        {
            PlayerName = "Lionel Messi",
            ClubName = "Inter Miami",
            Assists = 15,
            Goals = 10
        };

        Assert.Equal("Lionel Messi", dto.PlayerName);
        Assert.Equal("Inter Miami", dto.ClubName);
        Assert.Equal(15, dto.Assists);
        Assert.Equal(10, dto.Goals);
    }
}
