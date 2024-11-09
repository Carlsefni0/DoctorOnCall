using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace DoctorOnCall.Models;

public class VisitModel
{
    public int Id { get; set; }
  
    [Column(TypeName = "text")]
    public string Notes { get; set; }
    
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }
    
    [Required]
    public VisitStatus Status { get; set; }
  
    [Required]
    public int DoctorId { get; set; }
    
    
    
    [Required]
    public int VisitRequestId { get; set; }
    
    [ForeignKey("VisitRequestId")]
    public VisitRequestModel VisitRequest { get; set; }
    
    public virtual ICollection<MedicineModel> Medicines { get; set; }
}

public enum VisitStatus
{
    PENDING,
    COMPLETED,
    CANCELED
}