using LigaLibre.Application.DTOs;

namespace LigaLibre.Application.Interfaces
{
    public interface IClubService
    {
        Task<IEnumerable<ClubDto>> GetAllClubsAsync();
        Task<ClubDto?> GetClubByIdAsync(int id);
        Task<ClubDto> CreateClubAsync(CreateClubDto createClubDto);
        Task<ClubDto> UpdateClubAsync(int id, CreateClubDto createClubDto);
        Task<bool> DeleteClubAsync(int id);
    }
}
