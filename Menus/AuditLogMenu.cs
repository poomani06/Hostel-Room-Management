
using HostelRoomManagement.Models;
using HostelRoomManagement.Services;

namespace HostelRoomManagement.Menus;

public class AuditLogMenu
{
    private readonly AuditLogService _auditLogService;

    public AuditLogMenu(AuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n===== AUDIT LOG MANAGEMENT =====");
            Console.WriteLine("1. View All Audit Logs");
            Console.WriteLine("2. Search Audit Log by ID");
            Console.WriteLine("3. View Audit Logs by Entity");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ViewAllAuditLogs();
                    break;

                case "2":
                    SearchAuditLogById();
                    break;

                case "3":
                    ViewAuditLogsByEntity();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    // Display all audit logs
    private void ViewAllAuditLogs()
    {
        List<AuditLog> logs = _auditLogService.GetAll();

        DisplayAuditLogs(logs);
    }

    // Search audit log by ID
    private void SearchAuditLogById()
    {
        Console.Write("Enter Audit Log ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            Console.WriteLine("Please enter a valid Audit Log ID.");
            return;
        }

        AuditLog? log = _auditLogService.GetById(id);

        if (log == null)
        {
            Console.WriteLine("Audit log not found.");
            return;
        }

        DisplayAuditLog(log);
    }

    // View audit logs by entity name
    private void ViewAuditLogsByEntity()
    {
        Console.Write("Enter Entity Name (Room/Resident/RoomAllocation): ");

        string? entityName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(entityName))
        {
            Console.WriteLine("Entity name is required.");
            return;
        }

        List<AuditLog> logs =
            _auditLogService.GetByEntityName(entityName.Trim());

        DisplayAuditLogs(logs);
    }

    // Display multiple audit logs
    private void DisplayAuditLogs(List<AuditLog> logs)
    {
        if (logs.Count == 0)
        {
            Console.WriteLine("No audit logs found.");
            return;
        }

        Console.WriteLine("\n===== AUDIT LOG RECORDS =====");

        foreach (AuditLog log in logs)
        {
            DisplayAuditLog(log);
        }
    }

    // Display one audit log
    private void DisplayAuditLog(AuditLog log)
    {
        Console.WriteLine("\n------------------------------");
        Console.WriteLine($"Audit Log ID : {log.AuditLogId}");
        Console.WriteLine($"Action       : {log.Action}");
        Console.WriteLine($"Entity Name  : {log.EntityName}");
        Console.WriteLine($"Entity ID    : {log.EntityId}");
        Console.WriteLine($"Description  : {log.Description}");
        Console.WriteLine($"Performed By : {log.PerformedBy}");
        Console.WriteLine($"Timestamp    : {log.Timestamp}");
        Console.WriteLine("------------------------------");
    }
}
