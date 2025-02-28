using AutoMapper;
using DoctorOnCall.DTOs.Visit;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Services.Interfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Services;

public class VisitService: IVisitService
{
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public VisitService(IGoogleMapsService googleMapsService, IEmailService emailService, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _googleMapsService = googleMapsService;
        _emailService = emailService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
  
    public async Task<VisitSummaryDto> CompleteVisit(int visitRequestId, CompleteVisitDto completeVisit, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);
        
        await CheckIfAssignedDoctor(userId, visitRequest);
        
        var medicineCost = visitRequest.RequestedMedicines?
            .Sum(medicine => medicine.Medicine.UnitPrice * medicine.Quantity) ?? 0;

        var doctor = visitRequest?.DoctorVisitRequests?.FirstOrDefault(dvr => dvr.VisitRequestId == visitRequestId)?.Doctor;
        
        if(doctor == null) throw new Exception("Doctor not found");

        var routeInfo = await _googleMapsService.GetRouteInfo(doctor.Location,visitRequest.Location,"driving");

        TimeSpan delayTime;
        if (visitRequest.IsRegularVisit)
        {
            var visit = visitRequest?.RegularVisitDates?.FirstOrDefault(rvd => rvd.VisitStartDateTime.Date == completeVisit.ActualVisitStartDateTime.Date);
            
            if(visit == null) throw new NotFoundException("Visit not found");
            
            delayTime = completeVisit.ActualVisitStartDateTime.Subtract(visit.VisitStartDateTime);
        }
        else
        {
            delayTime = completeVisit.ActualVisitStartDateTime.Subtract(visitRequest.RequestedDateTime);
        }
        
        
        if (delayTime.TotalMinutes < 0) delayTime = TimeSpan.Zero;

        var visitData = new Visit
        {
            VisitRequestId = visitRequest.Id,
            ActualStartDateTime = completeVisit.ActualVisitStartDateTime,
            ActualEndDateTime = completeVisit.ActualVisitEndDateTime,
            Notes = completeVisit.Notes,
            MedicineCost = medicineCost,
            TravelDistance = routeInfo.Distance.value,
            TravelTime = routeInfo.Duration.DurationTime,
            DelayTime = delayTime,
            Status = VisitStatus.Completed,
        };
        
        var createdVisit = await _unitOfWork.Visits.CreateVisit(visitData);
        
        if (visitRequest.IsRegularVisit)
        {
            visitRequest.RegularVisitDates
                .FirstOrDefault(v => v.VisitStartDateTime.Date == completeVisit.ActualVisitStartDateTime.Date).IsReported = true;
            
            if(visitRequest.RegularVisitDates.All(v => v.IsReported)) visitRequest.Status = VisitRequestStatus.Completed;
        }
        else if(!visitRequest.IsRegularVisit)
        {
            visitRequest.Status = VisitRequestStatus.Completed;
        }
        
        visitRequest.Visits.Add(createdVisit);
        
        await _unitOfWork.CommitAsync();
        
        var mappedVisit = _mapper.Map<VisitSummaryDto>(createdVisit);
        
        return mappedVisit;

    }

    public async Task<VisitSummaryDto> CancelVisit(int visitRequestId, CancelVisitDto cancelVisit, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);
        
        await CheckIfAssignedDoctor(userId, visitRequest);
        
        var visitData = new Visit
        { 
            VisitRequestId = visitRequest.Id,
            Status = VisitStatus.Cancelled,
            CancellationReason = cancelVisit.CancellationReason,
        };
        
        var createdVisit = await _unitOfWork.Visits.CreateVisit(visitData);
        
        if (visitRequest.IsRegularVisit)
        {
            visitRequest.RegularVisitDates
                .FirstOrDefault(v => v.VisitStartDateTime.Date == cancelVisit.VisitDateTime.Date).IsReported = true;
            
            if(visitRequest.RegularVisitDates.All(v => v.IsReported)) visitRequest.Status = VisitRequestStatus.Completed;
        }
        else if(!visitRequest.IsRegularVisit)
        {
            visitRequest.Status = VisitRequestStatus.Cancelled;
        }
        
        var message = $"The doctor can't come to you today. We are sorry for the inconvenience. " +
                      $"Here is the doctor's message:<br/><strong>{cancelVisit.CancellationReason}</strong>";
        
        await NotifyPatient(visitRequest.PatientId, message, "Visit Cancellation");
        
        var mappedVisit = _mapper.Map<VisitSummaryDto>(createdVisit);
        
        return mappedVisit;
        
    }

    public async Task DelayVisit(int visitRequestId, DelayVisitDto delayVisit, int userId)
    {
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);

        await CheckIfAssignedDoctor(userId, visitRequest);
        
        var message = $"The doctor will be late for your visit. We are sorry for the inconvenience. " +
                      $"Here is the doctor's message:<br/><strong>{delayVisit.DelayReason}</strong>";
        
        await NotifyPatient(visitRequest.PatientId, message, "Visit Delay");
    }
    
    public async Task<ICollection<VisitSummaryDto>> GetVisitsByRequestId(int visitRequestId, int userId)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"User with ID {userId} not found");
        
        var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);
        
        if(userRoles.Contains("Doctor")) await CheckIfAssignedDoctor(userId, visitRequest);
        
        if (userRoles.Contains("Patient"))
        {
            var patient = await _unitOfWork.Patients.GetPatientById(visitRequest.PatientId);
            if(patient.Id != visitRequest.PatientId) throw new ForbiddenAccessException("You dont have access to this visit");
        }
        
        var visits = await _unitOfWork.Visits.GetVisitsByRequestId(visitRequestId);
        
        var mappedVisit = _mapper.Map<ICollection<VisitSummaryDto>>(visits);
        
        return mappedVisit;
    }

    private async Task CheckIfAssignedDoctor(int userId, VisitRequest visitRequest)
    {
        var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);

        var isDoctorAssigned =  visitRequest.DoctorVisitRequests.Any(dvr => dvr.DoctorId == doctor.Id);
        
        if(!isDoctorAssigned) throw new ForbiddenAccessException("This visit wasn't assigned to you. You can't delay it");
    }

    private async Task NotifyPatient(int patientId, string message, string topic)
    {
        var patient = await _unitOfWork.Patients.GetPatientById(patientId);
        
        var patientEmail = patient?.User.Email;
        
        await _emailService.SendEmailAsync(patientEmail, topic, message);

    }
}