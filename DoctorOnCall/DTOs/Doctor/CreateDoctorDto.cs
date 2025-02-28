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
    
    [Required(ErrorMessage = "Hospital  is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Address must be between 2 and 100 characters.")]
    public string Hospital { get; set; }
    
}
