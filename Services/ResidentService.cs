
using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories;

namespace HostelRoomManagement.Services;

public class ResidentService
{
    private readonly ResidentRepository _residentRepository;
    private readonly AuditLogService _auditLogService;

    public ResidentService(
        ResidentRepository residentRepository,
        AuditLogService auditLogService)
    {
        _residentRepository = residentRepository;
        _auditLogService = auditLogService;
    }

    public async Task<List<Resident>> GetAllResidentsAsync()
    {
        return await _residentRepository.GetAllAsync();
    }

    public async Task<Resident?> GetResidentByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Resident ID must be greater than zero.");
        }

        return await _residentRepository.GetByIdAsync(id);
    }

    // Add resident and create audit log
    public async Task AddResidentAsync(Resident resident)
    {
        ValidateResident(resident);

        await _residentRepository.AddAsync(resident);

        _auditLogService.AddLog(
            "Create",
            "Resident",
            resident.ResidentId,
            $"Resident {resident.FullName} was created");
    }

    // Update resident and create audit log
    public async Task UpdateResidentAsync(Resident resident)
    {
        if (resident.ResidentId <= 0)
        {
            throw new ArgumentException(
                "A valid resident ID is required.");
        }

        ValidateResident(resident);

        Resident? existingResident =
            await _residentRepository.GetByIdAsync(resident.ResidentId);

        if (existingResident == null)
        {
            throw new ArgumentException("Resident not found.");
        }

        existingResident.FullName = resident.FullName;
        existingResident.PhoneNumber = resident.PhoneNumber;
        existingResident.Department = resident.Department;

        await _residentRepository.UpdateAsync(existingResident);

        _auditLogService.AddLog(
            "Update",
            "Resident",
            existingResident.ResidentId,
            $"Resident {existingResident.FullName} was updated");
    }

    // Delete resident and create audit log
    public async Task DeleteResidentAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Resident ID must be greater than zero.");
        }

        Resident? resident =
            await _residentRepository.GetByIdAsync(id);

        if (resident == null)
        {
            throw new ArgumentException("Resident not found.");
        }

        string residentName = resident.FullName;

        await _residentRepository.DeleteAsync(resident);

        _auditLogService.AddLog(
            "Delete",
            "Resident",
            id,
            $"Resident {residentName} was deleted");
    }

    private static void ValidateResident(Resident resident)
    {
        if (string.IsNullOrWhiteSpace(resident.FullName))
        {
            throw new ArgumentException("Resident name is required.");
        }

        if (resident.FullName.Length > 100)
        {
            throw new ArgumentException(
                "Resident name cannot exceed 100 characters.");
        }

        if (resident.PhoneNumber?.Length > 20)
        {
            throw new ArgumentException(
                "Phone number cannot exceed 20 characters.");
        }

        if (resident.Department?.Length > 100)
        {
            throw new ArgumentException(
                "Department cannot exceed 100 characters.");
        }
    }
}
