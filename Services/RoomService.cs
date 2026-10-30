
using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories;
using HostelRoomManagement.Services;

namespace HostelRoomManagement.Services;

public class RoomService
{
    private readonly RoomRepository _roomRepository;
    private readonly AuditLogService _auditLogService;

    public RoomService(
        RoomRepository roomRepository,
        AuditLogService auditLogService)
    {
        _roomRepository = roomRepository;
        _auditLogService = auditLogService;
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _roomRepository.GetAllAsync();
    }

    public async Task<Room?> GetRoomByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Room ID must be greater than zero.");
        }

        return await _roomRepository.GetByIdAsync(id);
    }

    // Add room and create audit log
    public async Task AddRoomAsync(Room room)
    {
        if (string.IsNullOrWhiteSpace(room.RoomNumber))
        {
            throw new ArgumentException("Room number is required.");
        }

        if (string.IsNullOrWhiteSpace(room.RoomType))
        {
            throw new ArgumentException("Room type is required.");
        }

        if (room.Capacity <= 0)
        {
            throw new ArgumentException("Room capacity must be greater than zero.");
        }

        if (room.MonthlyFee < 0)
        {
            throw new ArgumentException("Monthly fee cannot be negative.");
        }

        if (room.OccupiedBeds < 0 || room.OccupiedBeds > room.Capacity)
        {
            throw new ArgumentException(
                "Occupied beds must be between zero and room capacity.");
        }

        await _roomRepository.AddAsync(room);

        _auditLogService.AddLog(
            "Create",
            "Room",
            room.RoomId,
            $"Room {room.RoomNumber} was created");
    }

    // Update room and create audit log
    public async Task UpdateRoomAsync(Room room)
    {
        if (room.RoomId <= 0)
        {
            throw new ArgumentException("A valid room ID is required.");
        }

        if (string.IsNullOrWhiteSpace(room.RoomNumber))
        {
            throw new ArgumentException("Room number is required.");
        }

        if (string.IsNullOrWhiteSpace(room.RoomType))
        {
            throw new ArgumentException("Room type is required.");
        }

        if (room.Capacity <= 0)
        {
            throw new ArgumentException("Room capacity must be greater than zero.");
        }

        if (room.MonthlyFee < 0)
        {
            throw new ArgumentException("Monthly fee cannot be negative.");
        }

        if (room.OccupiedBeds < 0 || room.OccupiedBeds > room.Capacity)
        {
            throw new ArgumentException(
                "Occupied beds must be between zero and room capacity.");
        }

        Room? existingRoom =
            await _roomRepository.GetByIdAsync(room.RoomId);

        if (existingRoom == null)
        {
            throw new ArgumentException("Room not found.");
        }

        existingRoom.RoomNumber = room.RoomNumber;
        existingRoom.RoomType = room.RoomType;
        existingRoom.Capacity = room.Capacity;
        existingRoom.OccupiedBeds = room.OccupiedBeds;
        existingRoom.MonthlyFee = room.MonthlyFee;

        await _roomRepository.UpdateAsync(existingRoom);

        _auditLogService.AddLog(
            "Update",
            "Room",
            existingRoom.RoomId,
            $"Room {existingRoom.RoomNumber} was updated");
    }

    // Delete room and create audit log
    public async Task DeleteRoomAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Room ID must be greater than zero.");
        }

        Room? room = await _roomRepository.GetByIdAsync(id);

        if (room == null)
        {
            throw new ArgumentException("Room not found.");
        }

        string roomNumber = room.RoomNumber;

        await _roomRepository.DeleteAsync(room);

        _auditLogService.AddLog(
            "Delete",
            "Room",
            id,
            $"Room {roomNumber} was deleted");
    }
}
