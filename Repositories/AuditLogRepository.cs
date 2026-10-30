
using HostelRoomManagement.Data;
using HostelRoomManagement.Models;

namespace HostelRoomManagement.Repositories;

public class AuditLogRepository
{
    private readonly HostelDbContext _context;

    public AuditLogRepository(HostelDbContext context)
    {
        _context = context;
    }

    // Save a new audit log
    public void Add(AuditLog auditLog)
    {
        _context.AuditLogs.Add(auditLog);
        _context.SaveChanges();
    }

    // Get all audit logs
    public List<AuditLog> GetAll()
    {
        return _context.AuditLogs
            .OrderByDescending(a => a.Timestamp)
            .ToList();
    }

    // Search audit log by ID
    public AuditLog? GetById(int id)
    {
        return _context.AuditLogs
            .FirstOrDefault(a => a.AuditLogId == id);
    }

    // Get audit logs by entity name
    public List<AuditLog> GetByEntityName(string entityName)
    {
        return _context.AuditLogs
            .Where(a => a.EntityName == entityName)
            .OrderByDescending(a => a.Timestamp)
            .ToList();
    }
}
