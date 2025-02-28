using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Services.Interfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Services;

public class PatientService : IPatientService
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IUnitOfWork _unitOfWork;


    public PatientService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IGoogleMapsService googleMapsService)
    {
        _userService = userService;
        _mapper = mapper;
        _googleMapsService = googleMapsService;
        _unitOfWork = unitOfWork;
    }


    public async Task<PatientDetailsDto> CreatePatient(CreatePatientDto patientData)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var location = await _googleMapsService.GetCoordinates(patientData.Address);
        
        var createdUser = await _userService.CreateUser(patientData, UserRole.Patient);

        var patient = new Patient()
        {
            UserId = createdUser.Id,
            Address = patientData.Address,
            Disease = patientData.Disease,
            DateOfBirth = patientData.DateOfBirth,
            District = patientData.District,
            Location = location,
        };
        
        var createdPatient = await _unitOfWork.Patients.CreatePatient(patient);
        
        await _unitOfWork.CommitAsync();
        
        var mappedPatient = _mapper.Map<PatientDetailsDto>(createdPatient);
        
        return mappedPatient;
    }

    public async Task<PatientDetailsDto> UpdatePatient(int patientId, EditPatientDto patientData)
    {
        await _unitOfWork.BeginTransactionAsync();

        var patient = await _unitOfWork.Patients.GetPatientById(patientId);
        
        if (!string.IsNullOrEmpty(patientData.FirstName)) patient.User.FirstName = patientData.FirstName;
        if (!string.IsNullOrEmpty(patientData.LastName)) patient.User.LastName = patientData.LastName;
        if (!string.IsNullOrEmpty(patientData.District)) patient.District = patientData.District;
        if (!string.IsNullOrEmpty(patientData.Disease)) patient.Disease = patientData.Disease;
        if (patientData.DateOfBirth.HasValue) patient.DateOfBirth = patientData.DateOfBirth.Value;
        
        await _unitOfWork.CommitAsync();
        
        var mappedPatient = _mapper.Map<PatientDetailsDto>(patient);
        
        return mappedPatient;
    }
    public async Task<PagedResult<PatientSummaryDto>> GetPagedPatients(PatientFilterDto filter)
    {
        var patients = await _unitOfWork.Patients.GetPagedPatients(filter);
        
        return patients;
    }

    public async Task<PatientDetailsDto> GetPatientById(int patientId)
    {
        var patient = await _unitOfWork.Patients.GetPatientById(patientId);
        
        var mappedPatient = _mapper.Map<PatientDetailsDto>(patient);
        
        return mappedPatient; 
    }
}