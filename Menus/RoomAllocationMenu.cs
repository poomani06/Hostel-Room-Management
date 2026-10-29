
using HostelRoomManagement.Services;

namespace HostelRoomManagement.Menus;

public class RoomAllocationMenu
{
    private readonly RoomAllocationService _allocationService;

    public RoomAllocationMenu(RoomAllocationService allocationService)
    {
        _allocationService = allocationService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- ROOM ALLOCATION MANAGEMENT ---");
            Console.WriteLine("1. Allocate Room to Resident");
            Console.WriteLine("2. View All Allocations");
            Console.WriteLine("3. Remove Allocation");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await AllocateRoomAsync();
                        break;

                    case "2":
                        await ViewAllocationsAsync();
                        break;

                    case "3":
                        await RemoveAllocationAsync();
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private async Task AllocateRoomAsync()
    {
        int roomId = ReadInt("Enter room ID: ");
        int residentId = ReadInt("Enter resident ID: ");

        await _allocationService.AllocateRoomAsync(roomId, residentId);

        Console.WriteLine("Room allocated successfully.");
    }

    private async Task ViewAllocationsAsync()
    {
        var allocations = await _allocationService.GetAllAllocationsAsync();

        if (allocations.Count == 0)
        {
            Console.WriteLine("No room allocations found.");
            return;
        }

        Console.WriteLine("\n--- ROOM ALLOCATION LIST ---");

        foreach (var allocation in allocations)
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine($"Allocation ID : {allocation.AllocationId}");
            Console.WriteLine($"Room ID       : {allocation.RoomId}");
            Console.WriteLine($"Room Number   : {allocation.Room.RoomNumber}");
            Console.WriteLine($"Resident ID   : {allocation.ResidentId}");
            Console.WriteLine($"Resident Name : {allocation.Resident.FullName}");
            Console.WriteLine($"Allocation Date: {allocation.AllocationDate:g}");
        }
    }

    private async Task RemoveAllocationAsync()
    {
        int allocationId = ReadInt("Enter allocation ID to remove: ");

        await _allocationService.RemoveAllocationAsync(allocationId);

        Console.WriteLine("Room allocation removed successfully.");
    }

    private static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }

            Console.WriteLine("Please enter a valid whole number.");
        }
    }
}
