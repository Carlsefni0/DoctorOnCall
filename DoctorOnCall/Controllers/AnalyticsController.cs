using DoctorOnCall.DTOs.Analytics;
using DoctorOnCall.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[ApiController]
[Authorize(Roles = "Admin, Doctor")]
[Route("api/analytics")]
public class AnalyticsController: BaseController
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }
    [HttpGet("travel")]
    public async Task<ActionResult<TravelStatsSummaryDto>> GetTravelStats([FromQuery] AnalyticsFilterDto analyticParams)
    {
        var userId = GetUserId();
        var result = await _analyticsService.GetTravelStats(analyticParams, userId);
        
        return Ok(result);
    }
    [HttpGet("visit")]
    public async Task<ActionResult<VisitStatsSummaryDto>> GetVisitStats([FromQuery] AnalyticsFilterDto analyticParams)
    {
        var userId = GetUserId();
        var result = await _analyticsService.GetVisitStats(analyticParams, userId);
        
        return Ok(result);
    }
    [HttpGet("medicine")]
    public async Task<ActionResult<MedicineStatsSummaryDto>> GetTravelTimeStats([FromQuery] AnalyticsFilterDto analyticParams)
    {
        var userId = GetUserId();
        var result = await _analyticsService.GetMedicineStats(analyticParams, userId);
        
        return Ok(result);
    }
    
}