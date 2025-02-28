using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Schedule;

namespace DoctorOnCall.ServiceInterfaces;

public interface IScheduleService
{
    Task<ScheduleDetailsDto> CreateSchedule(CreateScheduleDto scheduleData);
    
    Task<ScheduleDetailsDto> GetScheduleById(int scheduleId, int userId);
    Task<ICollection<ScheduleSummaryDto>> GetSchedulesNames();

    Task<PagedResult<ScheduleDetailsDto>> GetPagedSchedules(ScheduleFilterDto filter, int userId);
    Task<ScheduleDetailsDto> UpdateSchedule(int scheuleId, CreateScheduleDto scheduleData);
    
    Task DeleteSchedule(int scheduleId);

    Task AssignScheduleToDoctor(int scheduleId, int userId);

    Task RemoveScheduleFromDoctor(int doctorId, int scheduleId);

    Task<bool> IsWithinDoctorSchedule(int doctorId, DateTime startDate, DateTime endDate);

}