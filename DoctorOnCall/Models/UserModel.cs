using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.Models;

public class UserModel
{
    
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [PasswordPropertyText]
    public string HashedPassword { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
}