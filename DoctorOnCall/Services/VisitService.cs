using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Services;

public class VisitService : IVisitService
{
    private readonly IVisitRequestRepository _visitRequestRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly IMedicineService _medicineService;

    public VisitService(IVisitRequestRepository visitRequestRepository, UserManager<AppUser> userManager, IPatientRepository patientRepository, IMapper mapper,
        IMedicineService medicineService)
    {
        _visitRequestRepository = visitRequestRepository;
        _userManager = userManager;
        _patientRepository = patientRepository;
        _mapper = mapper;
        _medicineService = medicineService;
    }
    
    public async Task<VisitRequestDetailsDto> CreateVisitRequest(CreateVisitRequestDto createVisitRequest, int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
            throw new NotFoundException("User not found");
        
        var patient = await _patientRepository.GetPatientByUserId(userId);
        
        if (patient == null)
            throw new NotFoundException("Patient not found");

        var visitsRequests = await _visitRequestRepository.GetVisitRequestsByPatientId(patient.Id, new VisitRequestFilterDto() {});
        
        if(visitsRequests.Where(vr=>vr.Status == VisitRequestStatus.Pending).ToList().Count >= 1)
            throw new ValidationException("There is already a pending visit request");
        
        ValidateVisitRequest(createVisitRequest);

        var visitRequest = new VisitRequest()
        {
            RequestDescription = createVisitRequest.RequestDescription,
            RequestedDateTime = createVisitRequest.RequestedDateTime,
            PatientId = patient.Id,
            District = patient.District,
            VisitAddress = patient.Address,
            IsRegularVisit = createVisitRequest.IsRegularVisit,
        };
        

       var createdVisitRequest = await _visitRequestRepository.CreateVisitRequest(visitRequest);
       
       if(createVisitRequest.requestedMedicines.Count >= 1)
        await _medicineService.UpdateMedicinesForVisitRequest(createdVisitRequest.Id, createVisitRequest.requestedMedicines);

       var visitRequestDetails = await _visitRequestRepository.GetVisitRequestById(createdVisitRequest.Id);
       
       var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(visitRequestDetails);

        return  mappedVisitRequest;
    }
    
    public async Task<VisitRequestDetailsDto> EditVisitRequest(int visitRequestId, CreateVisitRequestDto editVisitRequest)
    {
        var visitRequest = await _visitRequestRepository.GetVisitRequestById(visitRequestId);

        if (visitRequest == null)
            throw new NotFoundException("Visit request not found.");

        if (visitRequest.Status != VisitRequestStatus.Pending)
            throw new ValidationException("Only pending visit requests can be edited.");

        ValidateVisitRequest(editVisitRequest);

        visitRequest.RequestedDateTime = editVisitRequest.RequestedDateTime;
        visitRequest.RequestDescription = editVisitRequest.RequestDescription;


        var updateVisitRequest = await _visitRequestRepository.UpdateVisitRequest(visitRequest);
        
        await _medicineService.UpdateMedicinesForVisitRequest(updateVisitRequest.Id, editVisitRequest.requestedMedicines);
        
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(updateVisitRequest);

        return mappedVisitRequest;
    }
    
    public async Task<VisitRequestDetailsDto> ChangeVisitRequestStatus(int visitRequestId, VisitRequestStatus status)
    {
        var visitRequest = await _visitRequestRepository.GetVisitRequestById(visitRequestId);
        
        if(visitRequest == null) throw new NotFoundException("Visit request not found.");
        
        visitRequest.Status = status;
        
        var updateVisitRequest = await _visitRequestRepository.UpdateVisitRequest(visitRequest);
        
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(updateVisitRequest);
        
        return mappedVisitRequest;
            
    }

    public async Task<ICollection<VisitRequestSummaryDto>> GetVisitRequestsByPatientId(int userId, VisitRequestFilterDto filter)
    {
        var patient = await _patientRepository.GetPatientByUserId(userId);
        
        if(patient == null) throw new NotFoundException("Patient not found");
        
        var visitRequests = await _visitRequestRepository.GetVisitRequestsByPatientId(patient.Id,filter);
        
        if(visitRequests == null) throw new NotFoundException("Visit requests for this patient were not found.");
        
        var mappedVisitRequests = _mapper.Map<List<VisitRequestSummaryDto>>(visitRequests);
        
        return mappedVisitRequests;
    }

    public async Task<VisitRequestDetailsDto> GetVisitRequestById(int visitRequestId, int userId)
    {
        
        var patient = await _patientRepository.GetPatientByUserId(userId);
        
        if(patient == null) throw new NotFoundException("Patient not found");
        
        var visitRequest = await _visitRequestRepository.GetVisitRequestById(visitRequestId);
        
        if(visitRequest == null) throw new NotFoundException("Visit request not found.");
        
        if(visitRequest.PatientId != patient.Id) throw new UnauthorizedAccessException("You are not authorized to access this visit request.");
            
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(visitRequest);
        
        return mappedVisitRequest;
    }

    private void ValidateVisitRequest(CreateVisitRequestDto visitRequestDto)
    {
        if (visitRequestDto.RequestedDateTime == default) throw new ValidationException("RequestedDateTime is required and cannot be the default date.");

        if (visitRequestDto.RequestedDateTime <= DateTime.UtcNow) throw new ValidationException("Requested visit date cannot be in the past or now.");

        if (visitRequestDto.RequestedDateTime.Subtract(DateTime.UtcNow).Days > 31) 
            throw new ValidationException("Requested visit date cannot be in more than 31 days.");

        if (!string.IsNullOrWhiteSpace(visitRequestDto.RequestDescription) && visitRequestDto.RequestDescription.Length > 1000) 
            throw new ValidationException("Request description exceeds the maximum length of 1000 characters.");
    }
}