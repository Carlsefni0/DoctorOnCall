using System.Text.Json.Serialization;
using DoctorOnCall.Enums;

namespace DoctorOnCall.DTOs.Vacation;

public class ScheduleExceptionSummaryDto: BaseScheduleExceptionDto
{
    public int Id { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public ScheduleExceptionType Type { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]

    public ScheduleExceptionStatus Status { get; set; }
    
    public string FullName { get; set; }
}