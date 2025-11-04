using LigaLibre.Application.DTOs;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Tests.DTOs;

public class MatchDtoTests
{
    [Fact]
    public void MatchDto_DefaultValues_AreCorrect()
    {
        var dto = new MatchDto();

        Assert.Equal(0, dto.Id);
        Assert.Equal(0, dto.Round);
        Assert.Equal(MatchStatusEnum.Scheduled, dto.Status);
    }

    [Fact]
    public void MatchDto_SetProperties_WorksCorrectly()
    {
        var dto = new MatchDto
        {
            Id = 1,
            HomeClubId = 1,
            AwayClubId = 2,
            Round = 5,
            HomeScore = 3,
            AwayScore = 1,
            Status = MatchStatusEnum.Finished,
            Stadium = "Monumental"
        };

        Assert.Equal(1, dto.Id);
        Assert.Equal(5, dto.Round);
        Assert.Equal(3, dto.HomeScore);
    }
}

public class CreateMatchDtoTests
{
    [Fact]
    public void CreateMatchDto_SetProperties_WorksCorrectly()
    {
        var dto = new CreateMatchDto
        {
            HomeClubId = 1,
            AwayClubId = 2,
            Round = 1,
            Stadium = "La Bombonera",
            MatchDate = DateTime.Now
        };

        Assert.Equal(1, dto.HomeClubId);
        Assert.Equal("La Bombonera", dto.Stadium);
    }
}

public class UpdateMatchDtoTests
{
    [Fact]
    public void UpdateMatchDto_SetProperties_WorksCorrectly()
    {
        var dto = new UpdateMatchDto
        {
            HomeClubId = 1,
            AwayClubId = 2,
            Round = 1,
            HomeScore = 2,
            AwayScore = 1,
            Status = MatchStatusEnum.Finished
        };

        Assert.Equal(2, dto.HomeScore);
        Assert.Equal(MatchStatusEnum.Finished, dto.Status);
    }
}
