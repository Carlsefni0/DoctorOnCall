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
    
    public async Task<VisitRequestModel> AddVisitRequest(VisitRequestModel VisitRequest)
    {
        await _db.VisitRequests.AddAsync(VisitRequest);
        await _db.SaveChangesAsync();
        
        return VisitRequest;
    }

    public Task<VisitRequestModel> DeleteVisitRequest(int id)
    {
        throw new NotImplementedException();
    }

    public Task<VisitRequestModel> GetVisitRequestById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<VisitRequestModel>> GetVisitRequestsByDate(DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task<List<VisitRequestModel>> GetVisitRequestsByPatientId(int patientId)
    {
        throw new NotImplementedException();
    }

    public Task<List<VisitRequestModel>> GetAllVisitRequests()
    {
        throw new NotImplementedException();
    }
}