using AutoMapper;
using DoctorOnCall;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.Vacation;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.Utils;
using Microsoft.EntityFrameworkCore;

public class ScheduleExceptionRepository : IScheduleExceptionRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public ScheduleExceptionRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<ScheduleException> CreateScheduleException(ScheduleException exception)
    {
        var createdEntity = await _db.ScheduleExceptions.AddAsync(exception);
        
        return createdEntity.Entity;
    }
    
    // public async Task<ScheduleException> UpdateScheduleException(ScheduleException scheduleExceptionData)
    // {
    //     var updatedScheduleException = _db.ScheduleExceptions.Update(scheduleExceptionData);
    //     
    //     var changes = await _db.SaveChangesAsync();
    //
    //     if (changes == 0) throw new ApplicationException("Failed to update schedule exception.");
    //
    //     return updatedScheduleException.Entity;
    // }
    
    
    public async Task<PagedResult<ScheduleExceptionSummaryDto>> GetPagedScheduleExceptions(ScheduleExceptionFilterDto? filter = null)
    {
        filter ??= new ScheduleExceptionFilterDto();
    
        var query = _db.ScheduleExceptions
            .Include(se => se.Doctor)
            .ThenInclude(d => d.User)
            .AsQueryable();

        var filteredQuery = FilterScheduleExceptions(filter, query);
        
        var totalCount = await filteredQuery.CountAsync(); 
        
        var scheduleExceptions = await filteredQuery
            .OrderBy(se => se.StartDateTime)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        

        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var mappedScheduleExceptions = _mapper.Map<ICollection<ScheduleExceptionSummaryDto>>(scheduleExceptions);

        var pagedScheduleExceptions = new PagedResult<ScheduleExceptionSummaryDto>
        {
            CurrentPage = filter.PageNumber,
            TotalPages = totalPages,
            TotalCount = totalCount,
            Items = mappedScheduleExceptions
        };

        return pagedScheduleExceptions;
    }
    
    public async Task<ScheduleException> GetScheduleExceptionById(int scheduleExceptionId)
    {
        var scheduleException = await _db.ScheduleExceptions
            .Include(se => se.Doctor)
            .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(e => e.Id == scheduleExceptionId);
        
        if (scheduleException == null) throw new NotFoundException($"ScheduleException with ID {scheduleExceptionId} not found.");

        return scheduleException;
    }
    
    public async Task DeleteScheduleException(int scheduleExceptionId)
    {
        var scheduleException = await _db.ScheduleExceptions.Where(se => se.Id == scheduleExceptionId).FirstOrDefaultAsync();
        
        if (scheduleException == null) throw new NotFoundException($"ScheduleException with ID {scheduleExceptionId} not found.");

        _db.ScheduleExceptions.Remove(scheduleException);
    }
    
    public async Task<bool> DoctorHasScheduleException(int doctorId, DateTime date)
    {
        return await _db.ScheduleExceptions.AnyAsync(se => se.DoctorId == doctorId && 
            date >= se.StartDateTime && date <= se.EndDateTime && se.ExceptionStatus == ScheduleExceptionStatus.Approved);
    }

    private IQueryable<ScheduleException> FilterScheduleExceptions(ScheduleExceptionFilterDto filter, IQueryable<ScheduleException> query)
    {
        if (filter.DoctorId.HasValue)
        {
             query = query.Where(se => se.DoctorId == filter.DoctorId);
        }
        if (filter.StartDateTime.HasValue)
        {
            query = query.Where(se => se.StartDateTime >= filter.StartDateTime.Value);
        }
        if (filter.EndDateTime.HasValue)
        {
            query = query.Where(se => se.EndDateTime <= filter.EndDateTime.Value);
        }
        if (filter.Types != null && filter.Types.Any())
        {
            query = query.Where(se => filter.Types.Contains(se.ExceptionType));
        }
        if (filter.Statuses != null && filter.Statuses.Any())
        {
            query = query.Where(se => filter.Statuses.Contains(se.ExceptionStatus));
        }
        if (!string.IsNullOrEmpty(filter.FirstName))
        {
            query = query.Where(se => se.Doctor.User.FirstName.Contains(filter.FirstName));
        }

        if (!string.IsNullOrEmpty(filter.LastName))
        {
            query = query.Where(se => se.Doctor.User.LastName.Contains(filter.LastName));
        }
        
        return query;
    }
}