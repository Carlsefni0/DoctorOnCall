using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;

namespace DoctorOnCall.ServiceInterfaces;

public interface IDoctorService
{
    public Task<DoctorDetailsDto> CreateDoctor(CreateDoctorDto doctor);
    public Task<ICollection<DoctorSummaryDto>> GetDoctors(DoctorFilterDto filter);
    public Task<DoctorDetailsDto> GetDoctorByUserId(int userId);

    public Task<DoctorDetailsDto> UpdateDoctor(int userId, CreateDoctorDto doctorData);
    //
    // public Task<bool> DeleteDoctor(int doctorId);
    //
    

}