namespace DoctorOnCall.DTOs.Analytics;

public class TravelStatsDto
{
    public string GroupIdentifier { get; set; }
    public double TotalDistance { get; set; }
    public double TotalTravelCost { get; set; }
    public double TotalTravelTime { get; set; }
}