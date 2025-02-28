using System.ComponentModel.DataAnnotations;
using DoctorOnCall.Models;
using DoctorOnCall.ValidationAttributes.Schedule;

namespace DoctorOnCall.DTOs.Schedule;

public class CreateScheduleDto
{
    [Required]
    [MaxLength(100)]
    public string ScheduleName { get; set; }
    
    [Required]
    [NotEmpty]
    [NoDuplicateDays]
    public ICollection<ScheduleDayDto> ScheduleDays { get; set; }
}