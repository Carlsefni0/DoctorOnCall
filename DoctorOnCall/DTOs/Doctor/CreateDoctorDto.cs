using System.ComponentModel.DataAnnotations;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.DTOs;

public class CreateDoctorDto : CreateUserDto
{
    [Required(ErrorMessage = "Specialization is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Specialization must be between 2 and 100 characters.")]
    public string Specialization { get; set; }
    
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Working district must be between 2 and 100 characters.")]
    public string WorkingDistrict { get; set; }
    
    [Required(ErrorMessage = "Status is required.")]
    public DoctorStatus Status { get; set; }
    
    [Required(ErrorMessage = "Schedule ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Schedule ID must be a positive number.")]
    public int ScheduleId { get; set; }
}
