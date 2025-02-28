namespace DoctorOnCall.Models;

public class ScheduleDayMapping
{
    public int ScheduleTypeId { get; set; }
    public ScheduleType ScheduleType { get; set; }

    public int ScheduleDayId { get; set; }
    public ScheduleDay ScheduleDay { get; set; }
}