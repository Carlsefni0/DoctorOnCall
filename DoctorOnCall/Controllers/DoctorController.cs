using DoctorOnCall.DTOs;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;


[ApiController]
[Route("api/doctor")]
public class DoctorController: ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<DoctorDetailsDto>> CreateDoctor([FromBody] CreateDoctorDto doctor)
    {
        
        var result = await _doctorService.CreateDoctor(doctor);
        
        return Ok(result);
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<DoctorDetailsDto>> GetAllDoctors([FromQuery] DoctorFilterDto filter)
    {
        var result = await _doctorService.GetDoctors(filter);
        
        return Ok(result);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<DoctorDetailsDto>> GetDoctor(int userId)
    {
        var result = await _doctorService.GetDoctorByUserId(userId);
        
        return Ok(result);
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<DoctorDetailsDto>> EditDoctor(int userId, [FromBody] CreateDoctorDto doctorData)
    {
        var result = await _doctorService.UpdateDoctor(userId, doctorData);
        
        return Ok(result);
    }
}