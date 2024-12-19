using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IVisitRequestRepository
{
    Task<VisitRequest> CreateVisitRequest(VisitRequest visitRequest);
    Task<ICollection<VisitRequest>> GetVisitRequestsByPatientId(int patientId, VisitRequestFilterDto filter);

    Task<VisitRequest> GetVisitRequestById(int visitRequestId);
    
    Task<VisitRequest> UpdateVisitRequest(VisitRequest visitRequest);
    



}