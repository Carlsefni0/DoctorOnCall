using System.ComponentModel.DataAnnotations;
using DoctorOnCall.ValidationAttributes;

namespace DoctorOnCall.DTOs;

public class EditPatientDto
{
    [MinLength(5, ErrorMessage = "First name cannot be shorter than 3 characters")]
    [MaxLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    [RegularExpression(
        @"^[A-Za-zА-Яа-яЁёІіЇїЄє]+(([',. -][A-Za-zА-Яа-яЁёІіЇїЄє ])?[A-Za-zА-Яа-яЁёІіЇїЄє]*)*$",
        ErrorMessage = "First name contains invalid characters"
    )]
    public string? FirstName { get; set; }

    [MinLength(5, ErrorMessage = "Last name cannot be shorter than 3 characters")]
    [MaxLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    [RegularExpression(
        @"^[A-Za-zА-Яа-яЁёІіЇїЄє]+(([',. -][A-Za-zА-Яа-яЁёІіЇїЄє ])?[A-Za-zА-Яа-яЁёІіЇїЄє]*)*$",
        ErrorMessage = "Last name contains invalid characters"
    )]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
    public string? Address { get; set; }
    
    [Required(ErrorMessage = "Disease description is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Disease name must be between 2 and 100 characters.")]
    public string? Disease { get; set; }
    
    [Required(ErrorMessage = "District is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "District name must be between 2 and 100 characters.")]
    public string? District { get; set; }
    
    [NotFutureDate]
    public DateTime? DateOfBirth { get; set; }
}