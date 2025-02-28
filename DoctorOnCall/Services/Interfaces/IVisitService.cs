using DoctorOnCall.DTOs.Visit;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.Models;

namespace DoctorOnCall.Services.Interfaces;

public interface IVisitService
{
    Task<VisitSummaryDto> CompleteVisit(int requestId, CompleteVisitDto completeVisit, int userId);
    Task<VisitSummaryDto> CancelVisit(int requestId, CancelVisitDto cancelVisit, int userId);
    Task DelayVisit(int visitRequestId, DelayVisitDto delayVisit, int userId);
    Task<ICollection<VisitSummaryDto>> GetVisitsByRequestId(int visitRequestId, int userId);
}