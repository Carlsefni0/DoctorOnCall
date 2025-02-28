using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DoctorOnCall.ValidationAttributes.Schedule;

namespace DoctorOnCall.DTOs.Schedule;

[TimeRange]
[WorkingHours("06:00", "22:00")]
public class ScheduleDayDto
{
    [Required]
    // [JsonConverter(typeof(JsonStringEnumConverter))]

    public DayOfWeek DayOfWeek { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public TimeSpan EndTime { get; set; }
}