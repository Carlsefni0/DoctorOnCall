using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class VisitRequestModel
{
    public int Id { get; set; }
    
    [Required]
    public DateTime RequestedAttendingDate { get; set; }
    
    [Required]
    public int PatientId { get; set; }
    
    [Required]
    [Column(TypeName = "text")]
    public string VisitRequestDescription { get; set; }

    public VisitRequestStatus Status { get; set; } = VisitRequestStatus.WAITING;

}

public enum VisitRequestStatus
{
    APPROVED,
    DECLINED,
    WAITING,
}