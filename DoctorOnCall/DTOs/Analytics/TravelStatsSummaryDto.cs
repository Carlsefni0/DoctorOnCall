namespace DoctorOnCall.DTOs.Analytics;

public class TravelStatsSummaryDto
{
    public double TravelCostSum { get; set; }
    public double TravelDistanceSum { get; set; }
    public double TravelTimeSum { get; set; }
    public IEnumerable<TravelStatsDto> GroupedStats { get; set; }
}