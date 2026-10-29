
using HostelRoomManagement.Models;
using HostelRoomManagement.Repositories;

namespace HostelRoomManagement.Services;

public class ResidentService
{
    private readonly ResidentRepository _residentRepository;

    public ResidentService(ResidentRepository residentRepository)
    {
        _residentRepository = residentRepository;
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

    public async Task AddResidentAsync(Resident resident)
    {
        ValidateResident(resident);

        await _residentRepository.AddAsync(resident);
    }

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
    }

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

        await _residentRepository.DeleteAsync(resident);
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
