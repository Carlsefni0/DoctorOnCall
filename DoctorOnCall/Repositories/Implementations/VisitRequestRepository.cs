using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.Analytics;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

//TODO: Add repository methods which include certain related entities.
public class VisitRequestRepository: IVisitRequestRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public VisitRequestRepository(DataContext context, IMapper mapper)
    {
        _db = context;
        _mapper = mapper;
    }
    
    public async Task<VisitRequest> CreateVisitRequest(VisitRequest visitRequestData)
    {
        var visitRequest = await _db.VisitRequests.AddAsync(visitRequestData);
        
        return visitRequest.Entity;
    }
    public void UpdateVisitRequest(VisitRequest visitRequest)
    {
        _db.Entry(visitRequest).State = EntityState.Modified;
    }
    public async Task<VisitRequest> GetVisitRequestById(int visitRequestId)
    {
        var visitRequest = await _db.VisitRequests
            .Include(vr => vr.DoctorVisitRequests)
                .ThenInclude(dvr => dvr.Doctor)
            .Include(vr=>vr.Patient)
                .ThenInclude(p=>p.User)
            .Include(vr => vr.RegularVisitDates)
            .Include(vr=>vr.RequestedMedicines)
            .FirstOrDefaultAsync(vr => vr.Id == visitRequestId);
        
        if(visitRequest == null) throw new NotFoundException($"Visit request with ID {visitRequestId } could not be found");
        
        return visitRequest;
    }
    
    public async Task<VisitRequestDetailsDto> GetVisitRequestDetailsById(int visitRequestId)
    {
        var visitRequest = await _db.VisitRequests
            .ProjectTo<VisitRequestDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(vr => vr.Id == visitRequestId);
        
        if(visitRequest == null) throw new NotFoundException($"Visit request with ID {visitRequestId} could not be found");
        
        return visitRequest;
    }

    public async Task<ICollection<VisitRequest>> GetVisitRequests(VisitRequestFilterDto? filter)
    {
        filter ??= new VisitRequestFilterDto();
        
        var query = _db.VisitRequests.AsQueryable();

        var filteredQuery = FilterVisitRequests(filter,query);
        
        var visitRequests = await filteredQuery.ToListAsync();
        
        return visitRequests;
    }
    
    public async Task<PagedResult<VisitRequestSummaryDto>> GetPagedVisitRequestsSummary(VisitRequestFilterDto? filter = null)
    {
        filter ??= new VisitRequestFilterDto();

        var query = _db.VisitRequests.AsQueryable();
        
        var filteredQuery = FilterVisitRequests(filter,query);
        
        var totalCount = await filteredQuery.CountAsync();

        var visitRequests = await filteredQuery
            .OrderByDescending(v => v.RequestedDateTime)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<VisitRequestSummaryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        
        
        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var mappedVisitRequests = _mapper.Map<ICollection<VisitRequestSummaryDto>>(visitRequests);

        var pagedVisitRequests = new PagedResult<VisitRequestSummaryDto>
        {
            CurrentPage = filter.PageNumber,
            TotalPages = totalPages,
            TotalCount = totalCount,
            Items = mappedVisitRequests
        };

        return pagedVisitRequests;
    }
    
    public async Task<ICollection<AssignedVisitRequestDto>> GetAssignedVisitRequests(VisitRequestFilterDto filter)
    {
        var singleVisitRequests = await FilterVisitRequests(filter, _db.VisitRequests.Where(vr => !vr.IsRegularVisit))
            .ProjectTo<AssignedVisitRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var regularVisitDates = await _db.RegularVisitDates
            .Where(rvd =>
                rvd.VisitStartDateTime.Date >= filter.DateRangeStart &&
                rvd.VisitStartDateTime.Date <= filter.DateRangeEnd &&
                rvd.VisitRequest.DoctorVisitRequests.Any(d => d.DoctorId == filter.DoctorId))
            .ProjectTo<AssignedVisitRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return singleVisitRequests.Concat(regularVisitDates).ToList();
    }

    public async Task<ICollection<CurrentVisitRequestDto>> GetCurrentVisitRequests(VisitRequestFilterDto filter)
    {
        var singleVisitRequests = await FilterVisitRequests(filter, _db.VisitRequests.Where(vr => !vr.IsRegularVisit))
            .ProjectTo<CurrentVisitRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        var regularVisitDates = await _db.RegularVisitDates
            .Where(rvd =>
                rvd.VisitStartDateTime.Date == filter.Date &&
                rvd.VisitRequest.DoctorVisitRequests.Any(d => d.DoctorId == filter.DoctorId))
            .ProjectTo<CurrentVisitRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return singleVisitRequests.Concat(regularVisitDates).ToList();
    }
  
    public async Task<bool> HasPendingVisitRequest(int patientId)
    {
        return await _db.VisitRequests
            .AnyAsync(vr => vr.PatientId == patientId && vr.Status == VisitRequestStatus.Pending && vr.RegularVisitDates == null);
    }
    public async Task RemoveRegularVisitDates(IEnumerable<RegularVisitDate> regularVisitDates)
    {
        _db.RegularVisitDates.RemoveRange(regularVisitDates);
        await _db.SaveChangesAsync();
    }

    public async Task<DoctorVisitRequest> UpdateDoctorVisitRequest(DoctorVisitRequest visitRequest)
    {
        _db.DoctorVisitRequests.Update(visitRequest);
        
        var changes = await _db.SaveChangesAsync();
    
        if (changes == 0) throw new ApplicationException("Failed to update the visit request.");

        return visitRequest;
    }

    
    private IQueryable<VisitRequest> FilterVisitRequests(VisitRequestFilterDto filter, IQueryable<VisitRequest> query)
    {

        if (filter.Year.HasValue)
        {
            query = query.Where(v => v.RequestedDateTime.Year == filter.Year.Value);
        }

        if (filter.Month.HasValue)
        {
            query = query.Where(v => v.RequestedDateTime.Month == filter.Month.Value);
        }

        if (filter.Statuses != null && filter.Statuses.Any())
        {
            query = query.Where(v => filter.Statuses.Contains(v.Status));
        }
        if (filter.Types != null && filter.Types.Any())
        {
            query = query.Where(v => filter.Types.Contains(v.Type));
        }
        if (filter.IsRegular.HasValue)
        {
            if(filter.IsRegular.Value) query = query.Where(v => v.IsRegularVisit == true);
            else if(!filter.IsRegular.Value) query = query.Where(v => v.IsRegularVisit == false);
        }

        if (filter.PatientId.HasValue)
        {
            query = query.Where(v => v.PatientId == filter.PatientId.Value);
        }

        if (filter.DoctorId.HasValue)
        {
            query = query.Where(v => v.DoctorVisitRequests.Any(dvr => dvr.DoctorId == filter.DoctorId.Value && dvr.IsApprovedByDoctor != false));
            
        }

        if (filter.DateRangeStart.HasValue && filter.DateRangeEnd.HasValue)
        {
            query = query.Where(v =>
                v.RequestedDateTime.Date >= filter.DateRangeStart.Value &&
                v.RequestedDateTime <= filter.DateRangeEnd.Value);

            if (filter.isSchedule)
            {
                query = query.Where(v => v.RequestedDateTime >= DateTime.Now);
            }
        }


        if (filter.Date.HasValue)
        {
            query = query.Where(v => v.RequestedDateTime.Date == filter.Date.Value.Date);
        }
        
        return query;
    }


}