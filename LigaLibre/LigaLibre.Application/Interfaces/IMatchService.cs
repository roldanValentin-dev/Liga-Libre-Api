using LigaLibre.Application.DTOs;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Application.Interfaces
{
    public interface IMatchService
    {
        Task<MatchDto> CreateMatchAsync(CreateMatchDto createMatchDto);
        Task<bool> DeleteMatchAsync(int id);
        Task<IEnumerable<MatchDto>> GetAllMatchesAsync();
        Task<MatchDto?> GetMatchByIdAsync(int matchId);
        Task<IEnumerable<MatchDto>> GetMatchesByClubAsync(int clubId);
        Task<IEnumerable<MatchDto>> GetMatchesByRoundAsync(int round);
        Task<IEnumerable<MatchDto>> GetMatchesByStatusAsync(MatchStatusEnum status);
        Task<MatchDto?> UpdateMatchAsync(int id, UpdateMatchDto updateMatchDto);
    }
}