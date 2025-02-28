namespace DoctorOnCall.DTOs.Analytics;

public class VisitStatsSummaryDto
{
    public int CompletedVisits { get; set; }
    public int CancelledVisits { get; set; }
    public int Consultations { get; set; }
    public int Examinations { get; set; }
    public int Procedures { get; set; }
    public int RemoteVisits { get; set; }
    public IEnumerable<VisitStatsDto> DelayStats { get; set; }
}