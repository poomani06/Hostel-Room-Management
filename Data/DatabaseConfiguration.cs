using System;
namespace HostelRoomManagement.Data
{
    public static class DatabaseConfiguration
    {
        public static string ConnectionString =
            @"Server=.\SQLEXPRESS;Database=HostelRoomManagementDB_New;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}