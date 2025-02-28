using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.Models;

public class RegularVisitDate
{
    public int Id { get; set; }
    
    [Required]
    public int VisitRequestId { get; set; }
    [Required]
    public DateTime VisitStartDateTime { get; set; }
    public DateTime VisitEndDateTime { get; set; }
    public VisitRequest VisitRequest { get; set; }
    public bool IsReported { get; set; } = false;
}
