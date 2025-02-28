using AutoMapper;
using DoctorOnCall.DTOs.Analytics;
using DoctorOnCall.DTOs.Visit;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.Utils;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

public class VisitRepository: IVisitRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public VisitRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<Visit> CreateVisit(Visit visitData)
    {
        var createdVisit = await _db.Visits.AddAsync(visitData);
        
        var changes = await _db.SaveChangesAsync();

        if (changes == 0) throw new ApplicationException("Failed to create visit.");

        return createdVisit.Entity;
    }

    public async Task<Visit> UpdateVisit(Visit visitData)
    {
        var visit = await _db.Visits.Where(v => v.Id == visitData.Id).FirstOrDefaultAsync();
        
        if (visit == null) throw new NotFoundException($"Visit with ID {visitData.Id} not found.");
        
        var updatedVisit = _db.Visits.Update(visitData);
        
        var changes = await _db.SaveChangesAsync();

        if (changes == 0) throw new ApplicationException("Failed to update visit.");

        return updatedVisit.Entity;
    }

    public async Task<Visit> GetVisitById(int visitId)
    {
        var visit = await _db.Visits.FirstOrDefaultAsync(e => e.Id == visitId);
        
        if (visit == null) throw new NotFoundException($"Visit with ID {visitId} not found.");

        return visit;
    }
    
    public async Task<ICollection<Visit>> GetVisits(VisitFilterDto? filter)
    {
        filter ??= new VisitFilterDto();
        
        var query = _db.Visits.AsQueryable();
        
        var filteredQuery = FilterVisits(filter,query);
        
        var visits = await filteredQuery.ToListAsync();
        
        return visits;
    }
    
    
    public async Task<ICollection<MedicineExpenseStatsDto>> GetMedicineExpensesByMonth(VisitFilterDto? filter)
    {
        filter ??= new VisitFilterDto();
        
        var query = _db.Visits.AsQueryable();

        var filteredQuery = FilterVisits(filter, query);
        
        var groupedExpenses = await filteredQuery
            .GroupBy(v => new { v.ActualStartDateTime.Value.Year, v.ActualStartDateTime.Value.Month })
            .Select(g => new MedicineExpenseStatsDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalExpense = g.Sum(v => v.MedicineCost)
            })
            .OrderBy(e => e.Year).ThenBy(e => e.Month)
            .ToListAsync();

        return groupedExpenses;
    }

    public async Task<ICollection<Visit>> GetVisitsByRequestId(int visitRequestId)
    {
        var visits = await _db.Visits.Where(v => v.VisitRequestId == visitRequestId).ToListAsync();
        
        return visits;
    }
    
    private IQueryable<Visit> FilterVisits(VisitFilterDto filter, IQueryable<Visit> query)
    {
        if (filter.StartDate.HasValue)
        {
            query = query.Where(v => v.ActualStartDateTime >= filter.StartDate.Value);
        }
        if (filter.EndDate.HasValue)
        {
            query = query.Where(v => v.ActualStartDateTime <= filter.EndDate.Value);
        }

        if (filter.DoctorId.HasValue)
        {
            query = query.Where(v => v.VisitRequest.DoctorVisitRequests.Any(dvr => dvr.DoctorId == filter.DoctorId.Value));
        }
        
        return query;
    }
}

