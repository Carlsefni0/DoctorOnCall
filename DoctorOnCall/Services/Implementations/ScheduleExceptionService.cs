using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Vacation;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Services.Interfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Services;

public class ScheduleExceptionService: IScheduleExceptionService
{
  
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleExceptionService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ScheduleExceptionSummaryDto> CreateScheduleException(CreateScheduleExceptionDto scheduleExceptionData, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
        
        var scheduleException = new ScheduleException()
        {
            StartDateTime = scheduleExceptionData.StartDate,
            EndDateTime = scheduleExceptionData.EndDate,
            ExceptionType = scheduleExceptionData.ExceptionType,
            Reason = scheduleExceptionData.Reason,
            DoctorId = doctor.Id
        };
        
        var createdScheduleException = await _unitOfWork.ScheduleExceptions.CreateScheduleException(scheduleException);
        
        await _unitOfWork.CommitAsync();
        
        var mappedScheduleException = _mapper.Map<ScheduleExceptionSummaryDto>(createdScheduleException);
        
        return mappedScheduleException;
    }

    public async Task<ScheduleExceptionSummaryDto> UpdateScheduleException(UpdateScheduleExceptionDto scheduleExceptionData, int scheduleExceptionId, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var scheduleException = await _unitOfWork.ScheduleExceptions.GetScheduleExceptionById(scheduleExceptionId);
        
        var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
        
        if (scheduleException.DoctorId != doctor.Id) throw new ForbiddenAccessException("This schedules exceptions is not yours");
        
        if (scheduleException.ExceptionStatus != ScheduleExceptionStatus.Pending) throw new ValidationException("The request was processed. You can't edit it anymore.");
        
        if (scheduleExceptionData.ExceptionType.HasValue) scheduleException.ExceptionType = scheduleExceptionData.ExceptionType.Value;
        if (scheduleExceptionData.StartDate.HasValue) scheduleException.StartDateTime = scheduleExceptionData.StartDate.Value;
        if (scheduleExceptionData.EndDate.HasValue) scheduleException.EndDateTime = scheduleExceptionData.EndDate.Value;
        if (scheduleExceptionData.Reason != null) scheduleException.Reason = scheduleExceptionData.Reason;
        
        await _unitOfWork.CommitAsync();
        
        var mappedScheduleException = _mapper.Map<ScheduleExceptionSummaryDto>(scheduleException);
        
        return mappedScheduleException;
        
    }
    
    public async Task<ScheduleExceptionDetailsDto> UpdateScheduleExceptionStatus(ScheduleExceptionStatus status, int scheduleExceptionId, int userId)
    {
        await _unitOfWork.BeginTransactionAsync(); 
            
        var user  = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"User with ID {userId} not found");
        
        var scheduleException = await _unitOfWork.ScheduleExceptions.GetScheduleExceptionById(scheduleExceptionId);
        
        var isDoctor = await _unitOfWork.UserManager.IsInRoleAsync(user, "Doctor");

        if (isDoctor)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
            
            if(scheduleException.DoctorId != doctor.Id) throw new ForbiddenAccessException("This schedule exception is not yours");
        }
        
        scheduleException.ExceptionStatus = status;

        await _unitOfWork.CommitAsync();
        
        var mappedScheduleException = _mapper.Map<ScheduleExceptionDetailsDto>(scheduleException);
        
        return mappedScheduleException;
    }

    public async Task<PagedResult<ScheduleExceptionSummaryDto>> GetPagedScheduleExceptions(ScheduleExceptionFilterDto filter, int userId)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"User with ID {userId} not found");
        
        var isDoctor = await _unitOfWork.UserManager.IsInRoleAsync(user, "Doctor");
        
        if (isDoctor)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
            filter.DoctorId = doctor.Id;
        }
        
        var pagedScheduleExceptions = await _unitOfWork.ScheduleExceptions.GetPagedScheduleExceptions(filter);
      
        return pagedScheduleExceptions;
    }
    public async Task<ScheduleExceptionDetailsDto> GetScheduleExceptionById(int scheduleExceptionId, int userId)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"User with ID {userId} not found");
        
        var scheduleExceptions = await _unitOfWork.ScheduleExceptions.GetScheduleExceptionById(scheduleExceptionId);
        
        var isDoctor = await _unitOfWork.UserManager.IsInRoleAsync(user, "Doctor");

        if (isDoctor)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
            
            if(scheduleExceptions.DoctorId != doctor.Id) throw new ForbiddenAccessException("This schedule exception is not yours ");
        }
        
        var mappedScheduleExceptions = _mapper.Map<ScheduleExceptionDetailsDto>(scheduleExceptions);
        
        return mappedScheduleExceptions;
    }
    
    public async Task DeleteScheduleException(int scheduleExceptionId, int userId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
        
        var scheduleException = await _unitOfWork.ScheduleExceptions.GetScheduleExceptionById(scheduleExceptionId);
        
        if(scheduleException.DoctorId != doctor.Id) throw new ForbiddenAccessException("This schedule exception is not yours ");
        
        await _unitOfWork.ScheduleExceptions.DeleteScheduleException(scheduleExceptionId);
        
        await _unitOfWork.CommitAsync();
    }
    
}