using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IDoctorRepository
{
    Task<Doctor> CreateDoctor(Doctor doctor);
    Task<Doctor> GetDoctorById(int doctorId, params string[] includes);
    Task<Doctor> GetDoctorByUserId(int userId);
    Task<DoctorSummaryDto> GetDoctorAssignedToVisitRequest(int visitRequestId);
    Task<PagedResult<DoctorSummaryDto>> GetPagedDoctors(DoctorFilterDto filter);
    Task<ICollection<Doctor>> GetAllDoctors(DoctorFilterDto filter);

}