using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Interfaces;
using LigaLibre.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace LigaLibre.Infrastructure.Repositories;

public class ClubRepository : IClubRepository
{
    private readonly ApplicationDbContext _context;
    //Inyectar el contexto de la base de datos
    public ClubRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    //Crear un club
    public async Task<Club> CreateAsync(Club club)
    {
        _context.Club.Add(club);
        await _context.SaveChangesAsync();
        return club;
    }

    //Obtener todos los clubes

    public async Task<IEnumerable<Club>> GetAllAsync()
    {
        return await _context.Club.ToListAsync();
    }
    //Obtener un club por id
    public async Task<Club?> GetByIdAsync(int id)
    {
        return await _context.Club.FindAsync(id);
    }
    //Actualizar un club
    public async Task<Club> UpdateAsync(Club club)
    {
        _context.Club.Update(club);
        await _context.SaveChangesAsync();
        return club;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var club = await _context.Club.FindAsync(id);
        if (club == null)
        {
            return false;
        }
        _context.Club.Remove(club);
        return await _context.SaveChangesAsync() > 0;
    }
}

