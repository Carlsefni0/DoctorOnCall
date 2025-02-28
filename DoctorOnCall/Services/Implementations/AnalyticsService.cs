using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using DoctorOnCall.DTOs.Analytics;
using DoctorOnCall.DTOs.Visit;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Services.Interfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Services;

public class AnalyticsService: IAnalyticsService
{
    private readonly IVisitRepository _visitRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IVisitRequestRepository _visitRequestRepository;

    public AnalyticsService(IVisitRepository visitRepository, IDoctorRepository doctorRepository,IVisitRequestRepository visitRequestRepository,  UserManager<AppUser> userManager)
    {
        _visitRepository = visitRepository;
        _doctorRepository = doctorRepository;
        _userManager = userManager;
        _visitRequestRepository = visitRequestRepository;
    }
    public async Task<TravelStatsSummaryDto> GetTravelStats(AnalyticsFilterDto analyticsFilter, int userId)
    {
        await GetStatsForDoctor(userId, analyticsFilter);
        
        var metrics = new List<string> {"TotalDistance", "TotalTravelTime", "TotalTravelCost"};
        
        var stats = await GetGroupedTravelStats(analyticsFilter, metrics);

        var travelStats = new TravelStatsSummaryDto
        {
            TravelCostSum = Math.Round(stats.Sum(month => month.TotalTravelCost),2),
            TravelDistanceSum = Math.Round(stats.Sum(month => month.TotalDistance),2),
            TravelTimeSum = Math.Round(stats.Sum(month => month.TotalTravelTime),2),
            GroupedStats = stats.Select(s => new TravelStatsDto()
            {
                GroupIdentifier = s.GroupIdentifier,
                TotalTravelCost = s.TotalTravelCost,
                TotalDistance = s.TotalDistance,
                TotalTravelTime = s.TotalTravelTime,
            })
        };
        
        return travelStats;
    }
   
    public async Task<VisitStatsSummaryDto> GetVisitStats(AnalyticsFilterDto analyticsFilter, int userId)
    {
        await GetStatsForDoctor(userId, analyticsFilter);
        
        var visitRequestFilter = new VisitRequestFilterDto()
        {
            DateRangeStart = analyticsFilter.StartDate,
            DateRangeEnd = analyticsFilter.EndDate,
            DoctorId = analyticsFilter.DoctorId,
            isSchedule = false,
        };
        
        var visitFilter = ToVisitFilterDto(analyticsFilter);
        
        var requests = await _visitRequestRepository.GetVisitRequests(visitRequestFilter);
        
        var visits = await _visitRepository.GetVisits(visitFilter);
        
        var metrics = new List<string> {"TotalDelayTime"};
        
        var groupedStats = await GetGroupedTravelStats(analyticsFilter,metrics);

        
        var stats = new VisitStatsSummaryDto()
        {
            CompletedVisits = visits.Count(v => v.Status == VisitStatus.Completed),
            CancelledVisits = visits.Count(v => v.Status == VisitStatus.Cancelled),
            Consultations = requests.Count(r => r.Type == VisitRequestType.Сonsultation),
            Examinations = requests.Count(r => r.Type == VisitRequestType.Examination),
            Procedures = requests.Count(r => r.Type == VisitRequestType.Procedures),
            RemoteVisits = requests.Count(r => r.Type == VisitRequestType.RemoteVisit),
            DelayStats = groupedStats.Select(gs => new VisitStatsDto() {GroupIdentifier = gs.GroupIdentifier, TotalDelayTime = gs.TotalDelayTime})
        };

        return stats;
    }
    
    public async Task<MedicineStatsSummaryDto> GetMedicineStats(AnalyticsFilterDto analyticsFilter, int userId)
    {
        await GetStatsForDoctor(userId, analyticsFilter);
        
        var metrics = new List<string> {"TotalMedicineCost"};
        
        var groupedStats = await GetGroupedTravelStats(analyticsFilter,metrics);

        var stats = new MedicineStatsSummaryDto
        {
            
           MedicineCostSum = groupedStats.Sum(month => month.TotalMedicineCost),
           GroupedStats = groupedStats.Select(s => new MedicineStatsDto()
           {
               GroupIdentifier = s.GroupIdentifier,
               TotalMedicineCost = s.TotalMedicineCost,
           })
        };
        
        return stats;
    }
   
    
    private async Task<ICollection<GroupedStatsDto>> GetGroupedTravelStats(AnalyticsFilterDto analyticsFilter, List<string> metrics)
    {
        var filter = ToVisitFilterDto(analyticsFilter);
        var visits = await _visitRepository.GetVisits(filter);

        var validVisits = visits.Where(v => v.ActualStartDateTime.HasValue).ToList();

        var groupedData = analyticsFilter.GroupInterval switch
        {
            GroupInterval.Daily => validVisits.GroupBy(v => new 
            { 
                Key = v.ActualStartDateTime.Value.Date.ToString("dd-MM"), 
                Identifier = v.ActualStartDateTime.Value.Date.ToString("dd-MM") 
            }),
            GroupInterval.Weekly => validVisits.GroupBy(v =>
            {
                DateTime mondayOfWeek = v.ActualStartDateTime.Value.Date.AddDays(-(int)v.ActualStartDateTime.Value.DayOfWeek + (int)DayOfWeek.Monday);
                return new 
                { 
                    Key = mondayOfWeek.ToString("dd-MM"), 
                    Identifier = mondayOfWeek.ToString("dd-MM") 
                };
            }),
            _ => validVisits.GroupBy(v => new 
            { 
                Key = $"{v.ActualStartDateTime.Value.Year}-{v.ActualStartDateTime.Value.Month:D2}",
                Identifier = v.ActualStartDateTime.Value.Month.ToString()
            })
        };

        return groupedData.Select(g => new GroupedStatsDto
        {
            GroupIdentifier = g.Key.Identifier,
            TotalDistance = metrics.Contains("TotalDistance") ? g.Sum(v => v.TravelDistance) / 1000 : 0,
            TotalTravelTime = metrics.Contains("TotalTravelTime") ? Math.Round(g.Sum(v => v.TravelTime.Value.TotalMinutes) / 60, 2) : 0,
            TotalDelayTime = metrics.Contains("TotalDelayTime") ? Math.Round(g.Sum(v => v.DelayTime.Value.TotalMinutes) / 60, 2) : 0,
            TotalTravelCost = metrics.Contains("TotalTravelCost") ? g.Sum(v => v.TravelDistance * 0.25) : 0,
            TotalMedicineCost = metrics.Contains("TotalMedicineCost") ? g.Sum(v => v.MedicineCost) : 0
        }) .OrderBy(g => 
        {
            if (DateTime.TryParseExact(g.GroupIdentifier, "dd-MM", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
                return parsedDate;
            if (DateTime.TryParseExact(g.GroupIdentifier, "yyyy-MM", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                return parsedDate;
            return DateTime.MaxValue;
        })
        .ToList();
    }

    
    private VisitFilterDto ToVisitFilterDto(AnalyticsFilterDto analyticsFilter)
    {
        return new VisitFilterDto
        {
            StartDate = analyticsFilter.StartDate,
            EndDate = analyticsFilter.EndDate,
            DoctorId = analyticsFilter.DoctorId,
            Status = VisitStatus.Completed
        };
    }

    private async Task GetStatsForDoctor(int userId, AnalyticsFilterDto filter)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"The user with ID {userId} was not found");
        
        var userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.Contains("Doctor"))
        {
            var doctor = await _doctorRepository.GetDoctorByUserId(userId);
            filter.DoctorId = doctor.Id;
        }
    }

}