using DoctorOnCall.DTOs;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

//TODO: Add Soft Delete

[ServiceFilter(typeof(ValidateModelStateFilter))]
[ApiController]
[Route("api/patient")]
public class PatientController: BaseController
{
    private readonly IPatientService _patientService;
    private readonly IUserService _userService;

    public PatientController(IPatientService patientService, IUserService userService)
    {
        _patientService = patientService;
        _userService = userService;
    }   

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<PatientDetailsDto>> CreatePatient(CreatePatientDto patientData)
    {
        var result = await _patientService.CreatePatient(patientData);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{patientId}")]
    public async Task<ActionResult<PatientDetailsDto>> EditPatient(int patientId, [FromBody] EditPatientDto patientData)
    {
        var result = await _patientService.UpdatePatient(patientId, patientData);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin, Doctor")]
    [HttpGet("{patientId}")]
    public async Task<ActionResult<PatientDetailsDto>> GetPatient(int patientId)
    {
        var result = await _patientService.GetPatientById(patientId);
        
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<PatientSummaryDto>> GetAllPatients([FromQuery] PatientFilterDto filter)
    {
        var result = await _patientService.GetPagedPatients(filter);
        
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeletePatient(int userId)
    {
        await _userService.DeleteUser(userId);
        
        return Ok(new { message = "Patient deleted successfully" });
    }
}