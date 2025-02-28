using DoctorOnCall.DTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.Repositories.Interfaces;

public interface IMedicineRepository
{
    Task<ICollection<RequestedMedicineDto>> GetMedicinesByVisitRequestId(int visitRequestId);
    Task<ICollection<Medicine>> SearchMedicinesByName(string medicineName);
    Task<ICollection<Medicine>> GetMedicines();
}