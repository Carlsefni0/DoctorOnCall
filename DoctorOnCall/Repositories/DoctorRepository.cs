using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DoctorOnCall.Repositories;

public class DoctorRepository: IDoctorRepository
{
    private readonly DbContext _db;

    DoctorRepository(DbContext context)
    {
        _db = context;
    }
    public Task<DoctorModel> AddDoctor(DoctorModel doctor)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorModel> UpdateDoctor(DoctorModel doctor)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorModel> DeleteDoctor(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorModel> GetDoctorById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<DoctorModel>> GetAllDoctors()
    {
        throw new NotImplementedException();
    }
}