using LigaLibre.Application.DTOs;

namespace LigaLibre.Application.Interfaces
{
    public interface IRefereeServices
    {
        Task<RefereeDto> CreateRefereeAsync(CreateRefereeDto refereeDto);
        Task<bool> DeleteRefereeAsync(int id);
        Task<IEnumerable<RefereeDto>> GetActiveRefereeAsync();
        Task<IEnumerable<RefereeDto>> GetAllRefereeAsync();
        Task<RefereeDto?> GetRefereeByIdAsync(int Id);
        Task<RefereeDto?> GetRefereeByLicenseNumberAsync(string licenceNumber);
        Task<RefereeDto> UpdateRefereeAsync(int id, UpdateRefereeDto refereeDto);
    }
}