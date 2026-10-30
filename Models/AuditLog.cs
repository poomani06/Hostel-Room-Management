using System;
using System.Collections.Generic;
using System.Text;



namespace HostelRoomManagement.Models;

public class AuditLog
{
    public int AuditLogId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string Description { get; set; } = string.Empty;

    public string PerformedBy { get; set; } = "Admin";

    public DateTime Timestamp { get; set; } = DateTime.Now;
}
