using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IDoctorRepository
{
    Task<DoctorModel> AddDoctor(DoctorModel doctor);
    Task<DoctorModel> UpdateDoctor(DoctorModel doctor);
    
    Task<DoctorModel> DeleteDoctor(int id);
    Task<DoctorModel> GetDoctorById(int id);
    
    Task<List<DoctorModel>> GetAllDoctors();

}