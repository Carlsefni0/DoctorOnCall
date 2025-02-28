using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.Extensions;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories.Implementations;

public class DoctorRepository : IDoctorRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public DoctorRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    
    public async Task<Doctor> CreateDoctor(Doctor doctor)
    {
        var createdDoctor = await _db.Doctors.AddAsync(doctor);
        
        return createdDoctor.Entity;
    }
    
    public async Task<Doctor> GetDoctorById(int doctorId, params string[] includes)
    {
        IQueryable<Doctor> query = _db.Doctors.IncludeMultiple(includes);

        var doctor = await query.FirstOrDefaultAsync(d => d.Id == doctorId);

        if (doctor == null) throw new NotFoundException($"Doctor with ID {doctorId} not found");
        
        return doctor;
    }
    
    public async Task<Doctor> GetDoctorByUserId(int userId)
    {
        var doctor = await _db.Doctors
            .Include(d => d.User)
            .Include(d=>d.ScheduleAssignments)
            .FirstOrDefaultAsync(d => d.UserId == userId);
        
        if(doctor == null) throw new NotFoundException($"Doctor with user ID {userId} not found");

        return doctor;
    }
   
    public async Task<ICollection<Doctor>> GetAllDoctors(DoctorFilterDto? filter)
    {
        filter ??= new DoctorFilterDto();

        var query = _db.Doctors.Include(d => d.User).AsQueryable();
        
        var filteredQuery = FilterQuery(filter, query);

        var doctors = await filteredQuery.ToListAsync();
        
        return doctors;
    }

    public async Task<PagedResult<DoctorSummaryDto>> GetPagedDoctors(DoctorFilterDto? filter)
    {
        filter ??= new DoctorFilterDto();

        var query = _db.Doctors.AsQueryable();

        var filterQuery = FilterQuery(filter, query);
        
        var totalCount = await filterQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var doctors = await filterQuery
            .ProjectTo<DoctorSummaryDto>(_mapper.ConfigurationProvider)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
        
        var pagedDoctors = new PagedResult<DoctorSummaryDto>()
        {
            CurrentPage = filter.PageNumber,
            TotalPages = totalPages,
            TotalCount = totalCount,
            Items = doctors
        };

        return pagedDoctors;
    }
    
    public async Task<DoctorSummaryDto> GetDoctorAssignedToVisitRequest(int visitRequestId)
    {
        var doctor = await  _db.DoctorVisitRequests
            .Where(dvr => dvr.VisitRequestId == visitRequestId && dvr.IsApprovedByDoctor != false)
            .Include(dvr => dvr.Doctor)
            .ThenInclude(d => d.User)
            .Select(dvr => dvr.Doctor)
            .ProjectTo<DoctorSummaryDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        
        return doctor;
    }
    
    private IQueryable<Doctor> FilterQuery(DoctorFilterDto filter, IQueryable<Doctor> query)
    {
        if (!string.IsNullOrEmpty(filter.FirstName))
        {
            query = query.Where(p => p.User.FirstName.Contains(filter.FirstName));
        }

        if (!string.IsNullOrEmpty(filter.LastName))
        {
            query = query.Where(p => p.User.LastName.Contains(filter.LastName));
        }

        if (filter.Specializations != null && filter.Specializations.Any())
        {
            query = query.Where(d => filter.Specializations.Contains(d.Specialization));
        }

        if (filter.Districts != null && filter.Districts.Any())
        {
            query = query.Where(d => filter.Districts.Contains(d.WorkingDistrict));
        }

        return query;
    }
}