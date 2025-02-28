using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Schedule;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.ServiceInterfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Services;
//TODO:Enhance DTO
public class ScheduleService: IScheduleService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<ScheduleDetailsDto> CreateSchedule(CreateScheduleDto scheduleData)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var existingSchedule = await _unitOfWork.Schedules.GetScheduleByName(scheduleData.ScheduleName);

        if (existingSchedule != null) throw new ValidationException("Schedule with this name already exists");
        
        var schedule = new ScheduleType
        {
            Name = scheduleData.ScheduleName,
            ScheduleDayMappings = new List<ScheduleDayMapping>()
        };
        
        await AddOrUpdateScheduleDays(schedule, scheduleData.ScheduleDays);

        var createdSchedule = await _unitOfWork.Schedules.CreateSchedule(schedule);
        
        await _unitOfWork.CommitAsync();
        
        return _mapper.Map<ScheduleDetailsDto>(createdSchedule);
    }
    
    public async Task<ScheduleDetailsDto> UpdateSchedule(int scheduleId, CreateScheduleDto scheduleData)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var schedule = await _unitOfWork.Schedules.GetScheduleById(scheduleId);

        var existingSchedule = await _unitOfWork.Schedules.GetScheduleByName(scheduleData.ScheduleName);

        if (existingSchedule != null && schedule != existingSchedule) throw new ValidationException("Schedule with this name already exists");
        
        schedule.Name = scheduleData.ScheduleName;
        schedule.ScheduleDayMappings.Clear();

        await AddOrUpdateScheduleDays(schedule, scheduleData.ScheduleDays);
        
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ScheduleDetailsDto>(schedule);
    }
    
    public async Task<ScheduleDetailsDto> GetScheduleById(int scheduleId, int userId)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null ) throw new NotFoundException($"User with ID {userId} not found");
        
        var isDoctor = await _unitOfWork.UserManager.IsInRoleAsync(user, "Doctor");

        if (isDoctor)
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
            var isAssignedToDoctor = doctor.ScheduleAssignments.Any(sa => sa.ScheduleTypeId == scheduleId);

            if (!isAssignedToDoctor) throw new ForbiddenAccessException("This schedule is not yours");

        }
        var schedule = await _unitOfWork.Schedules.GetScheduleById(scheduleId);
        
        var mappedSchedule = _mapper.Map<ScheduleDetailsDto>(schedule);
        
        return mappedSchedule;
    }
    
    public async Task<PagedResult<ScheduleDetailsDto>> GetPagedSchedules(ScheduleFilterDto filter, int userId)
    {
        var user = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"The user with ID {userId} was not found");
        
        var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);

        if (userRoles.Contains("Doctor"))
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
            filter.DoctorId = doctor.Id;
        }
        
        var schedules = await _unitOfWork.Schedules.GetPagedSchedules(filter);
        
        return schedules;
    }
    
    public async Task<ICollection<ScheduleSummaryDto>> GetSchedulesNames()
    {
        var schedules = await _unitOfWork.Schedules.GetSchedulesNames();
        
        return schedules;
    }
    public async Task DeleteSchedule(int scheduleId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var schedule = await _unitOfWork.Schedules.GetScheduleById(scheduleId);
      
        var scheduleDays = schedule.ScheduleDayMappings.Select(mapping => mapping.ScheduleDay).ToList();

        schedule.ScheduleDayMappings.Clear();
        
        await _unitOfWork.Schedules.DeleteSchedule(scheduleId);

        foreach (var day in scheduleDays)
        {
            var mappingsCount = await _unitOfWork.ScheduleDays.GetMappingsCountForDay(day.Id);
            if (mappingsCount == 0)
            {
                await _unitOfWork.ScheduleDays.DeleteScheduleDay(day.Id);
            }
        }
        
        await _unitOfWork.CommitAsync();
    }
    public async Task AssignScheduleToDoctor(int scheduleId, int doctorId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var doctor = await _unitOfWork.Doctors.GetDoctorById(doctorId);
        
        var doctorSchedules = await _unitOfWork.Schedules.GetSchedulesByDoctorId(doctor.Id);
        
        var newSchedule = await _unitOfWork.Schedules.GetScheduleById(scheduleId);
        
        if (newSchedule.Id == scheduleId) throw new ValidationException("Schedule already assigned to this doctor");

        var newScheduleDays = await _unitOfWork.ScheduleDays.GetScheduleDaysByScheduleId(newSchedule.Id);

        foreach (var schedule in doctorSchedules)
        {
            var existingScheduleDays = schedule.ScheduleDayMappings
                .Select(s => s.ScheduleDay);

            if (existingScheduleDays.Intersect(newScheduleDays).Any())
            {
                throw new ScheduleConflictException("The Schedule Conflict occured. Doctor schedules have common schedule days");
            }
        }

        var assignment = new DoctorScheduleAssignment
        {
            DoctorId = doctor.Id,
            ScheduleTypeId = newSchedule.Id,
        };

        doctor.ScheduleAssignments.Add(assignment);
        
        await _unitOfWork.CommitAsync();
    }
    public async Task RemoveScheduleFromDoctor(int doctorId, int scheduleId)
    {
        await _unitOfWork.BeginTransactionAsync();
        
        var doctor = await _unitOfWork.Doctors.GetDoctorById(doctorId, "ScheduleAssignments");
      
        var scheduleAssignment = doctor.ScheduleAssignments.FirstOrDefault(sa => sa.ScheduleTypeId == scheduleId);

        if (scheduleAssignment == null) throw new NotFoundException($"The schedule with id {scheduleId} is not assigned to the doctor.");

        doctor.ScheduleAssignments.Remove(scheduleAssignment);
        
        await _unitOfWork.CommitAsync();

    }
    private async Task AddOrUpdateScheduleDays(ScheduleType schedule, IEnumerable<ScheduleDayDto> scheduleDays)
    {
        foreach (var day in scheduleDays)
        {
            var existingDay = await _unitOfWork.ScheduleDays.GetScheduleDay(day.DayOfWeek, day.StartTime, day.EndTime);
            if (existingDay == null)
            {
                existingDay = new ScheduleDay
                {
                    DayOfWeek = day.DayOfWeek,
                    StartTime = day.StartTime,
                    EndTime = day.EndTime
                };

                existingDay = await _unitOfWork.ScheduleDays.CreateScheduleDay(existingDay);
            }

            schedule.ScheduleDayMappings.Add(new ScheduleDayMapping
            {
                ScheduleDay = existingDay
            });
        }
    }
    public async Task<bool> IsWithinDoctorSchedule(int doctorId, DateTime startDate, DateTime endDate)
    {
        var scheduleMapping = await _unitOfWork.Schedules.GetDoctorWorkingDayByDate(doctorId, startDate);

        if (scheduleMapping == null)
        {
            return false;
        }

        var startTime = scheduleMapping.ScheduleDay.StartTime;
        var endTime = scheduleMapping.ScheduleDay.EndTime;

        return startDate.TimeOfDay >= startTime && endDate.TimeOfDay <= endTime;
    }


}