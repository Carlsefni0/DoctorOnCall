using DoctorOnCall.DTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IDoctorRepository
{
    Task<Doctor> CreateDoctor(Doctor doctor);
    Task<Doctor> UpdateDoctor(Doctor doctorData);
   
    Task<ICollection<Doctor>> GetDoctors(DoctorFilterDto filter);
    Task<Doctor?> GetDoctorByUserId(int userId);

    
   

}