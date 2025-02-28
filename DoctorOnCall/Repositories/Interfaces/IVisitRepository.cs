using DoctorOnCall.DTOs.Analytics;
using DoctorOnCall.DTOs.Visit;
using DoctorOnCall.Models;

namespace DoctorOnCall.Repositories.Interfaces;

public interface IVisitRepository
{
    Task<Visit> CreateVisit(Visit visitData);
    Task<Visit> UpdateVisit(Visit visitData);
    Task<Visit> GetVisitById(int visitId);
    Task<ICollection<Visit>> GetVisitsByRequestId(int visitRequestId);
    Task<ICollection<Visit>> GetVisits(VisitFilterDto? filter);
    Task<ICollection<MedicineExpenseStatsDto>> GetMedicineExpensesByMonth(VisitFilterDto? filter);
}