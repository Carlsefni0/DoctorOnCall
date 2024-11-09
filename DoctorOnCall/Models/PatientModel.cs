using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.Models;

public class PatientModel : UserModel
{
    [Required]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    public string Gender { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    // public virtual ICollection<VisitModel> Visits { get; set; }
}