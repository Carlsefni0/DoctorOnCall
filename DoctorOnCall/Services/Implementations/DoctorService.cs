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

namespace DoctorOnCall.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IGoogleMapsService _googleMapsService;
    private readonly IVisitRequestRepository _visitRequestRepository;
    private readonly IScheduleService _scheduleService;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IScheduleExceptionRepository _scheduleExceptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DoctorService(IDoctorRepository doctorRepository, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IGoogleMapsService googleMapsService,
        IVisitRequestRepository visitRequestRepository,IScheduleService scheduleService, IScheduleRepository scheduleRepository, IScheduleExceptionRepository scheduleExceptionRepository, IUnitOfWork unitOfWork)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
        _userService = userService;
        _userManager = userManager;
        _googleMapsService = googleMapsService;
        _visitRequestRepository = visitRequestRepository;
        _scheduleService = scheduleService;
        _scheduleRepository = scheduleRepository;
        _scheduleExceptionRepository = scheduleExceptionRepository;
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
        
        var doctor = await _doctorRepository.GetDoctorById(doctorId, "User");

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
        
        var updateDoctor = await _unitOfWork.Doctors.UpdateDoctor(doctor);
        
        await _unitOfWork.CommitAsync();
        
        var mappedDoctor = _mapper.Map<DoctorDetailsDto>(updateDoctor);
        
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
        var doctors = await _doctorRepository.GetPagedDoctors(filter);
        
        return doctors;
    }

    public async Task<DoctorSummaryDto> GetDoctorAssignedToVisitRequest(int visitRequestId)
    {
        var doctor = await _doctorRepository.GetDoctorAssignedToVisitRequest(visitRequestId);
        
        var mappedDoctor = _mapper.Map<DoctorSummaryDto>(doctor);
        
        return mappedDoctor;
    }
    
    public async Task<ICollection<DoctorSummaryDto>> GetDoctorsForVisitRequest(int visitRequestId, string mode)
    {
        var visitRequest = await _visitRequestRepository.GetVisitRequestById(visitRequestId);

        var doctors = await _doctorRepository.GetAllDoctors(new DoctorFilterDto { Districts = new List<string> { visitRequest.District } });


        var availableDoctors = new List<Doctor>();
        
        foreach (var doctor in doctors)
        {
            if (!await _scheduleExceptionRepository.DoctorHasScheduleException(doctor.Id, visitRequest.RequestedDateTime) && 
                !HasDoctorDeclinedRegularVisitRequest(visitRequest, doctor))
            {
                availableDoctors.Add(doctor);
            }
        }

        List<Doctor> suitableDoctors = new List<Doctor>();

        foreach (var doctor in availableDoctors)
        {
            var nextWorkingDay = await _scheduleRepository.GetDoctorWorkingDayByDate(doctor.Id, visitRequest.RequestedDateTime);
            if (nextWorkingDay == null) continue;

            DateTime startPointDateTime;
            RouteInfoDto routeInfo;

            var filter = new VisitRequestFilterDto
            {
                DoctorId = doctor.Id,
                Date = visitRequest.RequestedDateTime
            };
            var doctorVisits = await _visitRequestRepository.GetVisitRequests(filter);

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

        if (assignation == null) return false;

        return assignation.IsApprovedByDoctor == null ? false : true;
    }

    
    
    
}