using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.ServiceInterfaces;

public interface IVisitRequestService
{
    Task<VisitRequestDetailsDto> CreateVisitRequest(CreateVisitRequestDto createVisitRequest, int userId);
    Task<VisitRequestDetailsDto> EditVisitRequest(int visitRequestId, int userId, EditVisitRequestDto editVisitRequest);
    Task<PagedResult<VisitRequestSummaryDto>> GetPagedVisitRequests(VisitRequestFilterDto filter, int userId);
    Task<ICollection<AssignedVisitRequestDto>> GetAssignedVisitRequests(DateTime dateRangeStart, DateTime dateRangeEnd, int userId);
    Task<DailyVisitsDto> GetDailyVisitRequests(DateTime date, int userId); 
    Task<VisitRequestDetailsDto> GetVisitRequestById(int visitRequestId, int userId);
    Task<VisitRequestDetailsDto> ApproveVisitRequest(int visitRequestId, int userId);
    Task<VisitRequestDetailsDto> ApproveRegularVisitRequest(int visitRequestId, int userId);
    Task<VisitRequestDetailsDto> RejectVisitRequest(int visitRequestId, RejectVisitRequestDto rejectVisitRequest);
    Task RejectRegularVisitRequest(int visitRequestId, int userId);
    Task<VisitRequestDetailsDto> CancelVisitRequest(int visitRequestId, int userId);


}