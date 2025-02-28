using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class Medicine
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    public string ImageUrl { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }

    public double UnitPrice { get; set; }

    [Required]
    [StringLength(50)]
    public string Dosage { get; set; }
    
    public ICollection<VisitRequestMedicine>? VisitRequestMedicines { get; set; } = new List<VisitRequestMedicine>();
    
}