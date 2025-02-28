using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DoctorOnCall.Enums;

namespace DoctorOnCall.DTOs.Vacation;

public class ScheduleExceptionDetailsDto : BaseScheduleExceptionDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScheduleExceptionType Type { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ScheduleExceptionStatus Status { get; set; }
    
    public string DoctorFirstName { get; set; }
    public string DoctorLastName { get; set; }
    public string DoctorEmail { get; set; }
}
