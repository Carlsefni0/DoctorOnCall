using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.VisitDTOs;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.ServiceInterfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Services;

public class VisitRequestService : IVisitRequestService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public VisitRequestService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<VisitRequestDetailsDto> CreateVisitRequest(CreateVisitRequestDto createVisitRequest, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var patient = await _unitOfWork.Patients.GetPatientByUserId(userId);

        var visitDuration = GetVisitDuration(createVisitRequest.RequestType);

        var expectedEndDatetime = createVisitRequest.RequestedDateTime.Add(visitDuration);

        var visitRequest = new VisitRequest
        {
            PatientId = patient.Id,
            RequestedDateTime = createVisitRequest.RequestedDateTime,
            ExpectedEndDateTime = expectedEndDatetime,
            RequestDescription = createVisitRequest.RequestDescription,
            District = patient.District,
            VisitAddress = patient.Address,
            Location = patient.Location,
            Type = createVisitRequest.RequestType,
            IsRegularVisit = createVisitRequest.IsRegularVisit,
            RegularVisitOccurrences = createVisitRequest.RegularVisitOccurrences,
            RegularVisitIntervalDays = createVisitRequest.RegularVisitIntervalDays,
            RequestedMedicines = createVisitRequest?.RequestedMedicines? 
                .Select(m => new VisitRequestMedicine { MedicineId = m.Key, Quantity = m.Value })
                .ToList()
        };

        if (createVisitRequest.IsRegularVisit) AssignVisitDates(visitRequest);
        
        var createdVisitRequest = await _unitOfWork.VisitRequests.CreateVisitRequest(visitRequest);
        
        await _unitOfWork.CommitAsync();
        
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(createdVisitRequest);

        return mappedVisitRequest;
    }
    public async Task<VisitRequestDetailsDto> EditVisitRequest(int visitRequestId, int userId, EditVisitRequestDto editVisitRequest)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var patient = await _unitOfWork.Patients.GetPatientByUserId(userId);
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);

        if (visitRequest.PatientId != patient.Id) 
            throw new ForbiddenAccessException("You cannot edit this visit");

        if (visitRequest.Status != VisitRequestStatus.Pending) 
            throw new ValidationException("The visit request was processed. You can't edit it anymore.");

        if (editVisitRequest.RequestedDateTime.HasValue)
        {
            visitRequest.RequestedDateTime = editVisitRequest.RequestedDateTime.Value;
            visitRequest.ExpectedEndDateTime = editVisitRequest.RequestedDateTime.Value.Add(GetVisitDuration(visitRequest.Type));
        }

        if (!string.IsNullOrEmpty(editVisitRequest.RequestDescription)) visitRequest.RequestDescription = editVisitRequest.RequestDescription;

        if (editVisitRequest.RequestedMedicines != null)
        {
            visitRequest.RequestedMedicines = editVisitRequest.RequestedMedicines
                .Select(m => new VisitRequestMedicine
                {
                    VisitRequestId = visitRequest.Id,
                    MedicineId = m.Key,
                    Quantity = m.Value
                }).ToList();
        }

        if (visitRequest.IsRegularVisit && editVisitRequest.RegularVisitIntervalDays.HasValue && editVisitRequest.RegularVisitOccurrences.HasValue)
        {
            if (visitRequest.RegularVisitDates != null && visitRequest.RegularVisitDates.Any())
            {
                await _unitOfWork.VisitRequests.RemoveRegularVisitDates(visitRequest.RegularVisitDates);
                visitRequest.RegularVisitDates.Clear();
            }

            visitRequest.RegularVisitIntervalDays = editVisitRequest.RegularVisitIntervalDays;
            visitRequest.RegularVisitOccurrences = editVisitRequest.RegularVisitOccurrences;
            
            AssignVisitDates(visitRequest);
        }
        
        await _unitOfWork.CommitAsync();
        
        return _mapper.Map<VisitRequestDetailsDto>(visitRequest);
    }



    public async Task<PagedResult<VisitRequestSummaryDto>> GetPagedVisitRequests(VisitRequestFilterDto filter, int userId)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"The user with ID {userId} was not found");
        
        var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);

        if (userRoles.Contains("Doctor"))
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
            filter.DoctorId = doctor.Id;
        }
        if (userRoles.Contains("Patient"))
        {
            var patient = await _unitOfWork.Patients.GetPatientByUserId(userId);
            filter.PatientId = patient.Id;
        }
        var pagedVisitRequests = await _unitOfWork.VisitRequests.GetPagedVisitRequestsSummary(filter);
        
        return pagedVisitRequests;
    }

    public async Task<ICollection<AssignedVisitRequestDto>> GetAssignedVisitRequests(DateTime dateRangeStart, DateTime dateRangeEnd, int userId)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"The user with ID {userId} was not found");
        
        var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
        
        var filter = new VisitRequestFilterDto
        {
            DateRangeStart = dateRangeStart,
            DateRangeEnd = dateRangeEnd,
            DoctorId = doctor.Id
        };
        
        var visitRequests = await _unitOfWork.VisitRequests.GetAssignedVisitRequests(filter);
        
        return visitRequests;
    }
    public async Task<DailyVisitsDto> GetDailyVisitRequests(DateTime date, int userId)
    {
        var doctor = await  _unitOfWork.Doctors.GetDoctorByUserId(userId);

        var filter = new VisitRequestFilterDto()
        {
            Date = date,
            DoctorId = doctor.Id
        };
        
        var visitRequests = await _unitOfWork.VisitRequests.GetCurrentVisitRequests(filter);
        
        var mappedVisitRequests = _mapper.Map<ICollection<CurrentVisitRequestDto>>(visitRequests);

        var dailyVisits = new DailyVisitsDto()
        {
            HospitalCoords = new CoordsDto() {Lat = doctor.Location.X, Lng = doctor.Location.Y},
            CurrentVisitRequests = mappedVisitRequests
        };
        
        return dailyVisits;
    }

    public async Task<VisitRequestDetailsDto> GetVisitRequestById(int visitRequestId, int userId)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"The user with ID {userId} was not found");
        
        var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);
        
        if (userRoles.Contains("Doctor"))
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
            bool isAssignedToDoctor = visitRequest.DoctorVisitRequests.Any(dvr => dvr.DoctorId == doctor.Id);

            if (!isAssignedToDoctor) throw new ForbiddenAccessException("This visit request is not assigned to you");
        }

        if (userRoles.Contains("Patient"))
        {
            var patient = await _unitOfWork.Patients.GetPatientByUserId(userId);
            bool belongToPatient = visitRequest.PatientId == patient.Id;
            
            if(!belongToPatient) throw new ForbiddenAccessException("This visit request is not yours");
        }
        
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(visitRequest);
        
        return mappedVisitRequest;
    }

    public async Task<VisitRequestDetailsDto> ApproveVisitRequest(int visitRequestId, int doctorId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);

        var doctor = await _unitOfWork.Doctors.GetDoctorById(doctorId);
        
        if(!visitRequest.IsRegularVisit) visitRequest.Status = VisitRequestStatus.Approved;

        var assignation = new DoctorVisitRequest()
        {
            DoctorId = doctor.Id,
            VisitRequestId = visitRequest.Id,
            AssignedDate = DateTime.Now,
        };

        if (!visitRequest.IsRegularVisit) assignation.IsApprovedByDoctor = true;
        
        doctor.DoctorVisitRequests.Add(assignation);
      
        await _unitOfWork.CommitAsync();
        
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(visitRequest);
        
        return mappedVisitRequest;
    }
    public async Task<VisitRequestDetailsDto> ApproveRegularVisitRequest(int visitRequestId, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);

        var assignation = doctor.DoctorVisitRequests.FirstOrDefault(v => v.VisitRequestId == visitRequestId);
    
        if (assignation == null) throw new NotFoundException("Assignation not found");

        assignation.IsApprovedByDoctor = true;
    
        visitRequest.Status = VisitRequestStatus.Approved;
    
        await _unitOfWork.CommitAsync();
    
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(visitRequest);
        
        return mappedVisitRequest;
    }

    public async Task<VisitRequestDetailsDto> RejectVisitRequest(int visitRequestId, RejectVisitRequestDto rejectVisitRequest)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);
        
        visitRequest.Status = VisitRequestStatus.Rejected;
        visitRequest.RejectionReason = rejectVisitRequest.RejectionReason;
        
        await _unitOfWork.CommitAsync();
        
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(visitRequest);
        
        return mappedVisitRequest;
    }
    public async Task RejectRegularVisitRequest(int visitRequestId, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);
        
        var assignation = visitRequest.DoctorVisitRequests.FirstOrDefault(v => v.DoctorId == doctor.Id);;
        
        if (assignation == null) throw new NotFoundException("Assignation not found");

        assignation.IsApprovedByDoctor = false;
        
        await _unitOfWork.CommitAsync();
        
    }
    public async Task<VisitRequestDetailsDto> CancelVisitRequest(int visitRequestId, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();

        var patient = await _unitOfWork.Patients.GetPatientByUserId(userId);
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);
        
        if(visitRequest.PatientId != patient.Id) throw new ForbiddenAccessException("This visit request is not yours");
        
        visitRequest.Status = VisitRequestStatus.Cancelled;
        
        var mappedVisitRequest = _mapper.Map<VisitRequestDetailsDto>(patient);
        
        await _unitOfWork.CommitAsync();
        
        return mappedVisitRequest;
    }
    
    private TimeSpan GetVisitDuration(VisitRequestType type)
    {
        return type switch
        {
            VisitRequestType.Сonsultation => TimeSpan.FromMinutes(30),
            VisitRequestType.Examination => TimeSpan.FromMinutes(60),
            VisitRequestType.Procedures => TimeSpan.FromMinutes(90),
            _ => throw new ArgumentException("Unknown visit request type", nameof(type))
        };
    }
    
    private List<DateTime> CalculateVisitDates(DateTime startDate, int? intervalDays, int? occurrences)
    {
        if (intervalDays == null || intervalDays <= 0)
            throw new ValidationException("RegularVisitIntervalDays must be a positive number.");
    
        if (occurrences == null || occurrences <= 0)
            throw new ValidationException("Occurrences must be a positive number.");

        var visitDates = new List<DateTime> { startDate };

        for (int i = 1; i < occurrences; i++)
        {
            startDate = startDate.AddDays(intervalDays.Value);
            visitDates.Add(startDate);
        }

        return visitDates;
    }

    private void AssignVisitDates(VisitRequest visitRequest)
    {
        var visitDates = CalculateVisitDates(visitRequest.RequestedDateTime, visitRequest.RegularVisitIntervalDays,
            visitRequest.RegularVisitOccurrences);
        
        foreach (var visitDate in visitDates)
        {
            var regularVisit = new RegularVisitDate
            {
                VisitRequestId = visitRequest.Id,
                VisitStartDateTime = visitDate,
                VisitEndDateTime = visitDate.Add(GetVisitDuration(visitRequest.Type)),
            };

            visitRequest.RegularVisitDates.Add(regularVisit);
        }
    } 



    
}