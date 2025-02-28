using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IScheduleDayRepository
{
    Task<ScheduleDay?> GetScheduleDay(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime);
    Task<ICollection<ScheduleDay>> GetScheduleDaysByScheduleId(int scheduleTypeId);
    Task<ScheduleDay> CreateScheduleDay(ScheduleDay scheduleDay);
    Task DeleteScheduleDay(int dayId);
    Task<int> GetMappingsCountForDay(int dayId);
}