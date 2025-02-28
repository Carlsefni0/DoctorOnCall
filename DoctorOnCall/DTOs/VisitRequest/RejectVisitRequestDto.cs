using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs.VisitRequestDTOs;

public class RejectVisitRequestDto
{
    [MaxLength(200, ErrorMessage = "Rejection reason cannot be longer than 200 characters.")]
    [Required]
    public string RejectionReason { get; set; }
}