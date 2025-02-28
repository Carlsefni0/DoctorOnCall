using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Schedule;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

public class ScheduleRepository: IScheduleRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public ScheduleRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<ScheduleType> CreateSchedule(ScheduleType scheduleData)
    {
        var createdSchedule = await _db.ScheduleTypes.AddAsync(scheduleData);
        
        return createdSchedule.Entity;
    }


    public async Task<ScheduleType> UpdateSchedule(ScheduleType schedule)
    {
        _db.ScheduleTypes.Update(schedule);
        
        var changes = await _db.SaveChangesAsync();
        
        if (changes == 0) throw new ApplicationException("Could not create schedule");
        
        return schedule;
    }
    
    public async Task<ScheduleType> GetScheduleById(int scheduleId)
    {
        var schedule = await _db.ScheduleTypes
            .Include(s => s.ScheduleDayMappings)
            .ThenInclude(mapping => mapping.ScheduleDay)
            .Include(s => s.DoctorScheduleAssignments)
            .FirstOrDefaultAsync(s => s.Id == scheduleId);
    
        if (schedule == null) throw new NotFoundException("Schedule not found");

        schedule.ScheduleDayMappings = schedule.ScheduleDayMappings
            .OrderBy(mapping => mapping.ScheduleDay.DayOfWeek)
            .ToList();

        return schedule;
    }


    //TODO: replace this method with GetSchedules() and pass there filter params
    public async Task<ICollection<ScheduleType>> GetSchedulesByDoctorId(int doctorId)
    {
        var schedules = await _db.DoctorScheduleAssignments
            .Where(d => d.DoctorId == doctorId)
            .Include(d => d.ScheduleType)
            .ThenInclude(s => s.ScheduleDayMappings)
            .ThenInclude(s=>s.ScheduleDay)
            .Select(d => d.ScheduleType)
            .ToListAsync();

        return schedules;
    }

    public async Task<ICollection<ScheduleType>> GetSchedules()
    {
        var schedules = await _db.ScheduleTypes
            .Include(s => s.ScheduleDayMappings)
            .ThenInclude(mapping => mapping.ScheduleDay)
            .Include(s => s.DoctorScheduleAssignments).ToListAsync();
        
        if (schedules == null) throw new NotFoundException("Schedules not found");
        
        return schedules;
    }
    public async Task<PagedResult<ScheduleDetailsDto>> GetPagedSchedules(ScheduleFilterDto filter)
    {
        var query = _db.ScheduleTypes
            .Include(s => s.ScheduleDayMappings)
            .ThenInclude(mapping => mapping.ScheduleDay)
            .Include(s => s.DoctorScheduleAssignments)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.ScheduleName))
        {
            query = query.Where(s => s.Name.Contains(filter.ScheduleName));
        }

        if (filter.DoctorId.HasValue)
        {
            query = query.Where(s => s.DoctorScheduleAssignments.Any(dsa => dsa.DoctorId == filter.DoctorId.Value));
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var schedules = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        foreach (var schedule in schedules)
        {
            schedule.ScheduleDayMappings = schedule.ScheduleDayMappings
                .OrderBy(mapping => mapping.ScheduleDay.DayOfWeek) 
                .ToList();
        }

        var mappedSchedules = _mapper.Map<ICollection<ScheduleDetailsDto>>(schedules);

        return new PagedResult<ScheduleDetailsDto>()
        {
            CurrentPage = filter.PageNumber,
            TotalPages = totalPages,
            TotalCount = totalCount,
            Items = mappedSchedules
        };
    }

    public async Task<ICollection<ScheduleSummaryDto>> GetSchedulesNames()
    {
        var schedules = await _db.ScheduleTypes
            .ProjectTo<ScheduleSummaryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        
        return schedules;
    }

    public async Task<ScheduleType> GetScheduleByName(string scheduleName)
    {
        var schedule = await _db.ScheduleTypes.FirstOrDefaultAsync(s => s.Name == scheduleName);
        
        return schedule;
    } 
    
    public async Task DeleteSchedule(int scheduleId)
    {
        var schedule = await _db.ScheduleTypes.FirstOrDefaultAsync(s => s.Id == scheduleId);

        if (schedule == null) throw new NotFoundException("Schedule not found");
        
         _db.ScheduleTypes.Remove(schedule);
    }

    public async Task<ScheduleDayMapping?> GetDoctorWorkingDayByDate(int doctorId, DateTime date)
    {
        var dayOfWeek = date.DayOfWeek;

        var workingDay = await _db.DoctorScheduleAssignments
            .Where(assignment => assignment.DoctorId == doctorId)
            .Include(assignment => assignment.ScheduleType)
            .ThenInclude(schedule => schedule.ScheduleDayMappings)
            .ThenInclude(mapping => mapping.ScheduleDay)
            .SelectMany(assignment => assignment.ScheduleType.ScheduleDayMappings
                .Where(mapping => mapping.ScheduleDay.DayOfWeek == dayOfWeek))
            .FirstOrDefaultAsync();
    
        return workingDay;
    }

}