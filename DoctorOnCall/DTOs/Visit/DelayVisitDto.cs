using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.DTOs.VisitDTOs;

public class DelayVisitDto
{
    [MaxLength(200, ErrorMessage = "Delay reason cannot be longer than 200 characters.")]
    [Required]
    public string DelayReason { get; set; }
}