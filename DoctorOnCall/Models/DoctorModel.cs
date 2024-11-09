using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Models;

public class DoctorModel: UserModel
{
    [Required]
    public string Specialization { get; set; }
    
    [Required]
    public DoctorStatus Status { get; set; }
    
    [Required]
    public string WorkingDistrict { get; set; }
    
    public virtual ICollection<VisitModel> Visits { get; set; }
    
    public int ScheduleId { get; set; }
    
    [ForeignKey("ScheduleId")]
    public virtual ScheduleModel Schedule { get; set; }
}

public enum DoctorStatus
{
    AVAIBLE_FOR_CALL,
    NOT_AVAIBLE_FOR_CALL
}


