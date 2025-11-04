using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Interfaces;
using Mapster;


namespace LigaLibre.Application.Services;

public class RefereeServices(IRefereeRepository refereeRepository, IRedisCacheService cacheService, ISqsService sqsService) : IRefereeServices
{
    public async Task<IEnumerable<RefereeDto>> GetAllRefereeAsync()
    {
        var cacheReferee = await cacheService.GetAsync<IEnumerable<RefereeDto>>("referee:all");
        if (cacheReferee != null)
        {
            return cacheReferee;
        }

        var referee = await refereeRepository.GetAllAsync();

        await cacheService.SetAsync("referee:all", referee.Select(MapToDto), TimeSpan.FromMinutes(10));

        return referee.Select(MapToDto);

    }

    public async Task<IEnumerable<RefereeDto>> GetActiveRefereeAsync()
    {
        var cacheReferee = await cacheService.GetAsync<IEnumerable<RefereeDto>>("referee:active");
        if (cacheReferee != null)
        {
            return cacheReferee;
        }

        var referee = await refereeRepository.GetActivesAsync();

        await cacheService.SetAsync("referee:active", referee.Select(MapToDto), TimeSpan.FromMinutes(10));

        return referee.Select(MapToDto);
    }

    public async Task<RefereeDto?> GetRefereeByIdAsync(int Id)
    {
        var cacheReferee = await cacheService.GetAsync<RefereeDto>($"referee:{Id}");
        if (cacheReferee != null)
        {
            return cacheReferee;
        }
        var referee = await refereeRepository.GetByIdAsync(Id);

        if (referee == null) return null;

        await cacheService.SetAsync($"referee:{Id}", MapToDto(referee), TimeSpan.FromMinutes(10));

        return MapToDto(referee);
    }

    public async Task<RefereeDto?> GetRefereeByLicenseNumberAsync(string licenceNumber)
    {
        var cacheReferee = await cacheService.GetAsync<RefereeDto>($"referee:licence:{licenceNumber}");
        if (cacheReferee != null)
        {
            return cacheReferee;
        }
        var referee = await refereeRepository.GetByLicenseNumberAsync(licenceNumber);

        if (referee == null) return null;

        await cacheService.SetAsync($"referee:licence:{licenceNumber}", MapToDto(referee), TimeSpan.FromMinutes(10));

        return MapToDto(referee);
    }

    public async Task<RefereeDto> CreateRefereeAsync(CreateRefereeDto refereeDto)
    {
        var existingReferee = await refereeRepository.GetByLicenseNumberAsync(refereeDto.LicenseNumber);
        if (existingReferee != null)
        {
            throw new ArgumentException("Existe un arbitro con la misma licencia");
        }
        var referee = refereeDto.Adapt<Referee>();

        var createdReferee = await refereeRepository.CreateAsync(referee);

        await sqsService.SendMessageAsync(new
        {
            EventType = "RefereeCreated",
            RefereeId = createdReferee.Id,
            LicenseNumber = createdReferee.LicenseNumber,
            FullName = $"{createdReferee.FirstName} {createdReferee.LastName}",
            Category = createdReferee.Category.ToString(),
            IsActive = createdReferee.IsActive,
            TimeStamp = DateTime.UtcNow

        }, QueueNames.RefereeEvent, 0);

        await cacheService.RemoveAsync("referee:all");
        await cacheService.RemoveAsync("referee:active");

        return MapToDto(createdReferee);
    }
    public async Task<RefereeDto> UpdateRefereeAsync(int id, UpdateRefereeDto refereeDto)
    {
        var existingReferee = await refereeRepository.GetByIdAsync(id);
        if (existingReferee == null)
        {
            throw new ArgumentException("Ya existe un arbitro con el mismo id");
        }

        refereeDto.Adapt(existingReferee);

        var updatedReferee = await refereeRepository.UpdateAsync(existingReferee);

        if (updatedReferee != null)
        {
            await sqsService.SendMessageAsync(new
            {
                EventType = "RefereeUpdated",
                RefereeId = updatedReferee.Id,
                LicenseNumber = updatedReferee.LicenseNumber,
                FullName = $"{updatedReferee.FirstName} {updatedReferee.LastName}",
                Category = updatedReferee.Category.ToString(),
                IsActive = updatedReferee.IsActive,
                TimeStamp = DateTime.UtcNow

            }, QueueNames.RefereeEvent, 0);
        }

        await cacheService.RemoveAsync($"referee:{id}");
        await cacheService.RemoveAsync("referee:all");
        await cacheService.RemoveAsync("referee:active");

        return MapToDto(updatedReferee!);
    }
    public async Task<bool> DeleteRefereeAsync(int id)
    {
        var existingReferee = await refereeRepository.GetByIdAsync(id);


        await sqsService.SendMessageAsync(new
        {
            EventType = "RefereeDeleted",
            RefereeId = id,
            TimeStamp = DateTime.UtcNow

        }, QueueNames.RefereeEvent, 0);

        await cacheService.RemoveAsync($"referee:{id}");
        await cacheService.RemoveAsync("referee:all");
        await cacheService.RemoveAsync("referee:active");

        return await refereeRepository.DeleteAsync(id);
    }
    private static RefereeDto MapToDto(Referee referee) => referee.Adapt<RefereeDto>();
   
}
