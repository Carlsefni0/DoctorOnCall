using DoctorOnCall.DTOs.Analytics;

namespace DoctorOnCall.Services.Interfaces;

public interface IAnalyticsService
{
    Task<TravelStatsSummaryDto> GetTravelStats(AnalyticsFilterDto analyticParams, int userId);
    Task<VisitStatsSummaryDto> GetVisitStats(AnalyticsFilterDto analyticsFilter, int userId);
    Task<MedicineStatsSummaryDto> GetMedicineStats(AnalyticsFilterDto analyticsFilter, int userId);

}