
using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories;

namespace HostelRoomManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _auditLogRepository;

    public AuditLogService(AuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    // Create and save an audit log
    public void AddLog(
        string action,
        string entityName,
        int entityId,
        string description)
    {
        AuditLog auditLog = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Description = description,
            PerformedBy = "Admin",
            Timestamp = DateTime.Now
        };

        _auditLogRepository.Add(auditLog);
    }

    // Get all audit logs
    public List<AuditLog> GetAll()
    {
        return _auditLogRepository.GetAll();
    }

    // Search audit log by ID
    public AuditLog? GetById(int id)
    {
        return _auditLogRepository.GetById(id);
    }

    // Get audit logs by entity name
    public List<AuditLog> GetByEntityName(string entityName)
    {
        return _auditLogRepository.GetByEntityName(entityName);
    }
}
