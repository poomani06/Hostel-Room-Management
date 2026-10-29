using HostelRoomManagement.Data;
using HostelRoomManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HostelRoomManagement.Repositories;

public class ResidentRepository
{
    private readonly HostelDbContext _context;

    public ResidentRepository(HostelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Resident>> GetAllAsync()
    {
        return await _context.Residents.ToListAsync();
    }

    public async Task<Resident?> GetByIdAsync(int id)
    {
        return await _context.Residents.FindAsync(id);
    }

    public async Task AddAsync(Resident resident)
    {
        _context.Residents.Add(resident);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Resident resident)
    {
        _context.Residents.Update(resident);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Resident resident)
    {
        _context.Residents.Remove(resident);
        await _context.SaveChangesAsync();
    }
}