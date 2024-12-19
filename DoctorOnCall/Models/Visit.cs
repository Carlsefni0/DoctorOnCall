using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorOnCall.Models;

public class Visit
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("VisitRequest")]
    public int VisitRequestId { get; set; }
    public VisitRequest VisitRequest { get; set; }

    [Required]
    public VisitStatus Status { get; set; }
    public DateTime? ActualStartDateTime { get; set; }
    public DateTime? ActualEndDateTime { get; set; }

    [Required]
    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public List<Medicine>? Medicines { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? TravelDistance { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? MedicineCost { get; set; }

    [StringLength(500)]
    public string? CancellationReason { get; set; }

    [StringLength(500)]
    public string? DelayReason { get; set; }
}


public enum VisitStatus
{
    Pending,
    Completed,
    Canceled,
}