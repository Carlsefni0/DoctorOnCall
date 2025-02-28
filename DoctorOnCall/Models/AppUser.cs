using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorOnCall.Enums;
using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Models;

public class AppUser: IdentityUser<int>
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Column(TypeName = "nvarchar(6)")]
    public Gender Gender { get; set; } 

    public ICollection<AppUserRole> UserRoles { get; set; } = [];


}


