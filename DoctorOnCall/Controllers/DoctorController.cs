using Azure;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.ServiceInterfaces;
using DoctorOnCall.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace DoctorOnCall.Controllers;


//TODO: Add Soft Delete

[ApiController]
[Route("api/doctor")]
public class DoctorController: ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IUserService _userService;


    public DoctorController(IDoctorService doctorService, IUserService userService)
    {
        _doctorService = doctorService;
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<DoctorDetailsDto>> CreateDoctor([FromBody] CreateDoctorDto doctor)
    {
        var result = await _doctorService.CreateDoctor(doctor);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{doctorId}")]
    public async Task<ActionResult<DoctorDetailsDto>> EditDoctor(int doctorId, [FromBody] EditDoctorDto doctorData)
    {
        var result = await _doctorService.UpdateDoctor(doctorId, doctorData);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("{doctorId}")]
    public async Task<ActionResult<DoctorDetailsDto>> GetDoctorById(int doctorId)
    {
        var result = await _doctorService.GetDoctorById(doctorId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<PagedResult<DoctorSummaryDto>>> GetAllDoctors( [FromQuery] DoctorFilterDto filter)
    {
        var result = await _doctorService.GetPagedDoctors(filter);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("find/{visitRequestId}")]
    public async Task<ActionResult<DoctorDetailsDto>> GetDoctorsForVisitRequest(int visitRequestId)
    {
        var result = await _doctorService.GetDoctorsForVisitRequest(visitRequestId,"driving");
        
        return Ok(result);
    }

    [Authorize(Roles = "Admin, Patient, Doctor")]
    [HttpGet("assigned/{visitRequestId}")]
    public async Task<ActionResult<DoctorSummaryDto>> GetDoctorAssignedToVisitRequest(int visitRequestId)
    {
        var result = await _doctorService.GetDoctorAssignedToVisitRequest(visitRequestId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteDoctor(int userId)
    {
        await _userService.DeleteUser(userId);
        
        return Ok(new { message = "Doctor deleted successfully" });
    }
    
}