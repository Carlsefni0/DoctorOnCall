using DoctorOnCall.Models;

namespace PatientOnCall.RepositoryInterfaces;

public interface IPatientRepository
{
    Task<PatientModel> AddPatient(PatientModel Patient);
    Task<PatientModel> UpdatePatient(PatientModel Patient);
    
    Task<PatientModel> DeletePatient(int id);
    Task<PatientModel> GetPatientById(int id);
    
    
    Task<List<PatientModel>> GetAllPatients();
}
