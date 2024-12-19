using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs.AuthDTOs;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Reset token is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email{ get; set; }

    [Required(ErrorMessage = "New password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirmation password is required.")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}