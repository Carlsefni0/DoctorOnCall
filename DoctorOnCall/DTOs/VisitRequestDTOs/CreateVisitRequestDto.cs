using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs.VisitDTOs;

public class CreateVisitRequestDto
{
    [Required]
    public DateTime RequestedDateTime { get; set; }

    [StringLength(1000)]
    public string? RequestDescription { get; set; }

    [Required]
    public bool IsRegularVisit { get; set; }
    
    public Dictionary<int,int>? requestedMedicines { get; set; }
}