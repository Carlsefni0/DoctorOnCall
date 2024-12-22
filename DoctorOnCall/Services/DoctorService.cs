using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.ServiceInterfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;

    public DoctorService(IDoctorRepository doctorRepository, IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
    }
    public async Task<DoctorDetailsDto> CreateDoctor(CreateDoctorDto doctorData)
    {
        var createdUser = await _userService.CreateUser(doctorData, UserRole.Doctor);
        
        var doctor = new Doctor()
        {
            UserId = createdUser.Id,
            Specialization = doctorData.Specialization,
            WorkingDistrict = doctorData.WorkingDistrict,
            Status = doctorData.Status,
            ScheduleId = doctorData.ScheduleId,
        };
        
        var createdDoctor = await _doctorRepository.CreateDoctor(doctor);
        
        if(createdDoctor != null) throw new ApplicationException("Could not create doctor");
        
        var mappedDoctor = _mapper.Map<DoctorDetailsDto>(createdDoctor);
        
        return mappedDoctor;
    }

    public async Task<ICollection<DoctorSummaryDto>> GetDoctors(DoctorFilterDto filter)
    {
        var doctors = await _doctorRepository.GetDoctors(filter);
        
        if (doctors == null) throw new NotFoundException("Could not find doctors");
        
        var mappedDoctors = _mapper.Map<ICollection<DoctorSummaryDto>>(doctors);
        
        return mappedDoctors;
    }

    public async Task<DoctorDetailsDto> GetDoctorByUserId(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException("User not found");
        
        var doctor = await _doctorRepository.GetDoctorByUserId(userId);
        
        if(doctor == null) throw new NotFoundException("Doctor not found");
        
        var mappedDoctor = _mapper.Map<DoctorDetailsDto>(doctor);
        
        return mappedDoctor;
    }

    public async Task<DoctorDetailsDto> UpdateDoctor(int userId, CreateDoctorDto doctorData)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException("User not found");
        
        var doctor = await _doctorRepository.GetDoctorByUserId(userId);
        
        if(doctor == null) throw new NotFoundException("Doctor not found");
        
        var userExists = await _userManager.FindByEmailAsync(doctorData.Email);
        
        if(userExists != null) throw new ValidationException($"User with email {doctorData.Email} already exists");
        
        doctor.User.FirstName = doctorData.FirstName;
        doctor.User.LastName = doctorData.LastName;
        doctor.User.Email = doctorData.Email;
        doctor.User.PhoneNumber = doctorData.PhoneNumber;
        doctorData.Status = doctor.Status;
        doctorData.ScheduleId = doctorData.ScheduleId;
        doctorData.WorkingDistrict = doctorData.WorkingDistrict;
        
        var updateDoctor = await _doctorRepository.UpdateDoctor(doctor);
        
        if(updateDoctor == null) throw new ApplicationException("Could not update doctor");
        
        var mappedDoctor = _mapper.Map<DoctorDetailsDto>(updateDoctor);
        
        return mappedDoctor;
    }
    // public async Task<bool> DeleteDoctor(int doctorId)
    // {
    //     if (doctorId <= 0)
    //     {
    //         throw new ArgumentException("Invalid doctor ID.", nameof(doctorId));
    //     }
    //
    //     var doctor = await userManager.FindByIdAsync(doctorId.ToString());
    //
    //     if (doctor == null)
    //     {
    //         return false; // Doctor not found
    //     }
    //
    //     var result = await userManager.DeleteAsync(doctor);
    //     return result.Succeeded;
    // }
    
}