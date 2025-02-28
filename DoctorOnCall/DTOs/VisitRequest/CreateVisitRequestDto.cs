using System.ComponentModel.DataAnnotations;
using DoctorOnCall.Enums;
using DoctorOnCall.ValidationAttributes.VisitRequest;

namespace DoctorOnCall.DTOs.VisitDTOs;
public class CreateVisitRequestDto
{
    [Required(ErrorMessage = "Date and time is required.")]
    [NotDefaultDate]
    [NotInPastOrNow]
    public DateTime RequestedDateTime { get; set; }

    [StringLength(200, ErrorMessage = "Request description cannot exceed 200 characters.")]
    public string? RequestDescription { get; set; }
    
    [Required(ErrorMessage = "Type is required.")]
    public VisitRequestType RequestType { get; set; }

    [Required(ErrorMessage = "IsRegularVisit is required.")]
    public bool IsRegularVisit { get; set; } = false;
    public int? RegularVisitIntervalDays { get; set; } 
    public int? RegularVisitOccurrences { get; set; } 

    public Dictionary<int, int>? RequestedMedicines { get; set; }
}
