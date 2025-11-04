using LigaLibre.Application.DTOs;
using static LigaLibre.Application.DTOs.LeagueStatisticsDto;

namespace LigaLibre.Tests.DTOs;

public class LeagueStatisticsDtoTests
{
    [Fact]
    public void LeagueStatisticsDto_DefaultValues_AreCorrect()
    {
        var dto = new LeagueStatisticsDto();

        Assert.Equal(0, dto.TotalMatches);
        Assert.Equal(0, dto.TotalClubs);
        Assert.Equal(0, dto.TotalPlayers);
        Assert.NotNull(dto.TopScorers);
        Assert.Empty(dto.TopScorers);
        Assert.NotNull(dto.Standings);
        Assert.Empty(dto.Standings);
    }

    [Fact]
    public void LeagueStatisticsDto_SetProperties_WorksCorrectly()
    {
        var dto = new LeagueStatisticsDto
        {
            TotalMatches = 10,
            FinishedMatches = 5,
            TotalClubs = 8,
            TotalPlayers = 100,
            TotalGoals = 25,
            AverageGoalsPerMatch = 2.5
        };

        Assert.Equal(10, dto.TotalMatches);
        Assert.Equal(5, dto.FinishedMatches);
        Assert.Equal(8, dto.TotalClubs);
        Assert.Equal(100, dto.TotalPlayers);
        Assert.Equal(25, dto.TotalGoals);
        Assert.Equal(2.5, dto.AverageGoalsPerMatch);
    }
}

public class TopScorersDtoTests
{
    [Fact]
    public void TopScorersDto_DefaultValues_AreCorrect()
    {
        var dto = new TopScorersDto();

        Assert.Equal(string.Empty, dto.PlayerName);
        Assert.Equal(string.Empty, dto.ClubName);
        Assert.Equal(0, dto.Goals);
        Assert.Equal(0, dto.Assists);
    }

    [Fact]
    public void TopScorersDto_SetProperties_WorksCorrectly()
    {
        var dto = new TopScorersDto
        {
            PlayerName = "Lionel Messi",
            ClubName = "Inter Miami",
            Goals = 20,
            Assists = 10,
            MatchesPlayed = 15
        };

        Assert.Equal("Lionel Messi", dto.PlayerName);
        Assert.Equal("Inter Miami", dto.ClubName);
        Assert.Equal(20, dto.Goals);
        Assert.Equal(10, dto.Assists);
        Assert.Equal(15, dto.MatchesPlayed);
    }
}

public class ClubStadingDtoTests
{
    [Fact]
    public void ClubStadingDto_DefaultValues_AreCorrect()
    {
        var dto = new ClubStadingDto();

        Assert.Equal(string.Empty, dto.ClubName);
        Assert.Equal(0, dto.Points);
        Assert.Equal(0, dto.Wins);
    }

    [Fact]
    public void ClubStadingDto_SetProperties_WorksCorrectly()
    {
        var dto = new ClubStadingDto
        {
            ClubName = "River Plate",
            MatchesPlayed = 10,
            Wins = 7,
            Draws = 2,
            Losses = 1,
            GoalsFor = 20,
            GoalsAgainst = 8,
            GoalDifference = 12,
            Points = 23
        };

        Assert.Equal("River Plate", dto.ClubName);
        Assert.Equal(10, dto.MatchesPlayed);
        Assert.Equal(7, dto.Wins);
        Assert.Equal(23, dto.Points);
    }
}

public class MatchStatisticsDtoTests
{
    [Fact]
    public void MatchStatisticsDto_DefaultValues_AreCorrect()
    {
        var dto = new MatchStatisticsDto();

        Assert.Equal(0, dto.TotalMatches);
        Assert.NotNull(dto.RecentMatches);
        Assert.Empty(dto.RecentMatches);
    }

    [Fact]
    public void MatchStatisticsDto_SetProperties_WorksCorrectly()
    {
        var dto = new MatchStatisticsDto
        {
            TotalMatches = 50,
            FinishedMatches = 30,
            ScheduledMatches = 15,
            InProgressMatches = 3,
            PostponedMatches = 2,
            CompletionPercentage = 60
        };

        Assert.Equal(50, dto.TotalMatches);
        Assert.Equal(30, dto.FinishedMatches);
        Assert.Equal(60, dto.CompletionPercentage);
    }
}

public class PlayerStatisticsDtoTests
{
    [Fact]
    public void PlayerStatisticsDto_DefaultValues_AreCorrect()
    {
        var dto = new PlayerStatisticsDto();

        Assert.Equal(0, dto.TotalPlayers);
        Assert.NotNull(dto.TopScorers);
        Assert.Empty(dto.TopScorers);
        Assert.NotNull(dto.TopAssists);
        Assert.Empty(dto.TopAssists);
    }

    [Fact]
    public void PlayerStatisticsDto_SetProperties_WorksCorrectly()
    {
        var dto = new PlayerStatisticsDto
        {
            TotalPlayers = 200,
            ActivePlayers = 180,
            AverageAge = 26.5
        };

        Assert.Equal(200, dto.TotalPlayers);
        Assert.Equal(180, dto.ActivePlayers);
        Assert.Equal(26.5, dto.AverageAge);
    }
}

public class PositionStatsDtoTests
{
    [Fact]
    public void PositionStatsDto_DefaultValues_AreCorrect()
    {
        var dto = new PositionStatsDto();

        Assert.Equal(string.Empty, dto.Position);
        Assert.Equal(0, dto.Playercount);
        Assert.Equal(0, dto.TotalGoals);
    }

    [Fact]
    public void PositionStatsDto_SetProperties_WorksCorrectly()
    {
        var dto = new PositionStatsDto
        {
            Position = "Delantero",
            Playercount = 40,
            AverageAge = 27.5,
            TotalGoals = 150
        };

        Assert.Equal("Delantero", dto.Position);
        Assert.Equal(40, dto.Playercount);
        Assert.Equal(27.5, dto.AverageAge);
        Assert.Equal(150, dto.TotalGoals);
    }
}
