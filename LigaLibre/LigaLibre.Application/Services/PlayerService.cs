using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Interfaces;
using Mapster;

namespace LigaLibre.Application.Services;

public class PlayerService(IPlayerRepository playerRepository, IRedisCacheService cacheService, ISqsService sqsService) : IPlayerService
{

    public async Task<IEnumerable<PlayerDto>> GetAllPlayers()
    {
        var cachedPlayers = await cacheService.GetAsync<IEnumerable<PlayerDto>>("players:all");
        if (cachedPlayers != null)
        {
            return cachedPlayers;
        }

        var players = await playerRepository.GetAllAsync();
        await cacheService.SetAsync("players:all", players.Select(MaptoDto), TimeSpan.FromMinutes(10));
        return players.Select(MaptoDto);
    }


    public async Task<IEnumerable<PlayerDto>> GetPlayerByClubAsync(int clubId)
    {
        var cachedPlayers = await cacheService.GetAsync<IEnumerable<PlayerDto>>($"players:club:{clubId}");
        if (cachedPlayers != null)
        {
            return cachedPlayers;
        }

        var players = await playerRepository.GetByClubIdAsync(clubId);
        await cacheService.SetAsync($"players:club:{clubId}", players.Select(MaptoDto), TimeSpan.FromMinutes(10));
        return players.Select(MaptoDto);
    }

    public async Task<PlayerDto?> GetPlayerByIdAsync(int playerId)
    {
        var cachedPlayer = await cacheService.GetAsync<PlayerDto>($"players:{playerId}");
        if (cachedPlayer != null)
        {
            return cachedPlayer;
        }

        var player = await playerRepository.GetByIdAsync(playerId);
        if (player == null)
        {
            return null;
        }

        await cacheService.SetAsync($"players:{playerId}", MaptoDto(player), TimeSpan.FromMinutes(10));
        return MaptoDto(player);
    }

    public async Task<PlayerDto> CreatePlayerAsync(CreatePlayerDto createPlayerDto)
    {
        var player = createPlayerDto.Adapt<Player>();

        var createdPlayer = await playerRepository.CreateAsync(player);

        await sqsService.SendMessageAsync(new
        {
            EventType = "PlayerCreated",
            PlayerId = createdPlayer.Id,
            FirstName = createdPlayer.FirstName,
            LastName = createdPlayer.LastName,
            ClubId = createdPlayer.ClubId,
            Position = createdPlayer.Position,
            Timestamp = DateTime.UtcNow
        }, QueueNames.PlayerEvent);

        await cacheService.RemoveAsync("players:all");
        await cacheService.RemoveAsync($"players:club:{createdPlayer.ClubId}");

        return MaptoDto(createdPlayer);
    }

    public async Task<PlayerDto> UpdatePlayerAsync(int id, UpdatePlayerDto updatePlayerDto)
    {
        var existingPlayer = await playerRepository.GetByIdAsync(id);
        if (existingPlayer == null)
        {
            throw new ArgumentException("Jugador no encontrado");
        }

        updatePlayerDto.Adapt(existingPlayer);

        var updatedPlayer = await playerRepository.UpdateAsync(existingPlayer);

        await sqsService.SendMessageAsync(new
        {
            EventType = "PlayerUpdated",
            PlayerId = updatedPlayer.Id,
            FirstName = updatedPlayer.FirstName,
            LastName = updatedPlayer.LastName,
            ClubId = updatedPlayer.ClubId,
            Position = updatedPlayer.Position,
            Timestamp = DateTime.UtcNow
        }, QueueNames.PlayerEvent);

        await cacheService.RemoveAsync("players:all");
        await cacheService.RemoveAsync($"players:{updatedPlayer.Id}");
        await cacheService.RemoveAsync($"players:club:{updatedPlayer.ClubId}");

        return MaptoDto(updatedPlayer);
    }

    public async Task<bool> DeletePlayerAsync(int id)
    {
        var player = await playerRepository.GetByIdAsync(id);
        if (player != null)
        {
            await sqsService.SendMessageAsync(new
            {
                EventType = "PlayerDeleted",
                PlayerId = id,
                ClubId = player.ClubId,
                Timestamp = DateTime.UtcNow
            }, QueueNames.PlayerEvent);

            await cacheService.RemoveAsync("players:all");
            await cacheService.RemoveAsync($"players:{id}");
            await cacheService.RemoveAsync($"players:club:{player.ClubId}");
        }

        return await playerRepository.DeleteAsync(id);
    }
    //mapeo dto con mappster
    private static PlayerDto MaptoDto(Player player) => player.Adapt<PlayerDto>();


}


