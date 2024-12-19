using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IVisitService
{
    Task<VisitRequestDetailsDto> CreateVisitRequest(CreateVisitRequestDto createVisitRequest, int userId);

    Task<VisitRequestDetailsDto> EditVisitRequest(int visitRequestId, CreateVisitRequestDto editVisitRequest);

    Task<VisitRequestDetailsDto> ChangeVisitRequestStatus(int visitRequestId, VisitRequestStatus status);

    Task<ICollection<VisitRequestSummaryDto>> GetVisitRequestsByPatientId(int userId,VisitRequestFilterDto filter);
    
    Task<VisitRequestDetailsDto> GetVisitRequestById(int visitRequestId, int userId);


}