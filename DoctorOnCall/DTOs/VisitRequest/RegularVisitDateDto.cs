namespace DoctorOnCall.DTOs.VisitRequest;

public class RegularVisitDateDto
{
    public int Id { get; set; }
    public int VisitRequestId { get; set; }
    public DateTime VisitStartDateTime { get; set; }
    public DateTime VisitEndDateTime { get; set; }
    public bool IsReported { get; set; } = false;
}