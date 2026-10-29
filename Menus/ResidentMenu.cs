
using HostelRoomManagement.Models;
using HostelRoomManagement.Services;

namespace HostelRoomManagement.Menus;

public class ResidentMenu
{
    private readonly ResidentService _residentService;

    public ResidentMenu(ResidentService residentService)
    {
        _residentService = residentService;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine("\n--- RESIDENT MANAGEMENT ---");
            Console.WriteLine("1. Add Resident");
            Console.WriteLine("2. View All Residents");
            Console.WriteLine("3. Search Resident by ID");
            Console.WriteLine("4. Update Resident");
            Console.WriteLine("5. Delete Resident");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        await AddResidentAsync();
                        break;

                    case "2":
                        await ViewResidentsAsync();
                        break;

                    case "3":
                        await SearchResidentAsync();
                        break;

                    case "4":
                        await UpdateResidentAsync();
                        break;

                    case "5":
                        await DeleteResidentAsync();
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

    private async Task AddResidentAsync()
    {
        Resident resident = new Resident();

        Console.Write("Enter full name: ");
        resident.FullName = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter phone number: ");
        resident.PhoneNumber = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter department: ");
        resident.Department = Console.ReadLine() ?? string.Empty;

        await _residentService.AddResidentAsync(resident);

        Console.WriteLine("Resident added successfully.");
    }

    private async Task ViewResidentsAsync()
    {
        var residents = await _residentService.GetAllResidentsAsync();

        if (residents.Count == 0)
        {
            Console.WriteLine("No residents found.");
            return;
        }

        Console.WriteLine("\n--- RESIDENT LIST ---");

        foreach (var resident in residents)
        {
            DisplayResident(resident);
        }
    }

    private async Task SearchResidentAsync()
    {
        int id = ReadInt("Enter resident ID: ");

        var resident = await _residentService.GetResidentByIdAsync(id);

        if (resident == null)
        {
            Console.WriteLine("Resident not found.");
            return;
        }

        DisplayResident(resident);
    }

    private async Task UpdateResidentAsync()
    {
        int id = ReadInt("Enter resident ID to update: ");

        var resident = await _residentService.GetResidentByIdAsync(id);

        if (resident == null)
        {
            Console.WriteLine("Resident not found.");
            return;
        }

        Console.Write($"Enter full name ({resident.FullName}): ");
        string? fullName = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(fullName))
        {
            resident.FullName = fullName;
        }

        Console.Write($"Enter phone number ({resident.PhoneNumber}): ");
        string? phoneNumber = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            resident.PhoneNumber = phoneNumber;
        }

        Console.Write($"Enter department ({resident.Department}): ");
        string? department = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(department))
        {
            resident.Department = department;
        }

        await _residentService.UpdateResidentAsync(resident);

        Console.WriteLine("Resident updated successfully.");
    }

    private async Task DeleteResidentAsync()
    {
        int id = ReadInt("Enter resident ID to delete: ");

        await _residentService.DeleteResidentAsync(id);

        Console.WriteLine("Resident deleted successfully.");
    }

    private static void DisplayResident(Resident resident)
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine($"Resident ID : {resident.ResidentId}");
        Console.WriteLine($"Full Name   : {resident.FullName}");
        Console.WriteLine($"Phone Number: {resident.PhoneNumber}");
        Console.WriteLine($"Department  : {resident.Department}");
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
