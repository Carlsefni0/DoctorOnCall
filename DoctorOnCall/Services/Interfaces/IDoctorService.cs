using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;

namespace DoctorOnCall.ServiceInterfaces;

public interface IDoctorService
{
    Task<DoctorDetailsDto> CreateDoctor(CreateDoctorDto doctor);
    Task<PagedResult<DoctorSummaryDto>> GetPagedDoctors(DoctorFilterDto filter);
    Task<ICollection<DoctorSummaryDto>> GetDoctorsForVisitRequest(int visitRequestId, string mode);
    Task<DoctorSummaryDto> GetDoctorAssignedToVisitRequest(int visitRequestId);
    Task<DoctorDetailsDto> GetDoctorById (int doctorId);
    Task<DoctorDetailsDto> UpdateDoctor(int userId, EditDoctorDto doctorData);
    
    

}