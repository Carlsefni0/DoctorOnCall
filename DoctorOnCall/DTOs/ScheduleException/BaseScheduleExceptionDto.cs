using System.ComponentModel.DataAnnotations;
using DoctorOnCall.ValidationAttributes;

namespace DoctorOnCall.DTOs.Vacation;


public abstract class BaseScheduleExceptionDto
{
    public DateTime? StartDate { get; set; }
    
    [DateRange("StartDate")]
    public DateTime? EndDate { get; set; }

    [MaxLength(200, ErrorMessage = "Description is too long")]
    public string? Reason { get; set; }
}

