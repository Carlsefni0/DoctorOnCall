using DoctorOnCall.DTOs.Visit;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[ApiController]
[Route("/api/visit")]
public class VisitController: BaseController
{
    private readonly IVisitService _visitService;

    public VisitController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [Authorize(Roles = "Admin, Doctor, Patient")]
    [HttpGet("{visitRequestId}")]
    public async Task<ActionResult<ICollection<VisitSummaryDto>>> GetVisitsById(int visitRequestId)
    {
        var userId = GetUserId();
        var result = await _visitService.GetVisitsByRequestId(visitRequestId, userId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpPatch("complete/{visitRequestId}")]
    public async Task<ActionResult<VisitSummaryDto>> CompleteVisit(int visitRequestId, CompleteVisitDto completeVisit)
    {
        var userId = GetUserId();
        var result = await _visitService.CompleteVisit(visitRequestId,  completeVisit,userId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpPost("delay/{visitRequestId}")]
    public async Task<ActionResult> DelayVisit(int visitRequestId, DelayVisitDto delayVisit)
    {
        var userId = GetUserId();
        await _visitService.DelayVisit(visitRequestId, delayVisit,userId);
        
        return Ok(new { message = "The patient was notified" });
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpPatch("cancel/{visitRequestId}")]
    public async Task<ActionResult<VisitSummaryDto>> CancelVisit(int visitRequestId, CancelVisitDto cancelVisit)
    {
        var userId = GetUserId();
        var result = await _visitService.CancelVisit(visitRequestId,  cancelVisit, userId);
        
        return Ok(result);
    }
}