using DoctorOnCall.DTOs.Schedule;
using DoctorOnCall.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[ApiController]
[Route("api/schedule")]
public class ScheduleController: BaseController
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<ActionResult<ScheduleDetailsDto>> CreateSchedule([FromBody] CreateScheduleDto createScheduleDto)
    {
        var result = await _scheduleService.CreateSchedule(createScheduleDto);
        
        return Ok(result);
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("{scheduleId}")]
    public async Task<ActionResult<ScheduleDetailsDto>> UpdateSchedule(int scheduleId, [FromBody] CreateScheduleDto updateScheduleDto)
    {
        var result = await _scheduleService.UpdateSchedule(scheduleId, updateScheduleDto);
        
        return Ok(result);
    }
    [Authorize(Roles = "Admin, Doctor")]
    [HttpGet("{scheduleId}")]
    public async Task<ActionResult<ScheduleDetailsDto>> GetScheduleById(int scheduleId)
    {
        var userId = GetUserId();
        var result = await _scheduleService.GetScheduleById(scheduleId, userId);
        
        return Ok(result);
    }
    [Authorize(Roles = "Admin, Doctor")]
    [HttpGet]
    public async Task<ActionResult<ScheduleDetailsDto>> GetPagedSchedules([FromQuery]ScheduleFilterDto filter)
    {
        var userId = GetUserId();
        
        var result = await _scheduleService.GetPagedSchedules(filter, userId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("names")]
    public async Task<ActionResult<ScheduleSummaryDto>> GetScheduleNames()
    {
        var result = await _scheduleService.GetSchedulesNames();
        
        return Ok(result);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{scheduleId}")]
    public async Task<IActionResult> DeleteSchedule(int scheduleId)
    {
        await _scheduleService.DeleteSchedule(scheduleId);
        
        return Ok(new {message = "Schedule deleted successfully"});
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("assign/{scheduleId}/doctor/{doctorId}")]
    public async Task<IActionResult> AssignSchedule(int doctorId, int scheduleId)
    {
        await _scheduleService.AssignScheduleToDoctor(scheduleId, doctorId);

        return Ok(new {message = "Schedule assigned to the doctor successfully"});
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("remove/{scheduleId}/doctor/{doctorId}")]
    public async Task<IActionResult> RemoveSchedule(int doctorId, int scheduleId)
    {
        await _scheduleService.RemoveScheduleFromDoctor(doctorId, scheduleId);
        
        return Ok(new {message = "Schedule removed from the doctor successfully"});
    }
    
    
}