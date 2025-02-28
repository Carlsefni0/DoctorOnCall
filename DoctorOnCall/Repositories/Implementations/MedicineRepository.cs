using DoctorOnCall.DTOs;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

public class MedicineRepository: IMedicineRepository
{
    private readonly DataContext _db;

    public MedicineRepository(DataContext db)
    {
        _db = db;
    }
    public async Task<ICollection<RequestedMedicineDto>> GetMedicinesByVisitRequestId(int visitRequestId)
    {
        var medicines = await _db.Medicines
            .Where(m => m.VisitRequestMedicines.Any(vrm => vrm.VisitRequestId == visitRequestId))
            .Select(m => new RequestedMedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                ImageUrl = m.ImageUrl,
                Description = m.Description,
                UnitPrice = m.UnitPrice,
                Dosage = m.Dosage,
                Quantity = m.VisitRequestMedicines
                    .Where(vrm => vrm.VisitRequestId == visitRequestId)
                    .Select(vrm => vrm.Quantity)
                    .FirstOrDefault()
            })
            .ToListAsync();
        
        return medicines;
    }
    
    public async Task<ICollection<Medicine>> GetMedicines()
    {
        var medicines = await _db.Medicines.ToListAsync();
        
        return medicines;
    }
    
    public async Task<ICollection<Medicine>> SearchMedicinesByName(string medicineName)
    {
        var medicines = await _db.Medicines
            .Where(m => EF.Functions.Like(m.Name, $"%{medicineName}%"))
            .ToListAsync();

        return medicines;
    }


}