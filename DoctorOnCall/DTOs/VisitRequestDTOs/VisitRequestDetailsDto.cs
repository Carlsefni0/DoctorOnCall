
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.DTOs.VisitRequestDTOs;

public class VisitRequestDetailsDto
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public VisitRequestStatus Status { get; set; }

    public DateTime RequestedDateTime { get; set; }

    public string? RequestDescription { get; set; }

    public bool IsRegularVisit { get; set; } = false;

    public string VisitAddress { get; set; }

    public string District { get; set; }

    public ICollection<MedicineDto> RequestedMedicines { get; set; }

    public string? RejectionReason { get; set; }
}