using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.Analytics;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IVisitRequestRepository
{
    Task<VisitRequest> CreateVisitRequest(VisitRequest visitRequest);
    Task<VisitRequest> GetVisitRequestById(int visitRequestId);
    Task<VisitRequestDetailsDto> GetVisitRequestDetailsById(int visitRequestId);
    Task<ICollection<VisitRequest>> GetVisitRequests(VisitRequestFilterDto? filter);
    Task<PagedResult<VisitRequestSummaryDto>> GetPagedVisitRequestsSummary(VisitRequestFilterDto? filter = null);
    Task<ICollection<AssignedVisitRequestDto>> GetAssignedVisitRequests(VisitRequestFilterDto filter);
    Task<ICollection<CurrentVisitRequestDto>> GetCurrentVisitRequests(VisitRequestFilterDto filter);
    Task RemoveRegularVisitDates(IEnumerable<RegularVisitDate> regularVisitDates);
    Task<DoctorVisitRequest> UpdateDoctorVisitRequest(DoctorVisitRequest visitRequest);
}