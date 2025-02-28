using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs.Schedule;

public class UpdateScheduleDto
{
    [MaxLength(100)]
    public string? ScheduleName { get; set; }
    
    public ICollection<ScheduleDayDto>? ScheduleDays { get; set; }
}