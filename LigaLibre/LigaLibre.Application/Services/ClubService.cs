using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Interfaces;

namespace LigaLibre.Application.Services
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;

        public ClubService(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<IEnumerable<ClubDto>> GetAllClubsAsync()
        {
            var clubs = await _clubRepository.GetAllAsync();
            return clubs.Select(MapToDto);
        }
        public async Task<ClubDto?> GetClubByIdAsync(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            return club != null ? MapToDto(club) : null;
        }
        public async Task<ClubDto> CreateClubAsync(CreateClubDto createClubDto)
        {
            var club = new Club
            {
                Name = createClubDto.Name,
                City = createClubDto.City,
                Email = createClubDto.Email,
                NumberOfPartners = createClubDto.NumberOfPartners,
                Phone = createClubDto.Phone,
                Address = createClubDto.Address,
                StadiumName = createClubDto.StadiumName,
            };

            var createClub = await _clubRepository.CreateAsync(club);
            return MapToDto(createClub);
        }
        public async Task<ClubDto> UpdateClubAsync(int id, CreateClubDto createClubDto)
        {
            var existingClub = await _clubRepository.GetByIdAsync(id);
            if (existingClub == null)
                throw new ArgumentException("Club not found");


            existingClub.Name = createClubDto.Name;
            existingClub.City = createClubDto.City;
            existingClub.Email = createClubDto.Email;
            existingClub.NumberOfPartners = createClubDto.NumberOfPartners;
            existingClub.Phone = createClubDto.Phone;
            existingClub.Address = createClubDto.Address;
            existingClub.StadiumName = createClubDto.StadiumName;

            var updatedClub = await _clubRepository.UpdateAsync(existingClub);
            return MapToDto(updatedClub);
        }
        public async Task<bool> DeleteClubAsync(int id)
        {
            return await _clubRepository.DeleteAsync(id);
        }

        private static ClubDto MapToDto(Club club)
        {
            return new ClubDto
            {
                Id = club.Id,
                Name = club.Name,
                City = club.City,
                Email = club.Email,
                NumberOfPartners = club.NumberOfPartners,
                Phone = club.Phone,
                Address = club.Address,
                StadiumName = club.StadiumName,
            };
        }

    }
}
