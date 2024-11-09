using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class ScheduleDayModel
{
    public int Id { get; set; }
    
    [Required]
    public DayOfWeek DayOfWeek { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public DateTime EndTime { get; set; }
    
    
}