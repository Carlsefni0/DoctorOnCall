using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs.VisitRequest;

public class CancelVisitDto
{
    [MaxLength(200, ErrorMessage = "Cancellation reason cannot be longer than 200 characters.")]
    [Required]
    public string CancellationReason { get; set; }
    public DateTime VisitDateTime { get; set; }
}