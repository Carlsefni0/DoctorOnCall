using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Vacation;
using DoctorOnCall.Models;

namespace DoctorOnCall.Repositories.Interfaces;


public interface IScheduleExceptionRepository
{
    Task<PagedResult<ScheduleExceptionSummaryDto>> GetPagedScheduleExceptions(ScheduleExceptionFilterDto? filter = null);
    Task<ScheduleException> GetScheduleExceptionById(int scheduleExceptionId);
    Task<ScheduleException> CreateScheduleException(ScheduleException exception);
    // Task<ScheduleException> UpdateScheduleException(ScheduleException exception);
    Task DeleteScheduleException(int scheduleExceptionId);
    Task<bool> DoctorHasScheduleException(int doctorId, DateTime date);
}
