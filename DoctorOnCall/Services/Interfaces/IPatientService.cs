using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IPatientService
{
    Task<PatientDetailsDto> CreatePatient(CreatePatientDto patientData);
    Task<PatientDetailsDto> UpdatePatient(int userId, EditPatientDto patientData);
    Task<PagedResult<PatientSummaryDto>> GetPagedPatients(PatientFilterDto filter);
    Task<PatientDetailsDto> GetPatientById(int userId);
   
}