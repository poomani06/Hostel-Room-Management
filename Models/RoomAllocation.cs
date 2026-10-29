using System;
using System.Collections.Generic;
using System.Text;

namespace HostelRoomManagement.Models;

public class RoomAllocation
{
    public int AllocationId { get; set; }

    public int RoomId { get; set; }
    public int ResidentId { get; set; }

    public DateTime AllocationDate { get; set; }

    public Room Room { get; set; } = null!;
    public Resident Resident { get; set; } = null!;
}
