using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

public class ScheduleDayRepository : IScheduleDayRepository
{
    private readonly DataContext _db;

    public ScheduleDayRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<ScheduleDay?> GetScheduleDay(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        var scheduleDay = await _db.ScheduleDays.FirstOrDefaultAsync(d => d.DayOfWeek == dayOfWeek && 
                                                            d.StartTime == startTime && d.EndTime == endTime);
        
        return scheduleDay;
    }

    public async Task<ScheduleDay> CreateScheduleDay(ScheduleDay scheduleDay)
    {
        var createdScheduleDay = await _db.ScheduleDays.AddAsync(scheduleDay);
        
        return createdScheduleDay.Entity;
    }
    public async Task DeleteScheduleDay(int dayId)
    {
        var scheduleDay = await _db.ScheduleDays.FirstOrDefaultAsync(sd => sd.Id == dayId);
        
        if (scheduleDay == null) throw new NotFoundException("Day not found");
        
        _db.ScheduleDays.Remove(scheduleDay);
    }
    public async Task<ICollection<ScheduleDay>> GetScheduleDaysByScheduleId(int scheduleTypeId)
    {
        var days = await _db.ScheduleDayMappings
            .Where(s => s.ScheduleTypeId == scheduleTypeId)
            .Select(s => s.ScheduleDay)
            .ToListAsync();
        
        return days;
    }
    public async Task<int> GetMappingsCountForDay(int dayId)
    {
        return await _db.ScheduleDayMappings
            .CountAsync(mapping => mapping.ScheduleDayId == dayId);
    }
}