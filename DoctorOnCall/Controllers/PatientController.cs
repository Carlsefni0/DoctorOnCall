using DoctorOnCall.Models;
using DoctorOnCall.ServiceInterfaces;
using DoctorOnCall.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

public class PatientController: Controller
{
    private readonly IVisitRequestsService _visitRequestsService;

    public PatientController(IVisitRequestsService visitRequestsService)
    {
        _visitRequestsService = visitRequestsService;
    }

    [HttpPost("/create-visit-request")]
    public async Task<IActionResult> CreateVisitRequest(HttpRequest request)
    {
        VisitRequestModel visitRequest = await request.ReadFromJsonAsync<VisitRequestModel>();
        _visitRequestsService.CreateVisitRequest(visitRequest);
        
        return Ok("Visit request created");
    }

}