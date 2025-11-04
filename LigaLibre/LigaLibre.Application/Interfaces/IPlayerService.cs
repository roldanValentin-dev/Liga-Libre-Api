using LigaLibre.Application.DTOs;

namespace LigaLibre.Application.Interfaces
{
    public interface IPlayerService
    {
        Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto createPlayerDto);
        Task<bool> DeletePlayerAsync(int id);
        Task<IEnumerable<PlayerDto>> GetPlayerByClubAsync(int clubId);
        Task<PlayerDto?> GetPlayerByIdAsync(int playerId);
        Task<IEnumerable<PlayerDto>> GetAllPlayers();
        Task<PlayerDto> UpdatePlayerAsync(int id, UpdatePlayerDto updatePlayerDto);
    }
}