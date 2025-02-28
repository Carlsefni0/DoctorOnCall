using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Utils;

public interface IUnitOfWork : IDisposable
{
    UserManager<AppUser> UserManager { get; }
    IDoctorRepository Doctors { get; }
    IPatientRepository Patients { get; }
    IVisitRequestRepository VisitRequests { get; }
    IVisitRepository Visits { get; }
    IScheduleRepository Schedules { get; }
    IScheduleDayRepository ScheduleDays { get; }
    IScheduleExceptionRepository ScheduleExceptions { get; }
    IMedicineRepository Medicines { get; }
    
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
