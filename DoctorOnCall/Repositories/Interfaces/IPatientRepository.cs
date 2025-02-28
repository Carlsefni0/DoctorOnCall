using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.Models;

namespace PatientOnCall.RepositoryInterfaces;

public interface IPatientRepository
{
    Task<Patient> CreatePatient(Patient patientData);
    Task<Patient> GetPatientById(int patientId);
    Task<PagedResult<PatientSummaryDto>> GetPagedPatients(PatientFilterDto filter);
    Task<Patient> GetPatientByUserId(int patientId);
}


