using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs.AuthDTOs;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
}