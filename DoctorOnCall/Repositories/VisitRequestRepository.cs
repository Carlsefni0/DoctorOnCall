using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

public class VisitRequestRepository: IVisitRequestRepository
{
    private readonly DataContext _db;

    public VisitRequestRepository(DataContext context)
    {
        _db = context;
    }


    public async Task<VisitRequest> CreateVisitRequest(VisitRequest visitRequest)
    {
        _db.VisitRequests.Add(visitRequest);
        
        await _db.SaveChangesAsync(); 
        
        return visitRequest;
    }

    public async Task<ICollection<VisitRequest>> GetVisitRequestsByPatientId(
        int patientId,
        VisitRequestFilterDto filter)
    {
        var query = _db.VisitRequests
            .Where(v => v.PatientId == patientId);

        if (filter.Year.HasValue)
        {
            query = query.Where(vr => vr.RequestedDateTime.Year == filter.Year.Value);
        }

        if (filter.Month.HasValue && filter.Year.HasValue)
        {
            query = query.Where(vr => vr.RequestedDateTime.Month == filter.Month.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(v => v.Status == filter.Status.Value);
        }

        var visitRequests = await query
            .Include(v => v.RequestedMedicines)
            .ThenInclude(rm => rm.Medicine)
            .ToListAsync();

        return visitRequests;
    }

    public async Task<VisitRequest> GetVisitRequestById(int visitRequestId)
    {
        var visitRequest = await _db.VisitRequests.Include(vr=>vr.RequestedMedicines)
            .ThenInclude(vrm=>vrm.Medicine).FirstOrDefaultAsync(vr => vr.Id == visitRequestId);;
        
        return visitRequest;
    }

    public async Task<VisitRequest> UpdateVisitRequest(VisitRequest visitRequest)
    {
        var updatedVisitRequest =  _db.VisitRequests.Update(visitRequest);
        await _db.SaveChangesAsync();
        
        return updatedVisitRequest.Entity;
    }
    
}