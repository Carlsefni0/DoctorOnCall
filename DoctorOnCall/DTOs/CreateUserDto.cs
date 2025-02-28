using System.ComponentModel.DataAnnotations;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.DTOs;

public class CreateUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character"
    )]
    public string Password { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [MinLength(5, ErrorMessage = "First name cannot be shorter than 3 characters")]
    [MaxLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
    [RegularExpression(
        @"^[A-Za-zА-Яа-яЁёІіЇїЄє]+(([',. -][A-Za-zА-Яа-яЁёІіЇїЄє ])?[A-Za-zА-Яа-яЁёІіЇїЄє]*)*$",
        ErrorMessage = "First name contains invalid characters"
    )]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [MinLength(5, ErrorMessage = "Last name cannot be shorter than 3 characters")]
    [MaxLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
    [RegularExpression(
        @"^[A-Za-zА-Яа-яЁёІіЇїЄє]+(([',. -][A-Za-zА-Яа-яЁёІіЇїЄє ])?[A-Za-zА-Яа-яЁёІіЇїЄє]*)*$",
        ErrorMessage = "Last name contains invalid characters"
    )]
    public string LastName { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? PhoneNumber { get; set; } 

    [Required(ErrorMessage = "Gender is required")]
    [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender value")]
    public Gender Gender { get; set; }
}
