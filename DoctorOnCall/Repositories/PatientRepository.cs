using DoctorOnCall.Models;
using PatientOnCall.RepositoryInterfaces;

namespace DoctorOnCall.Repositories;

public class PatientRepository: IPatientRepository
{
    public Task<PatientModel> AddPatient(PatientModel Patient)
    {
        throw new NotImplementedException();
    }

    public Task<PatientModel> UpdatePatient(PatientModel Patient)
    {
        throw new NotImplementedException();
    }

    public Task<PatientModel> DeletePatient(int id)
    {
        throw new NotImplementedException();
    }

    public Task<PatientModel> GetPatientById(int id)
    {
        throw new NotImplementedException();
    }

   
    public Task<List<PatientModel>> GetAllPatients()
    {
        throw new NotImplementedException();
    }
    
}