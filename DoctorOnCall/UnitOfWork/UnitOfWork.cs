using DoctorOnCall.Exceptions;
using DoctorOnCall.Models;
using DoctorOnCall.Repositories.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _db;
    private IDbContextTransaction _transaction;
    public UserManager<AppUser> UserManager { get; }
    public IDoctorRepository Doctors { get; }
    public IPatientRepository Patients { get; }
    public IVisitRequestRepository VisitRequests { get; }
    public IVisitRepository Visits { get; }
    public IScheduleRepository Schedules { get; }
    public IScheduleDayRepository ScheduleDays { get; }
    public IScheduleExceptionRepository ScheduleExceptions { get; }
    public IMedicineRepository Medicines { get; }


    public UnitOfWork (DataContext db, IDoctorRepository doctors,  IVisitRequestRepository visitRequestRepository, IScheduleRepository scheduleRepository, IScheduleExceptionRepository scheduleExceptionRepository,
        IPatientRepository patientRepository, UserManager<AppUser> userManager, IVisitRepository visitRepository, IScheduleDayRepository scheduleDays, IMedicineRepository medicineRepository)
    {
        _db = db;
        Doctors = doctors;
        VisitRequests = visitRequestRepository;
        Schedules = scheduleRepository;
        ScheduleExceptions = scheduleExceptionRepository;
        Patients = patientRepository;
        UserManager = userManager;
        Visits = visitRepository;
        ScheduleDays = scheduleDays;
        Medicines = medicineRepository;
    }


    public async Task BeginTransactionAsync()
    {
        _transaction = await _db.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _db.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await RollbackAsync();
            throw new UoWTransactionException("Transaction failed. Rollback transaction.", ex);
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}