using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class ScheduleDay
{
    public int Id { get; set; }
    
    [Required]
    public DayOfWeek DayOfWeek { get; set; } // День тижня

    [Required]
    public TimeSpan StartTime { get; set; } // Початок роботи

    [Required]
    public TimeSpan EndTime { get; set; } // Кінець роботи
    
    public ICollection<ScheduleDayMapping> ScheduleDayMappings { get; set; } = new List<ScheduleDayMapping>(); // Ініціалізація

    
}