using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorOnCall.Enums;

public class Visit
{
    public int Id { get; set; }

    [ForeignKey("VisitRequest")]
    public int VisitRequestId { get; set; }
    public VisitRequest VisitRequest { get; set; }
    
    public DateTime? ActualStartDateTime { get; set; }

    public DateTime? ActualEndDateTime { get; set; }

    public TimeSpan? DelayTime { get; set; }

    public double TravelDistance { get; set; }

    public TimeSpan? TravelTime { get; set; }
    
    public VisitStatus Status { get; set; }
    
    [StringLength(200)]
    public string? Notes { get; set; }
    
    [StringLength(200)]
    public string? CancellationReason { get; set; }
    
    public double MedicineCost { get; set; }

}