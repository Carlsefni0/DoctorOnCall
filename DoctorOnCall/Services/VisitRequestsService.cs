using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Services;

public class VisitRequestsService : IVisitRequestsService
{
    private readonly IVisitRequestRepository _visitRequestRepository;

    public VisitRequestsService(IVisitRequestRepository visitRequestRepository)
    {
        _visitRequestRepository = visitRequestRepository;
    }
    public async Task<VisitRequestModel> CreateVisitRequest(VisitRequestModel visitRequest)
    {
        return  await _visitRequestRepository.AddVisitRequest(visitRequest);
    }
}