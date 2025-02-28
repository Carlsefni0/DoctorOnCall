namespace DoctorOnCall.DTOs.Analytics;

public class MedicineStatsSummaryDto
{
    public double MedicineCostSum { get; set; }
    public IEnumerable<MedicineStatsDto> GroupedStats { get; set; }
}