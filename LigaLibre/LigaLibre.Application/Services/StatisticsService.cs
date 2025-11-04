using LigaLibre.Application.DTOs;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;
using LigaLibre.Domain.Interfaces;
using Mapster;
using static LigaLibre.Application.DTOs.LeagueStatisticsDto;
namespace LigaLibre.Application.Services;

public class StatisticsService(IMatchRepository matchRepository, IPlayerRepository playerRepository, IClubRepository clubRepository, IRedisCacheService cacheService) : IStatisticsService
{
    public async Task<LeagueStatisticsDto> GetLeaguesStatisticsDtoAsync()
    {
        var cachedStats = await cacheService.GetAsync<LeagueStatisticsDto>("statistics:league");
        if (cachedStats != null)
        {
            return cachedStats;
        }

        var matches = await matchRepository.GetAllAsync();
        var players = await playerRepository.GetAllAsync();
        var clubs = await clubRepository.GetAllAsync();

        var finishedMatches = matches.Where(m => m.Status == MatchStatusEnum.Finished).ToList();
        var totalGoals = finishedMatches.Sum(m => (m.HomeScore ?? 0) + (m.AwayScore ?? 0));


        var topScorers = players.Where(p => p.Goals > 0)
            .OrderByDescending(p => p.Goals)
            .Take(10)
            .Select(p => p.Adapt<TopScorersDto>()).ToArray();

        var standings = GetClubStadingsAsync(clubs, matches);

        var result = new LeagueStatisticsDto
        {
            TotalMatches = matches.Count(),
            FinishedMatches = finishedMatches.Count(),
            ScheduledMatches = matches.Count(m => m.Status == MatchStatusEnum.Scheduled),
            TotalGoals = totalGoals,
            TotalClubs = clubs.Count(),
            TotalPlayers = players.Count(),
            TopScorers = topScorers,
            Standings = standings
        };

        await cacheService.SetAsync("statistics:league", result, TimeSpan.FromMinutes(5));
        return result;
    }

    public async Task<MatchStatisticsDto> GetMatchesStatisticsDtoAsync()
    {
        var cachedStats = await cacheService.GetAsync<MatchStatisticsDto>("statistics:matches");
        if (cachedStats != null)
        {
            return cachedStats;
        }

        var matches = await matchRepository.GetAllAsync();
        var recentMatches = matches
            .Where(m => m.Status == MatchStatusEnum.Finished)
            .OrderByDescending(m => m.MatchDate)
            .Take(10)
            .Select(m => m.Adapt<RecentMatchDto>()).ToArray();

        var totalMatches = matches.Count();
        var finishedMatches = matches.Count(m => m.Status == MatchStatusEnum.Finished);

        var result = new MatchStatisticsDto
        {
            TotalMatches = totalMatches,
            FinishedMatches = finishedMatches,
            ScheduledMatches = matches.Count(m => m.Status == MatchStatusEnum.Scheduled),
            InProgressMatches = matches.Count(m => m.Status == MatchStatusEnum.InProgress),
            PostponedMatches = matches.Count(m => m.Status == MatchStatusEnum.Postponed),
            CancelledMatches = matches.Count(m => m.Status == MatchStatusEnum.Cancelled),
            CompletionPercentage = totalMatches > 0 ? finishedMatches / totalMatches * 100 : 0,
            RecentMatches = recentMatches
        };

        await cacheService.SetAsync("statistics:matches", result, TimeSpan.FromMinutes(5));
        return result;
    }

    public async Task<PlayerStatisticsDto> GetPlayersStatisticsDtoAsync()
    {
        var cachedStats = await cacheService.GetAsync<PlayerStatisticsDto>("statistics:players");
        if (cachedStats != null)
        {
            return cachedStats;
        }

        var players = await playerRepository.GetAllAsync();

        var topScorers = players.Where(p => p.Goals > 0).OrderByDescending(p => p.Goals).Take(10).Select(p => new TopScorersDto
        {
            PlayerName = $"{p.FirstName} {p.LastName}",
            ClubName = p.Club?.Name ?? "Sin Club",
            Goals = p.Goals,
            Assists = p.Assists,
            MatchesPlayed = p.MatchesPlayed
        }).ToArray();

        var topAssits = players.Where(p => p.Assists > 0)
            .OrderByDescending(p => p.Assists)
            .Take(10)
            .Select(p => p.Adapt<TopAssistsDto>()).ToArray();
        var positionStats = players
            .GroupBy(p => p.Position)
            .Select(g => g.Adapt<PositionStatsDto>()).ToArray();

        var result = new PlayerStatisticsDto
        {
            TotalPlayers = players.Count(),
            ActivePlayers= players.Count(p=> p.IsActive),
            AverageAge = players.Any() ? players.Average(p => p.Age) : 0,
            TopScorers = topScorers,
            TopAssists = topAssits,
            PositionStats = positionStats
        };

        await cacheService.SetAsync("statistics:players", result, TimeSpan.FromMinutes(5));
        return result;
    }
    public static ClubStadingDto[] GetClubStadingsAsync(IEnumerable<Club> clubs, IEnumerable<Match> matches)
    {

        return clubs.Select(club =>
        {

            var homeMatches = matches.Where(m => m.HomeClubId == club.Id && m.Status == MatchStatusEnum.Finished);
            var awayMatches = matches.Where(m => m.AwayClubId == club.Id && m.Status == MatchStatusEnum.Finished);

            var wins = homeMatches.Count(m => m.HomeScore > m.AwayScore) +
                        awayMatches.Count(m => m.AwayScore > m.HomeScore);

            var draws = homeMatches.Count(m => m.HomeScore == m.AwayScore) +
                        awayMatches.Count(m => m.AwayScore == m.HomeScore);

            var losses = homeMatches.Count(m => m.HomeScore < m.AwayScore) +
                        awayMatches.Count(m => m.AwayScore < m.HomeScore);

            var goalsFor = homeMatches.Sum(m => m.HomeScore ?? 0) +
                        awayMatches.Sum(m => m.AwayScore ?? 0);

            var goalsAgainst = homeMatches.Sum(m => m.AwayScore ?? 0) +
                              awayMatches.Sum(m => m.HomeScore ?? 0);

            return new ClubStadingDto
            {
                ClubName = club.Name,
                MatchesPlayed = wins + draws + losses,
                Wins = wins,
                Draws = draws,
                Losses = losses,
                GoalsFor = goalsFor,
                GoalsAgainst = goalsAgainst,
                GoalDifference = goalsFor - goalsAgainst,
                Points = (wins * 3) + draws
            };

        })
        .OrderByDescending(s => s.Points)
        .ThenByDescending(s => s.GoalDifference)
        .ThenByDescending(s => s.GoalsFor)
        .ToArray();
    }
}
