using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IVisitRequestRepository
{
    Task<VisitRequestModel> AddVisitRequest(VisitRequestModel visitRequest);
    
    Task<VisitRequestModel> DeleteVisitRequest(int id);
    Task<VisitRequestModel> GetVisitRequestById(int id);
    
    Task<List<VisitRequestModel>> GetVisitRequestsByDate(DateTime date);
    Task<List<VisitRequestModel>> GetVisitRequestsByPatientId(int patientId);
    
    Task<List<VisitRequestModel>> GetAllVisitRequests();
}