using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class MedicineModel
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [Required]
    [Column(TypeName = "text")]
    public string MedicineDescription { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    
    [Required]
    [Range(0, double.MaxValue)]
    public double Mass { get; set; }
    
}