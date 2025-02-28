using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Route;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.ServiceInterfaces;
using DoctorOnCall.Services.Interfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Services.Implementations;

public class DoctorService : IDoctorService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IScheduleService _scheduleService;
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IMapper mapper, IUserService userService, IGoogleMapsService googleMapsService, IScheduleService scheduleService, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _userService = userService;
        _googleMapsService = googleMapsService;
        _scheduleService = scheduleService;
        _unitOfWork = unitOfWork;
    }
    public async Task<DoctorDetailsDto> CreateDoctor(CreateDoctorDto doctorData)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var createdUser = await _userService.CreateUser(doctorData, UserRole.Doctor);

        var location = await _googleMapsService.GetCoordinates(doctorData.Hospital);
        
        var doctor = new Doctor()
        {
            UserId = createdUser.Id,
            Specialization = doctorData.Specialization,
            WorkingDistrict = doctorData.WorkingDistrict,
            Hospital = doctorData.Hospital,
            Location = location,
        };
        
        var createdDoctor = await _unitOfWork.Doctors.CreateDoctor(doctor);
        
        await _unitOfWork.CommitAsync();
        
        var mappedDoctor = _mapper.Map<DoctorDetailsDto>(createdDoctor);
        
        return mappedDoctor;
    }
    
    public async Task<DoctorDetailsDto> UpdateDoctor(int doctorId, EditDoctorDto doctorData)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var doctor = await _unitOfWork.Doctors.GetDoctorById(doctorId, "User");

        if (!string.IsNullOrEmpty(doctorData.FirstName)) doctor.User.FirstName = doctorData.FirstName;
        if (!string.IsNullOrEmpty(doctorData.LastName)) doctor.User.LastName = doctorData.LastName;
        if (!string.IsNullOrEmpty(doctorData.Specialization)) doctor.Specialization = doctorData.Specialization;
        if (!string.IsNullOrEmpty(doctorData.WorkingDistrict)) doctor.WorkingDistrict = doctorData.WorkingDistrict;
        if (!string.IsNullOrEmpty(doctorData.Hospital))
        {
            var location = await _googleMapsService.GetCoordinates(doctorData.Hospital);
            doctor.Hospital = doctorData.Hospital;
            doctor.Location = location;
        }
        
        await _unitOfWork.CommitAsync();
        
        var mappedDoctor = _mapper.Map<DoctorDetailsDto>(doctor);
        
        return mappedDoctor;
    }
    
    public async Task<DoctorDetailsDto> GetDoctorById(int doctorId)
    {
        var doctor = await _unitOfWork.Doctors.GetDoctorById(doctorId, "User");
        
        var mappedDoctor = _mapper.Map<DoctorDetailsDto>(doctor);
        
        return mappedDoctor;
    }

    public async Task<PagedResult<DoctorSummaryDto>> GetPagedDoctors(DoctorFilterDto filter)
    {
        var doctors = await _unitOfWork.Doctors.GetPagedDoctors(filter);
        
        return doctors;
    }

    public async Task<DoctorSummaryDto> GetDoctorAssignedToVisitRequest(int visitRequestId)
    {
        var doctor = await _unitOfWork.Doctors.GetDoctorAssignedToVisitRequest(visitRequestId);
        
        return doctor;
    }
    
    public async Task<ICollection<DoctorSummaryDto>> GetDoctorsForVisitRequest(int visitRequestId, string mode)
    {
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);

        var doctors = await _unitOfWork.Doctors.GetAllDoctors(new DoctorFilterDto { Districts = new List<string> { visitRequest.District } });
        
        var availableDoctors = new List<Doctor>();
        
        foreach (var doctor in doctors)
        {
            if (!await _unitOfWork.ScheduleExceptions.DoctorHasScheduleException(doctor.Id, visitRequest.RequestedDateTime) && 
                !HasDoctorDeclinedRegularVisitRequest(visitRequest, doctor))
            {
                availableDoctors.Add(doctor);
            }
        }

        List<Doctor> suitableDoctors = new List<Doctor>();

        foreach (var doctor in availableDoctors)
        {
            var nextWorkingDay = await _unitOfWork.Schedules.GetDoctorWorkingDayByDate(doctor.Id, visitRequest.RequestedDateTime);
            if (nextWorkingDay == null) continue;

            DateTime startPointDateTime;
            RouteInfoDto routeInfo;

            var filter = new VisitRequestFilterDto
            {
                DoctorId = doctor.Id,
                Date = visitRequest.RequestedDateTime
            };
            var doctorVisits = await _unitOfWork.VisitRequests.GetVisitRequests(filter);

            if (!doctorVisits.Any())
            {
                startPointDateTime = visitRequest.RequestedDateTime.Date.Add(nextWorkingDay.ScheduleDay.StartTime);
                routeInfo = await _googleMapsService.GetRouteInfo(doctor.Location, visitRequest.Location, mode);
            }
            else
            {
                var lastVisit = doctorVisits.OrderByDescending(v => v.ExpectedEndDateTime).First();
                startPointDateTime = lastVisit.ExpectedEndDateTime;
                routeInfo = await _googleMapsService.GetRouteInfo(lastVisit.Location, visitRequest.Location, mode);
            }

            var expectedStartDateTime = startPointDateTime.AddSeconds(routeInfo.Duration.value);
            var expectedEndDateTime = visitRequest.ExpectedEndDateTime;

            if (expectedStartDateTime <= visitRequest.RequestedDateTime &&
                await IsDoctorAvailable(doctor.Id, expectedStartDateTime, expectedEndDateTime))
            {
                suitableDoctors.Add(doctor);
            }
        }

        var mappedDoctors = _mapper.Map<ICollection<DoctorSummaryDto>>(suitableDoctors);
        return mappedDoctors;
    }
    
    private Task<bool> IsDoctorAvailable(int doctorId, DateTime startDateTime, DateTime endDateTime)
    {
        return _scheduleService.IsWithinDoctorSchedule(doctorId, startDateTime, endDateTime);
    }
    private bool HasDoctorDeclinedRegularVisitRequest(VisitRequest visitRequest, Doctor doctor)
    {
        var assignation =  visitRequest.DoctorVisitRequests
            .FirstOrDefault(doctorVisitRequest => doctorVisitRequest.DoctorId == doctor.Id);

        if (assignation == null || assignation.IsApprovedByDoctor == null) return false;

        return assignation.IsApprovedByDoctor.Value ;
    }
}