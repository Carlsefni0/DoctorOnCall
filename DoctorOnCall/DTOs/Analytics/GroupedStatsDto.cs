namespace DoctorOnCall.DTOs.Analytics;

public class GroupedStatsDto
{
    public string GroupIdentifier { get; set; }
    public double TotalDistance { get; set; }
    public double TotalTravelCost { get; set; }
    public double TotalTravelTime { get; set; }
    public double TotalDelayTime { get; set; }
    public double TotalMedicineCost { get; set; }
    
}
