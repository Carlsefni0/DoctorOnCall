using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

public class VisitRequestMedicineRepository : IVisitRequestMedicineRepository
{
    private readonly DataContext _db;

    public VisitRequestMedicineRepository(DataContext db)
    {
        _db = db;
    }

    public async Task AddMedicinesAsync(ICollection<VisitRequestMedicine> medicines)
    {
        await _db.VisitRequestMedicine.AddRangeAsync(medicines);
        await _db.SaveChangesAsync();

    }

    public async Task RemoveMedicinesAsync(ICollection<VisitRequestMedicine> medicines)
    {
        _db.VisitRequestMedicine.RemoveRange(medicines);
        await _db.SaveChangesAsync();

    }

    public async Task<ICollection<VisitRequestMedicine>> GetMedicinesByVisitRequestId(int visitRequestId)
    {
        var visitRequestMedicines = await _db.VisitRequestMedicine
            .Where(vrm => vrm.VisitRequestId == visitRequestId)
            .ToListAsync();
        
        await _db.SaveChangesAsync();
        
        return visitRequestMedicines;

    }
}