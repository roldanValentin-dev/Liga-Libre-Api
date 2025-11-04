using LigaLibre.Domain.Entities;

namespace LigaLibre.Domain.Interfaces
{
    public interface IRefereeRepository
    {
        Task<Referee> CreateAsync(Referee referee);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Referee>> GetActivesAsync();
        Task<IEnumerable<Referee>> GetAllAsync();
        Task<Referee?> GetByIdAsync(int id);
        Task<Referee?> GetByLicenseNumberAsync(string licenseNumber);
        Task<Referee?> UpdateAsync(Referee referee);
    }
}