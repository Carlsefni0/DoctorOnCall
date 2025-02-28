using DoctorOnCall.Enums;

namespace DoctorOnCall.DTOs.Visit;

public class VisitFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? DoctorId { get; set; }
    
    public VisitStatus? Status { get; set; }
}
