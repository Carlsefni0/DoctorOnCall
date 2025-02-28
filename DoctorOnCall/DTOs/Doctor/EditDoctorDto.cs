using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs;

public class EditDoctorDto
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

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Specialization must be between 3 and 100 characters.")]
    public string? Specialization { get; set; }

    [StringLength(50, MinimumLength = 2, ErrorMessage = "WorkingDistrict must be between 2 and 50 characters.")]
    public string? WorkingDistrict { get; set; }

    [StringLength(100, MinimumLength = 5, ErrorMessage = "Hospital must be between 5 and 100 characters.")]
    public string? Hospital { get; set; }
}
