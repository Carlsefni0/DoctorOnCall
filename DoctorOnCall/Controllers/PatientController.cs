using System.Security.Claims;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Enums;

using DoctorOnCall.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[ApiController]
[Authorize(Roles = "Patient")]
[Route("api/patient")]
public class PatientController: ControllerBase
{
    private readonly IVisitService _visitService;

    public PatientController(IVisitService visitRequestsService)
    {
        _visitService = visitRequestsService;
    }
    
    [HttpPost("create-visit-request")]
    public async Task<ActionResult<VisitRequestDetailsDto>> CreateVisitRequest([FromBody]CreateVisitRequestDto createVisitRequest)
    {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found in token");;
        var createdVisitRequest = await _visitService.CreateVisitRequest(createVisitRequest, 1);

        return Ok(createdVisitRequest);
    }

    [HttpGet("get-visit-requests")]
    public async Task<ActionResult<ICollection<VisitRequestSummaryDto>>> GetMyVisitRequests([FromQuery] VisitRequestFilterDto filter)
    {
        //TODO: patient id is stored in accessToken;
        
        int userId = 1;
        var visitRequests = await _visitService.GetVisitRequestsByPatientId(userId,filter);
        
        return Ok(visitRequests);
    }

    [HttpGet("get-visit-request/{visitRequestId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> GetVisitRequest(int visitRequestId)
    {
        int userId = 1;
        var visitRequest = await _visitService.GetVisitRequestById(visitRequestId, userId);
        
        return Ok(visitRequest);
    }

    [HttpPatch("update-visit-request/{requestId}")]
    public async Task<ActionResult<VisitRequestDetailsDto>> UpdateVisitRequest([FromBody] CreateVisitRequestDto updateVisitRequest, int requestId)
    {
        var visitRequest = await _visitService.EditVisitRequest(requestId, updateVisitRequest);
        
        return Ok(visitRequest);
    }

    [HttpPatch("cancel-visit-request/{requestId}")]
    public async Task<IActionResult> CancelVisitRequest(int requestId)
    {
        await _visitService.ChangeVisitRequestStatus(requestId, VisitRequestStatus.Cancelled);
        return Ok("Visit request cancelled successfully.");
    }
    

}