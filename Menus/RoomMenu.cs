
using HostelRoomManagement.Models;
using HostelRoomManagement.Services;

namespace HostelRoomManagement.Menus;

public class RoomMenu
{
    private readonly RoomService _roomService;

    public RoomMenu(RoomService roomService)
    {
        _roomService = roomService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- ROOM MANAGEMENT ---");
            Console.WriteLine("1. Add Room");
            Console.WriteLine("2. View All Rooms");
            Console.WriteLine("3. Search Room by ID");
            Console.WriteLine("4. Update Room");
            Console.WriteLine("5. Delete Room");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await AddRoomAsync();
                        break;

                    case "2":
                        await ViewRoomsAsync();
                        break;

                    case "3":
                        await SearchRoomAsync();
                        break;

                    case "4":
                        await UpdateRoomAsync();
                        break;

                    case "5":
                        await DeleteRoomAsync();
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

    private async Task AddRoomAsync()
    {
        Room room = new Room();

        Console.Write("Enter room number: ");
        room.RoomNumber = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter room type (Single/Double/Shared): ");
        room.RoomType = Console.ReadLine() ?? string.Empty;

        room.Capacity = ReadInt("Enter room capacity: ");
        room.MonthlyFee = ReadDecimal("Enter monthly fee: ");

        room.OccupiedBeds = 0;

        await _roomService.AddRoomAsync(room);

        Console.WriteLine("Room added successfully.");
    }

    private async Task ViewRoomsAsync()
    {
        var rooms = await _roomService.GetAllRoomsAsync();

        if (rooms.Count == 0)
        {
            Console.WriteLine("No rooms found.");
            return;
        }

        Console.WriteLine("\n--- ROOM LIST ---");

        foreach (var room in rooms)
        {
            DisplayRoom(room);
        }
    }

    private async Task SearchRoomAsync()
    {
        int id = ReadInt("Enter room ID: ");

        var room = await _roomService.GetRoomByIdAsync(id);

        if (room == null)
        {
            Console.WriteLine("Room not found.");
            return;
        }

        DisplayRoom(room);
    }

    private async Task UpdateRoomAsync()
    {
        int id = ReadInt("Enter room ID to update: ");

        var room = await _roomService.GetRoomByIdAsync(id);

        if (room == null)
        {
            Console.WriteLine("Room not found.");
            return;
        }

        Console.Write($"Enter room number ({room.RoomNumber}): ");
        string? roomNumber = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(roomNumber))
        {
            room.RoomNumber = roomNumber;
        }

        Console.Write($"Enter room type ({room.RoomType}): ");
        string? roomType = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(roomType))
        {
            room.RoomType = roomType;
        }

        int capacity = ReadOptionalInt(
            $"Enter capacity ({room.Capacity}): ", room.Capacity);

        if (capacity < room.OccupiedBeds)
        {
            Console.WriteLine(
                "Capacity cannot be less than the number of occupied beds.");
            return;
        }

        room.Capacity = capacity;

        room.MonthlyFee = ReadOptionalDecimal(
            $"Enter monthly fee ({room.MonthlyFee}): ", room.MonthlyFee);

        await _roomService.UpdateRoomAsync(room);

        Console.WriteLine("Room updated successfully.");
    }

    private async Task DeleteRoomAsync()
    {
        int id = ReadInt("Enter room ID to delete: ");

        await _roomService.DeleteRoomAsync(id);

        Console.WriteLine("Room deleted successfully.");
    }

    private static void DisplayRoom(Room room)
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine($"Room ID      : {room.RoomId}");
        Console.WriteLine($"Room Number  : {room.RoomNumber}");
        Console.WriteLine($"Room Type    : {room.RoomType}");
        Console.WriteLine($"Capacity     : {room.Capacity}");
        Console.WriteLine($"Occupied Beds: {room.OccupiedBeds}");
        Console.WriteLine($"Available Beds: {room.Capacity - room.OccupiedBeds}");
        Console.WriteLine($"Monthly Fee  : {room.MonthlyFee:C}");
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

    private static decimal ReadDecimal(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (decimal.TryParse(Console.ReadLine(), out decimal value))
            {
                return value;
            }

            Console.WriteLine("Please enter a valid amount.");
        }
    }

    private static int ReadOptionalInt(string message, int currentValue)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (int.TryParse(input, out int value))
            {
                return value;
            }

            Console.WriteLine("Please enter a valid whole number.");
        }
    }

    private static decimal ReadOptionalDecimal(
        string message, decimal currentValue)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (decimal.TryParse(input, out decimal value))
            {
                return value;
            }

            Console.WriteLine("Please enter a valid amount.");
        }
    }
}
