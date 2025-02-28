using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.Extensions;
using DoctorOnCall.Models;
using DoctorOnCall.Utils;
using Microsoft.EntityFrameworkCore;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Repositories.Implementations;

public class PatientRepository: IPatientRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public PatientRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<Patient> CreatePatient(Patient patientData)
    {
        var createdPatient = await _db.Patients.AddAsync(patientData);
       
        return createdPatient.Entity;
    }

    public async Task<Patient> GetPatientById(int patientId)
    {
        var patient = await _db.Patients.Include(p=>p.User).FirstOrDefaultAsync(p => p.Id == patientId);
         
        if (patient == null) throw new NotFoundException("Patient not found");
         
        return patient;
    }
    
    public async Task<PagedResult<PatientSummaryDto>> GetPagedPatients(PatientFilterDto? filter)
    {
        filter ??= new PatientFilterDto();
        
        var query = _db.Patients.AsQueryable();

        var filteredQuery = FilterQuery(filter,query);
        
        var totalCount = await filteredQuery.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        var patients = await filteredQuery
            .Include("User")
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var mappedPatients = _mapper.Map<ICollection<PatientSummaryDto>>(patients);

        return new PagedResult<PatientSummaryDto>()
        {
            CurrentPage = filter.PageNumber,
            TotalPages = totalPages,
            TotalCount = totalCount,
            Items = mappedPatients
        };
    }

    public async Task<Patient> GetPatientByUserId(int userId)
    {
         var patient = await _db.Patients
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.UserId == userId);
         
         if (patient == null) throw new NotFoundException($"Patient with user ID {userId} not found");
         
         return patient;
    }
    
    private IQueryable<Patient> FilterQuery(PatientFilterDto filter, IQueryable<Patient> query)
    {

        if (!string.IsNullOrEmpty(filter.FirstName))
        {
            query = query.Where(p => p.User.FirstName.Contains(filter.FirstName));
        }

        if (!string.IsNullOrEmpty(filter.LastName))
        {
            query = query.Where(p => p.User.LastName.Contains(filter.LastName));
        }

        if (filter.Districts != null && filter.Districts.Any())
        {
            query = query.Where(p => filter.Districts.Contains(p.District));
        }
        
        if (filter.BirthYear.HasValue)
        {
            query = query.Where(p => p.DateOfBirth.Year == filter.BirthYear.Value);
        }

        if (filter.BirthMonth.HasValue)
        {
            query = query.Where(p => p.DateOfBirth.Month == filter.BirthMonth.Value);
        }

        if (filter.Gender.HasValue)
        {
            query = query.Where(p => p.User.Gender == filter.Gender.Value);
        }
        
        return query;
    }
}