using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IVisitRequestsService
{
    public Task<VisitRequestModel> CreateVisitRequest(VisitRequestModel visitRequest);
}