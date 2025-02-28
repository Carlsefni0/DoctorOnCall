using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Vacation;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.Services.Interfaces;

public interface IScheduleExceptionService
{
    Task<ScheduleExceptionSummaryDto> CreateScheduleException(CreateScheduleExceptionDto scheduleExceptionData, int userId);
    Task<ScheduleExceptionSummaryDto> UpdateScheduleException(UpdateScheduleExceptionDto scheduleExceptionData, int scheduleExceptionId, int userId);
    Task<PagedResult<ScheduleExceptionSummaryDto>> GetPagedScheduleExceptions(ScheduleExceptionFilterDto filter, int userId);
    Task<ScheduleExceptionDetailsDto> GetScheduleExceptionById(int scheduleExceptionId, int userId);
    Task<ScheduleExceptionDetailsDto> UpdateScheduleExceptionStatus(ScheduleExceptionStatus status, int scheduleExceptionId, int userId);
    Task DeleteScheduleException(int scheduleExceptionId, int userId);
}