using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class VisitRequestMedicine
{
    public int Id { get; set; }

    [Required]
    [ForeignKey("VisitRequest")]
    public int VisitRequestId { get; set; }
    public VisitRequest VisitRequest { get; set; }

    [Required]
    [ForeignKey("Medicine")]
    public int MedicineId { get; set; }
    public Medicine Medicine { get; set; }

    [Required]
    public int Quantity { get; set; }
}