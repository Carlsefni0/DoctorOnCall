using DoctorOnCall.DTOs.Vacation;
using DoctorOnCall.Enums;
using DoctorOnCall.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[ApiController]
[Route("api/schedule-exception")]
public class ScheduleExceptionController: BaseController
{
    private readonly IScheduleExceptionService _scheduleExceptionService;

    public ScheduleExceptionController(IScheduleExceptionService scheduleExceptionService)
    {
        _scheduleExceptionService = scheduleExceptionService;
    }
    [Authorize(Roles = "Doctor")]
    [HttpPost("create")]
    public async Task<ActionResult<ScheduleExceptionDetailsDto>> CreateScheduleException(CreateScheduleExceptionDto scheduleExceptionData)
    {
        var userId = GetUserId();
        var result = await _scheduleExceptionService.CreateScheduleException(scheduleExceptionData, userId);
        
        return Ok(result);
    }
    [Authorize(Roles = "Doctor, Admin")]
    [HttpGet]
    public async Task<ActionResult<ScheduleExceptionDetailsDto>> GetPagedScheduleExceptions([FromQuery]ScheduleExceptionFilterDto filter)
    {
        var userId = GetUserId();
        var result = await _scheduleExceptionService.GetPagedScheduleExceptions(filter, userId);
        
        return Ok(result);
    }
    [Authorize(Roles = "Doctor, Admin")]
    [HttpGet("{scheduleExceptionId}")]
    public async Task<ActionResult<ScheduleExceptionDetailsDto>> GetScheduleExceptionById(int scheduleExceptionId)
    {
        var userId = GetUserId();
        var result = await _scheduleExceptionService.GetScheduleExceptionById(scheduleExceptionId, userId);
        
        return Ok(result);
    }
    [Authorize(Roles = "Doctor")]
    [HttpPut("{scheduleExceptionId}")]
    public async Task<ActionResult<ScheduleExceptionDetailsDto>> UpdateScheduleException(UpdateScheduleExceptionDto scheduleExceptionData, int scheduleExceptionId)
    {
        var userId = GetUserId();
        var result = await _scheduleExceptionService.UpdateScheduleException(scheduleExceptionData, scheduleExceptionId, userId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Doctor, Admin")]
    [HttpPatch("{scheduleExceptionId}")]
    public async Task<ActionResult<ScheduleExceptionDetailsDto>> ResolveScheduleException([FromQuery] ScheduleExceptionStatus status, int scheduleExceptionId)
    {
        var userId = GetUserId();
        var result = await _scheduleExceptionService.UpdateScheduleExceptionStatus(status, scheduleExceptionId, userId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Doctor")]
    [HttpDelete("{scheduleExceptionId}")]
    public async Task<IActionResult> DeleteScheduleException(int scheduleExceptionId)
    {
        var userId = GetUserId();
        await _scheduleExceptionService.DeleteScheduleException(scheduleExceptionId, userId);
        
        return Ok(new { message = $"The schedule exception with ID {scheduleExceptionId} has been deleted" });
    }
} 