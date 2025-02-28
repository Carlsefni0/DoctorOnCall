using DoctorOnCall.Enums;
using DoctorOnCall.ValidationAttributes;

namespace DoctorOnCall.DTOs.Analytics;

public class AnalyticsFilterDto
{
    public GroupInterval GroupInterval { get; set; }
    public DateTime? StartDate { get; set; }
    [DateRange("StartDate")]
    public DateTime? EndDate { get; set; }
    public int? DoctorId { get; set; }
}