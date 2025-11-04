using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Tests.Entities;

public class MatchTests
{
    [Fact]
    public void Match_DefaultValues_AreCorrect()
    {
        var match = new Match();

        Assert.Equal(0, match.Round);
        Assert.Equal(MatchStatusEnum.Scheduled, match.Status);
        Assert.Null(match.HomeScore);
        Assert.Null(match.AwayScore);
    }

    [Fact]
    public void Match_SetProperties_WorksCorrectly()
    {
        var match = new Match
        {
            HomeClubId = 1,
            AwayClubId = 2,
            Round = 5,
            Stadium = "Monumental",
            HomeScore = 3,
            AwayScore = 1,
            Status = MatchStatusEnum.Finished,
            MatchDate = DateTime.Now
        };

        Assert.Equal(1, match.HomeClubId);
        Assert.Equal(2, match.AwayClubId);
        Assert.Equal(5, match.Round);
        Assert.Equal(3, match.HomeScore);
        Assert.Equal(MatchStatusEnum.Finished, match.Status);
    }
}
