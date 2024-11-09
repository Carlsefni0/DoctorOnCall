using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.Models;

public class ScheduleModel
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }  
    
    public virtual ICollection<ScheduleDayModel> Days { get; set; }
}