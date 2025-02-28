using DoctorOnCall.DTOs;
using DoctorOnCall.Repository.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[ServiceFilter(typeof(ValidateModelStateFilter))]
[Authorize]
[ApiController]
[Route("api/medicine")]
public class MedicineController: BaseController
{
    private readonly IMedicineService _medicineService;

    public MedicineController(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<RequestedMedicineDto>>> GetMedicines()
    {
        var result = await _medicineService.GetMedicines();
        
        return Ok(result);
    }
    
    [HttpGet("{requestId}")]
    public async Task<ActionResult<ICollection<RequestedMedicineDto>>> GetMedicinesByVisitRequestId(int requestId)
    {
        var userId = GetUserId();
        var result = await _medicineService.GetMedicinesByVisitRequestId(requestId, userId);
        
        return Ok(result);
    }
    
    [HttpGet("find/{medicineName}")]
    public async Task<ActionResult<ICollection<RequestedMedicineDto>>> FindMedicineByName(string medicineName)
    {
        var result = await _medicineService.FindMedicinesByName(medicineName);
        
        return Ok(result);
    }
    
    
    
}