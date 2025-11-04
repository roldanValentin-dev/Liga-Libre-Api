using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;
using LigaLibre.Domain.Interfaces;
using Mapster;

namespace LigaLibre.Application.Services;

public class MatchService(IMatchRepository matchRepository, IRedisCacheService cacheService, ISqsService sqsService) : IMatchService
{
    public async Task<IEnumerable<MatchDto>> GetAllMatchesAsync()
    {
        var cachedMatches = await cacheService.GetAsync<IEnumerable<MatchDto>>("matches:all");
        if (cachedMatches != null)
        {
            return cachedMatches;
        }

        var matches = await matchRepository.GetAllAsync();

        await cacheService.SetAsync("matches:all", matches.Select(MapToDto),TimeSpan.FromMinutes(10));

        return matches.Select(MapToDto);
    }

    public async Task<MatchDto?> GetMatchByIdAsync(int matchId)
    {
        var cachedMatches = await cacheService.GetAsync<MatchDto>($"matches:{matchId}");

        if (cachedMatches != null)
        {
            return cachedMatches;
        }

        var match = await matchRepository.GetByIdAsync(matchId);
        if (match == null)
        {
            return null;
        }

        await cacheService.SetAsync($"matches:{matchId}", MapToDto(match),TimeSpan.FromMinutes(10));

        return MapToDto(match);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByClubAsync(int clubId)
    {
        var cachedMatches = await cacheService.GetAsync<IEnumerable<MatchDto>>($"matches:club:{clubId}");
        if (cachedMatches != null)
        {
            return cachedMatches;
        }
        var matches = await matchRepository.GetByClubAsync(clubId);

        await cacheService.SetAsync($"matches:club:{clubId}", matches.Select(MapToDto), TimeSpan.FromMinutes(10));
        return matches.Select(MapToDto);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByRoundAsync(int round)
    {
        var cachedMatches = await cacheService.GetAsync<IEnumerable<MatchDto>>($"matches:round:{round}");
        if (cachedMatches != null)
        {
            return cachedMatches;
        }

        var matches = await matchRepository.GetByRoundAsync(round);

        await cacheService.SetAsync($"matches:round:{round}", matches.Select(MapToDto), TimeSpan.FromMinutes(10));
        return matches.Select(MapToDto);
    }

    public async Task<IEnumerable<MatchDto>> GetMatchesByStatusAsync(MatchStatusEnum status)
    {
        var matches = await matchRepository.GetByStatusAsync(status);
        return matches.Select(MapToDto);
    }

    public async Task<MatchDto> CreateMatchAsync(CreateMatchDto createMatchDto)
    {
        var match = createMatchDto.Adapt<Match>();

        var createdMatch = await matchRepository.CreateAsync(match);

        await sqsService.SendMessageAsync(new
        {
            EventType = "MatchCreated",
            MatchId = createdMatch.Id,
            HomeClubId = createdMatch.HomeClubId,
            AwayClubId = createdMatch.AwayClubId,
            Round = createdMatch.Round,
            MatchDate = createdMatch.MatchDate,
            Timestamp = DateTime.UtcNow
        }, QueueNames.MatchEvent);

        await cacheService.RemoveAsync("matches:all");
        await cacheService.RemoveAsync($"matches:club:{createdMatch.HomeClubId}");
        await cacheService.RemoveAsync($"matches:club:{createdMatch.AwayClubId}");
        await cacheService.RemoveAsync($"matches:round:{createdMatch.Round}");

        return MapToDto(createdMatch);
    }

    public async Task<MatchDto?> UpdateMatchAsync(int id, UpdateMatchDto updateMatchDto)
    {
        var existingMatch = await matchRepository.GetByIdAsync(id);
        if (existingMatch == null) return null;

        updateMatchDto.Adapt(existingMatch);

        var updatedMatch = await matchRepository.UpdateAsync(existingMatch);

        await sqsService.SendMessageAsync(new
        {
            EventType = "MatchUpdated",
            MatchId = updatedMatch.Id,
            HomeClubId = updatedMatch.HomeClubId,
            AwayClubId = updatedMatch.AwayClubId,
            HomeScore = updatedMatch.HomeScore,
            AwayScore = updatedMatch.AwayScore,
            Status = updatedMatch.Status,
            Timestamp = DateTime.UtcNow
        }, QueueNames.MatchEvent);

        await cacheService.RemoveAsync("matches:all");
        await cacheService.RemoveAsync($"matches:{updatedMatch.Id}");
    
        return MapToDto(updatedMatch);
    }
    public async Task<bool> DeleteMatchAsync(int id)
    {
        var match = await matchRepository.GetByIdAsync(id);
        if (match != null)
        {
            await sqsService.SendMessageAsync(new
            {
                EventType = "MatchDeleted",
                MatchId = id,
                Timestamp = DateTime.UtcNow
            }, QueueNames.MatchEvent);

            await cacheService.RemoveAsync("matches:all");
            await cacheService.RemoveAsync($"matches:{id}");
        
        }

        return await matchRepository.DeleteAsync(id);
    }

    private static MatchDto MapToDto(Match match) => match.Adapt<MatchDto>();
}
