namespace DoctorOnCall.DTOs;

public class MedicineDto
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string ImageUrl { get; set; }
    
    public string? Description { get; set; }

    public double? UnitPrice { get; set; }

    public string Dosage { get; set; }
}