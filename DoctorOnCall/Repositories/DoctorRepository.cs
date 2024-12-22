using DoctorOnCall.DTOs;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly DataContext _db;
    public DoctorRepository(DataContext db) => _db = db;
    
    public async Task<ICollection<Doctor>> GetDoctors(DoctorFilterDto filter)
    {
        var query = _db.Doctors.AsQueryable();
        
        if (!string.IsNullOrEmpty(filter.FirstName))
        {
            query = query.Where(p => p.User.FirstName.Contains(filter.FirstName));
        }

        if (!string.IsNullOrEmpty(filter.LastName))
        {
            query = query.Where(p => p.User.LastName.Contains(filter.LastName));
        }

        if (!string.IsNullOrEmpty(filter.Specialization))
        {
            query = query.Where(d => d.Specialization == filter.Specialization);
        }

        if (!string.IsNullOrEmpty(filter.WorkingDistrict))
        {
            query = query.Where(d => d.WorkingDistrict == filter.WorkingDistrict);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(d => d.Status == filter.Status.Value);
        }

        return await query
            .Include("User")
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();
    }
    
    public async Task<Doctor?> GetDoctorByUserId(int userId)
    {
        return await _db.Doctors
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.UserId == userId);
    }

    public async Task<Doctor> CreateDoctor(Doctor doctor)
    {
        var createdDoctor = await _db.Doctors.AddAsync(doctor);
        
        await _db.SaveChangesAsync();
        
        return createdDoctor.Entity;
    }

    public async Task<Doctor> UpdateDoctor(Doctor doctorData)
    {
        var updatedDoctor =  _db.Doctors.Update(doctorData);
        await _db.SaveChangesAsync();
        
        return doctorData;
    }
    
}