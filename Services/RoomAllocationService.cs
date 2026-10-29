
using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories;

namespace HostelRoomManagement.Services;

public class RoomAllocationService
{
    private readonly RoomAllocationRepository _allocationRepository;
    private readonly RoomRepository _roomRepository;
    private readonly ResidentRepository _residentRepository;

    public RoomAllocationService(
        RoomAllocationRepository allocationRepository,
        RoomRepository roomRepository,
        ResidentRepository residentRepository)
    {
        _allocationRepository = allocationRepository;
        _roomRepository = roomRepository;
        _residentRepository = residentRepository;
    }

    public async Task<List<RoomAllocation>> GetAllAllocationsAsync()
    {
        return await _allocationRepository.GetAllAsync();
    }

    public async Task AllocateRoomAsync(int roomId, int residentId)
    {
        if (roomId <= 0 || residentId <= 0)
        {
            throw new ArgumentException(
                "Room ID and Resident ID must be greater than zero.");
        }

        var room = await _roomRepository.GetByIdAsync(roomId);

        if (room == null)
        {
            throw new ArgumentException("Room not found.");
        }

        var resident = await _residentRepository.GetByIdAsync(residentId);

        if (resident == null)
        {
            throw new ArgumentException("Resident not found.");
        }

        if (room.OccupiedBeds >= room.Capacity)
        {
            throw new ArgumentException("This room has no available beds.");
        }

        var allocations = await _allocationRepository.GetAllAsync();

        if (allocations.Any(a => a.ResidentId == residentId))
        {
            throw new ArgumentException(
                "This resident already has a room allocation.");
        }

        var allocation = new RoomAllocation
        {
            RoomId = roomId,
            ResidentId = residentId,
            AllocationDate = DateTime.Now
        };

        room.OccupiedBeds++;

        await _roomRepository.UpdateAsync(room);
        await _allocationRepository.AddAsync(allocation);
    }

    public async Task RemoveAllocationAsync(int allocationId)
    {
        if (allocationId <= 0)
        {
            throw new ArgumentException(
                "Allocation ID must be greater than zero.");
        }

        var allocation =
            await _allocationRepository.GetByIdAsync(allocationId);

        if (allocation == null)
        {
            throw new ArgumentException("Room allocation not found.");
        }

        var room = await _roomRepository.GetByIdAsync(allocation.RoomId);

        if (room != null && room.OccupiedBeds > 0)
        {
            room.OccupiedBeds--;
            await _roomRepository.UpdateAsync(room);
        }

        await _allocationRepository.DeleteAsync(allocation);
    }
}
