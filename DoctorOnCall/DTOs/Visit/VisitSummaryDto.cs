namespace DoctorOnCall.DTOs.Visit;

public class VisitSummaryDto
{
    public int Id { get; set; }
    public int visitRequestId {get; set;}
    public DateTime ActualStartDateTime { get; set; }
    public DateTime ActualEndDateTime { get; set; }
    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }
    public TimeSpan? DelayTime { get; set; }
}