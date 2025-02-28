using DoctorOnCall.Enums;

namespace DoctorOnCall.DTOs.Vacation;

public class ScheduleExceptionFilterDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public List<ScheduleExceptionType>? Types { get; set; }
    public List<ScheduleExceptionStatus>? Statuses { get; set; }
    public int? DoctorId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
}