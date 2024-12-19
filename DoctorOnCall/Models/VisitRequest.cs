using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorOnCall.Enums;

namespace DoctorOnCall.Models;

public class VisitRequest
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("Patient")]
    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    [Required]
    public VisitRequestStatus Status { get; set; } = VisitRequestStatus.Pending;

    [Required]
    public DateTime RequestedDateTime { get; set; }

    [StringLength(1000)]
    public string? RequestDescription { get; set; }

    [Required]
    public bool IsRegularVisit { get; set; } = false;

    [Required]
    [StringLength(500)]
    public string VisitAddress { get; set; }

    [Required]
    [StringLength(100)]
    public string District { get; set; }

    public IEnumerable<VisitRequestMedicine>? RequestedMedicines { get; set; }

    [StringLength(500)]
    public string? RejectionReason { get; set; }
}


