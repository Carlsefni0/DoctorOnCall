using System.ComponentModel.DataAnnotations;
using DoctorOnCall.ValidationAttributes;

namespace DoctorOnCall.DTOs.VisitRequest;

public class CompleteVisitDto
{
    [Required(ErrorMessage = "Notes are required")]
    [StringLength(200)]
    public string Notes { get; set; }
    
    [Required]
    public DateTime ActualVisitStartDateTime { get; set; }
    [Required]
    [DateRange("ActualVisitStartDateTime", ErrorMessage = "Visit end time cannot be earlier than actual visit start time")]
    public DateTime ActualVisitEndDateTime { get; set; }

}