using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

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
    public VisitRequestType Type { get; set; }

    [Required]
    public DateTime RequestedDateTime { get; set; }

    [Required]
    public DateTime ExpectedEndDateTime { get; set; }
    [Required]
    public bool IsRegularVisit { get; set; } = false;
    public int? RegularVisitIntervalDays { get; set; } 
    public int? RegularVisitOccurrences { get; set; }
    [StringLength(200)]
    public string? RequestDescription { get; set; }

    [StringLength(200)]
    public string? RejectionReason { get; set; }
    [Required]
    [StringLength(500)]
    public string VisitAddress { get; set; }

    public Point Location { get; set; }

    [Required]
    [StringLength(100)]
    public string District { get; set; }

    public IEnumerable<VisitRequestMedicine>? RequestedMedicines { get; set; }

    public virtual ICollection<DoctorVisitRequest> DoctorVisitRequests { get; set; } = new List<DoctorVisitRequest>();

    public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
    
    public virtual ICollection<RegularVisitDate>? RegularVisitDates { get; set; } = new List<RegularVisitDate>();
}