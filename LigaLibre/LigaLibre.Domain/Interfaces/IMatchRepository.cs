using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Domain.Interfaces;

public interface IMatchRepository
{
    public Task<IEnumerable<Match>> GetAllAsync();
    public Task<Match?> GetByIdAsync(int id);
    public Task<IEnumerable<Match>> GetByClubAsync(int clubId);
    public Task<IEnumerable<Match>> GetByRoundAsync(int round);
    public Task<IEnumerable<Match>> GetByStatusAsync(MatchStatusEnum status);
    public Task<Match> CreateAsync(Match match);
    public Task<Match> UpdateAsync(Match match);
    public Task<bool> DeleteAsync(int id);

}
