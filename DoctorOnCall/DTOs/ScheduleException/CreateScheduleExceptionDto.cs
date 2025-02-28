using System.ComponentModel.DataAnnotations;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.DTOs.Vacation;

public class CreateScheduleExceptionDto: BaseScheduleExceptionDto
{
    [Required]
    public ScheduleExceptionType ExceptionType { get; set; }

    [Required]
    public DateTime StartDate{ get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [MaxLength(200, ErrorMessage = "Description is too long")]
    public string Reason { get; set; }
}

