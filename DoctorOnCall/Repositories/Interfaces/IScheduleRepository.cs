using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Schedule;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IScheduleRepository
{
    Task<ScheduleType> CreateSchedule(ScheduleType schedule);
    Task<ScheduleType> GetScheduleById(int scheduleId);
    Task<ICollection<ScheduleType>> GetSchedulesByDoctorId(int doctorId);
    Task<PagedResult<ScheduleDetailsDto>> GetPagedSchedules(ScheduleFilterDto filter);
    Task<ICollection<ScheduleSummaryDto>> GetSchedulesNames();
    Task<ScheduleType> GetScheduleByName(string scheduleName);
    Task DeleteSchedule(int scheduleId);
    Task<ScheduleDayMapping?> GetDoctorWorkingDayByDate(int doctorId, DateTime date);

}