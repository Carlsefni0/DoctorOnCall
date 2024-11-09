using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.Models;

public class DoctorSchedule
{
    public int Id { get; set; }
    
    [Required]
    public int DoctorId { get; set; }
    
    [Required]
    public int ScheduleId { get; set; }
    
    [Required]
    public DateTime ScheduleStartDate { get; set; }
    
    public DateTime? ScheduleEndDate { get; set; }
}