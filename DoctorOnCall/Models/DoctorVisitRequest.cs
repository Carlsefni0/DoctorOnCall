using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class DoctorVisitRequest
{
    [Required]
    [ForeignKey("Doctor")]
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    [Required]
    [ForeignKey("VisitRequest")]
    public int VisitRequestId { get; set; }
    public VisitRequest VisitRequest { get; set; }

    [Required]
    public DateTime AssignedDate { get; set; }
    public bool? IsApprovedByDoctor { get; set; }
}
