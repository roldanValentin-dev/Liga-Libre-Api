
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Interfaces;
using Mapster;


namespace LigaLibre.Application.Services;


public class ClubService(IClubRepository clubRepository, ISqsService sqsServices, IRedisCacheService cacheService) : IClubService
{
 
    public async Task<IEnumerable<ClubDto>> GetAllClubsAsync()
    {
        var cacheClubs = await cacheService.GetAsync<IEnumerable<ClubDto>>("clubs:all");

        if (cacheClubs != null)
        {
            return cacheClubs;
        }

        var clubs = await clubRepository.GetAllAsync();

        await cacheService.SetAsync("clubs:all", clubs.Select(MapToDto), TimeSpan.FromMinutes(10));

        return clubs.Select(MapToDto);

    }
    
    //hacemos una expresion para implementar mapeo de mapster 
    private static ClubDto MapToDto(Club club) => club.Adapt<ClubDto>();


    public async Task<ClubDto?> GetClubByIdAsync(int id)
    {
        var cacheClub = await cacheService.GetAsync<ClubDto>($"club:{id}");

        if (cacheClub != null)
        {
            return cacheClub;
        }

        var club = await clubRepository.GetByIdAsync(id);
        if (club == null) return null;
        await cacheService.SetAsync($"club:{id}", MapToDto(club), TimeSpan.FromMinutes(10));
        return MapToDto(club);

    }

    public async Task<ClubDto> CreateClubAsync(CreateClubDto createClubDto)
    {

        var club = createClubDto.Adapt<Club>();

        var createdClub = await clubRepository.CreateAsync(club);

        await sqsServices.SendMessageAsync(new
        {
            EventType = "ClubCreated",
            ClubId = createdClub.Id,
            Name = createdClub.Name,
            City = createdClub.City,
            NumberOfPartners = createdClub.NumberOfPartners,
            Email = createdClub.Email,
            Phone = createdClub.Phone,
            Address = createdClub.Address,
            StadiumName = createdClub.StadiumName,
            Timestamp = DateTime.UtcNow

        }, QueueNames.ClubEvent);

        await cacheService.RemoveAsync("clubs:all");


        return MapToDto(createdClub);

    }

  
    public async Task<ClubDto> UpdateClubAsync(int id, UpdateClubDto updateClubDto)
    {
        var existingClub = await clubRepository.GetByIdAsync(id);
        if (existingClub == null)
            throw new ArgumentException("Club not found");

        updateClubDto.Adapt(existingClub);

        var updatedClub = await clubRepository.UpdateAsync(existingClub);

        await sqsServices.SendMessageAsync(new
        {
            EventType = "ClubUpdated",
            ClubId = updatedClub.Id,
            Name = updatedClub.Name,
            City = updatedClub.City,
            NumberOfPartners = updatedClub.NumberOfPartners,
            Email = updatedClub.Email,
            Phone = updatedClub.Phone,
            Address = updatedClub.Address,
            StadiumName = updatedClub.StadiumName,
            Timestamp = DateTime.UtcNow

        }, QueueNames.ClubEvent);

        await cacheService.RemoveAsync("clubs:all");
        await cacheService.RemoveAsync($"club:{updatedClub.Id}");

        return MapToDto(updatedClub);
    }

 
    public async Task<bool> DeleteClubAsync(int id)
    {
        await sqsServices.SendMessageAsync(new
        {
            EventType = "ClubDeleted",
            ClubId = id,
            Timestamp = DateTime.UtcNow
        }, QueueNames.ClubEvent);

        await cacheService.RemoveAsync("clubs:all");
        await cacheService.RemoveAsync($"club:{id}");

        return await clubRepository.DeleteAsync(id);

    }
}
