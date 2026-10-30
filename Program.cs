
using HostelRoomManagement.Data;
using HostelRoomManagement.Menus;
using HostelRoomManagement.Repositories;
using HostelRoomManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostelRoomManagement
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            try
            {
                using var scope = host.Services.CreateScope();

                var dbContext = scope.ServiceProvider
                    .GetRequiredService<HostelDbContext>();

                bool connected =
                    await dbContext.Database.CanConnectAsync();

                if (!connected)
                {
                    Console.WriteLine(
                        "Could not connect to the database.");
                    return;
                }

                Console.WriteLine("Database connected successfully.");

                var roomMenu = scope.ServiceProvider
                    .GetRequiredService<RoomMenu>();

                var residentMenu = scope.ServiceProvider
                    .GetRequiredService<ResidentMenu>();

                var allocationMenu = scope.ServiceProvider
                    .GetRequiredService<RoomAllocationMenu>();

                var auditLogMenu = scope.ServiceProvider
                    .GetRequiredService<AuditLogMenu>();

                await ShowMainMenuAsync(
                    roomMenu,
                    residentMenu,
                    allocationMenu,
                    auditLogMenu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<HostelDbContext>(options =>
                        options.UseSqlServer(
                            DatabaseConfiguration.ConnectionString));

                    // Repositories
                    services.AddScoped<RoomRepository>();
                    services.AddScoped<ResidentRepository>();
                    services.AddScoped<RoomAllocationRepository>();
                    services.AddScoped<AuditLogRepository>();

                    // Services
                    services.AddScoped<RoomService>();
                    services.AddScoped<ResidentService>();
                    services.AddScoped<RoomAllocationService>();
                    services.AddScoped<AuditLogService>();

                    // Menus
                    services.AddScoped<RoomMenu>();
                    services.AddScoped<ResidentMenu>();
                    services.AddScoped<RoomAllocationMenu>();
                    services.AddScoped<AuditLogMenu>();
                });
        }

        private static async Task ShowMainMenuAsync(
            RoomMenu roomMenu,
            ResidentMenu residentMenu,
            RoomAllocationMenu allocationMenu,
            AuditLogMenu auditLogMenu)
        {
            while (true)
            {
                Console.WriteLine("\n===== HOSTEL ROOM MANAGEMENT =====");
                Console.WriteLine("1. Room Management");
                Console.WriteLine("2. Resident Management");
                Console.WriteLine("3. Room Allocation Management");
                Console.WriteLine("4. Audit Log Management");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await roomMenu.ShowAsync();
                        break;

                    case "2":
                        await residentMenu.ShowAsync();
                        break;

                    case "3":
                        await allocationMenu.ShowAsync();
                        break;

                    case "4":
                        auditLogMenu.ShowMenu();
                        break;

                    case "0":
                        Console.WriteLine("Exiting application...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}
