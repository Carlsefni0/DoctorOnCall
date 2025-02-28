using DoctorOnCall.DTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.Repository.Interfaces;

public interface IMedicineService
{
    Task<ICollection<RequestedMedicineDto>> GetMedicinesByVisitRequestId(int visitRequestId, int userId);
    Task<ICollection<MedicineDto>> GetMedicines();
    Task<ICollection<MedicineDto>> FindMedicinesByName(string medicineName);
}