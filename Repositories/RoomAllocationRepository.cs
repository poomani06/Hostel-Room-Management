using HostelRoomManagement.Data;
using HostelRoomManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HostelRoomManagement.Repositories;

public class RoomAllocationRepository
{
    private readonly HostelDbContext _context;

    public RoomAllocationRepository(HostelDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomAllocation>> GetAllAsync()
    {
        return await _context.RoomAllocations
            .Include(a => a.Room)
            .Include(a => a.Resident)
            .ToListAsync();
    }

    public async Task<RoomAllocation?> GetByIdAsync(int id)
    {
        return await _context.RoomAllocations
            .Include(a => a.Room)
            .Include(a => a.Resident)
            .FirstOrDefaultAsync(a => a.AllocationId == id);
    }

    public async Task AddAsync(RoomAllocation allocation)
    {
        _context.RoomAllocations.Add(allocation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(RoomAllocation allocation)
    {
        _context.RoomAllocations.Remove(allocation);
        await _context.SaveChangesAsync();
    }
}