namespace DoctorOnCall.DTOs.Schedule;

public class ScheduleFilterDto
{
    public string? ScheduleName { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public int? DoctorId { get; set; }
}