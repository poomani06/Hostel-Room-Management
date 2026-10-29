namespace HostelRoomManagement.Models;

public class Room
{
    public int RoomId { get; set; }

    public string RoomNumber { get; set; } = string.Empty;

    public string RoomType { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public int OccupiedBeds { get; set; }

    public decimal MonthlyFee { get; set; }

    public List<RoomAllocation> Allocations { get; set; } = new();
}