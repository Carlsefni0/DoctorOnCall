using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Models;

public class AppUserRole: IdentityUserRole<int>
{
    public AppUser User { get; set; } = null!;
    
    public AppRole Role { get; set; } = null!;
    
}