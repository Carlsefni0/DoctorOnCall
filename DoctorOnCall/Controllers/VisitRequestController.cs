using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[ApiController]
[Route("api/visit-request")]
public class VisitRequestController: BaseController
{
    private readonly IVisitRequestService _visitRequestService;


    public VisitRequestController(IVisitRequestService visitRequestRequestsService)
    {
        _visitRequestService = visitRequestRequestsService;
    }
    
    [Authorize(Roles = "Patient")]
    [HttpPost("create")]
    public async Task<ActionResult<VisitRequestDetailsDto>> CreateVisitRequest([FromBody]CreateVisitRequestDto createVisitRequest)
    {
        var userId = GetUserId();
  
        var createdVisitRequest = await _visitRequestService.CreateVisitRequest(createVisitRequest, userId);

        return Ok(createdVisitRequest);
    }
    
    [Authorize(Roles = "Admin, Patient, Doctor")]
    [HttpGet]
    public async Task<ActionResult<ICollection<VisitRequestSummaryDto>>> GetVisitRequests([FromQuery] VisitRequestFilterDto filter)
    {
        var userId = GetUserId();
        
        var visitRequests = await _visitRequestService.GetPagedVisitRequests(filter, userId);
        
        return Ok(visitRequests);
    }
    
    [Authorize(Roles = "Admin, Patient, Doctor")]
    [HttpGet("{requestId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> GetVisitRequestById(int requestId)
    {
        var userId = GetUserId();
 
        var visitRequest = await _visitRequestService.GetVisitRequestById(requestId, userId);
        
        return Ok(visitRequest);
    }

    [Authorize(Roles = "Patient")]
    [HttpPut("{requestId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> UpdateVisitRequest([FromBody] EditVisitRequestDto updateVisitRequest, int requestId)
    {
        var userId = GetUserId();
        
        var visitRequest = await _visitRequestService.EditVisitRequest(requestId, userId, updateVisitRequest);
        
        return Ok(visitRequest);
    }

    [Authorize(Roles = "Doctor")]
    [HttpGet("assigned")]
    public async Task<ActionResult<ICollection<VisitRequestSummaryDto>>> GetAssignedVisitRequests([FromQuery] DateTime dateRangeStart, [FromQuery] DateTime dateRangeEnd)
    {
        var userId = GetUserId();
        var result = await _visitRequestService.GetAssignedVisitRequests(dateRangeStart, dateRangeEnd, userId);
        
        return Ok(result);
    }
    
    [HttpGet("current")]
    public async Task<ActionResult<DailyVisitsDto>> GetCurrentVisits([FromQuery] DateTime date)
    {
        var userId = GetUserId();
        
        var result = await _visitRequestService.GetDailyVisitRequests(date, userId);
        
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("approve/{requestId}/doctor/{doctorId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> AcceptVisitRequest(int requestId, int doctorId )
    { 
        var result = await _visitRequestService.ApproveVisitRequest(requestId, doctorId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpPatch("approve/{requestId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> AcceptRegularVisitRequest(int requestId)
    {
        var userId = GetUserId();
        
        var result = await _visitRequestService.ApproveRegularVisitRequest(requestId,userId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPatch("reject/{requestId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> RejectVisitRequest(int requestId, [FromBody] RejectVisitRequestDto resolveVisitRequest)
    {
        var result = await _visitRequestService.RejectVisitRequest(requestId, resolveVisitRequest);
        
        return Ok(result);
    }
    [Authorize(Roles = "Doctor")]
    [HttpPatch("decline/{requestId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> DeclineVisitRequest(int requestId)
    {
        var userId = GetUserId();
        
        await _visitRequestService.RejectRegularVisitRequest(requestId, userId);
        
        return Ok(new {message = "Regular visit request was declined"});
    }
    
    [Authorize(Roles = "Patient")]
    [HttpPatch("cancel/{requestId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> CancelVisitRequest(int requestId)
    {
        var userId = GetUserId();
        
        await _visitRequestService.CancelVisitRequest(requestId, userId);
        
        return Ok(new {message = "Visit request was cancelled"});
    }
   

}