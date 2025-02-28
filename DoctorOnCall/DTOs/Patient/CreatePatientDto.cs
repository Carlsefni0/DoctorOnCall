using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs;

public class CreatePatientDto : CreateUserDto
{
    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
    public string Address { get; set; }
    
    [Required(ErrorMessage = "Disease description is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Disease name must be between 2 and 100 characters.")]
    public string Disease { get; set; }
    
    [Required(ErrorMessage = "District is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "District name must be between 2 and 100 characters.")]
    public string District { get; set; }
}
