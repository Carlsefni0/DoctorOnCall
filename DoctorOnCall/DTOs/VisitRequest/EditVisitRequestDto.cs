using System.ComponentModel.DataAnnotations;
using DoctorOnCall.ValidationAttributes.VisitRequest;

namespace DoctorOnCall.DTOs.VisitRequest;

public class EditVisitRequestDto
{
    [NotDefaultDate]
    [NotInPastOrNow]
    public DateTime? RequestedDateTime { get; set; }
    [StringLength(200, ErrorMessage = "Request description cannot exceed 200 characters.")]
    public string? RequestDescription { get; set; }
    public int? RegularVisitIntervalDays { get; set; } 
    public int? RegularVisitOccurrences { get; set; } 
    public Dictionary<int, int>? RequestedMedicines { get; set; }
}
