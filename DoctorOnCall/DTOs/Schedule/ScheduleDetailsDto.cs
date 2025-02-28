namespace DoctorOnCall.DTOs.Schedule;

public class ScheduleDetailsDto
{
    public int ScheduleId {get; set;}
    public string ScheduleName { get; set; }
    public ICollection<ScheduleDayDto> ScheduleDays { get; set; }
}