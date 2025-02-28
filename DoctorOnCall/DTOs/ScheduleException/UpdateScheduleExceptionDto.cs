using DoctorOnCall.Enums;

namespace DoctorOnCall.DTOs.Vacation;

public class UpdateScheduleExceptionDto : BaseScheduleExceptionDto
{
    public ScheduleExceptionType? ExceptionType { get; set; }
}
