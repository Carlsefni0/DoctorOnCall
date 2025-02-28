using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.Repository.Interfaces;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;



namespace DoctorOnCall.Services.Implementations;

public class MedicineService: IMedicineService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public MedicineService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        
        _unitOfWork = unitOfWork;
    }


    public async Task<ICollection<RequestedMedicineDto>> GetMedicinesByVisitRequestId(int visitRequestId, int userId)
    {
        var user  = await _unitOfWork.UserManager.FindByIdAsync(userId.ToString());
        
        if(user == null) throw new NotFoundException($"User with ID {userId} not found");
        
        var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);
        
        var visitRequest = await _unitOfWork.VisitRequests.GetVisitRequestById(visitRequestId);

        if (userRoles.Contains("Doctor"))
        {
            var doctor = await _unitOfWork.Doctors.GetDoctorByUserId(userId);
            
            var isDoctorAssigned = visitRequest.DoctorVisitRequests.Any(dvr => dvr.DoctorId == doctor.Id);
            
           if(!isDoctorAssigned) throw new ForbiddenAccessException("You are not allowed to get this information");
        }
        
        if (userRoles.Contains("Patient"))
        {
            var patient = await _unitOfWork.Patients.GetPatientByUserId(userId);
            
            if(visitRequest.PatientId != patient.Id) throw new ForbiddenAccessException("You are not allowed get this information");
        }
        
        
        var medicines = await _unitOfWork.Medicines.GetMedicinesByVisitRequestId(visitRequestId);
        
        return medicines;
    }

    public async Task<ICollection<MedicineDto>> GetMedicines()
    {
        var medicines = await _unitOfWork.Medicines.GetMedicines();
        
        var mappedMedicines = _mapper.Map<ICollection<MedicineDto>>(medicines);
        
        return mappedMedicines;
    }
    
    public async Task<ICollection<MedicineDto>> FindMedicinesByName(string medicineName)
    {
        var medicines = await _unitOfWork.Medicines.SearchMedicinesByName(medicineName);
        
        var mappedMedicines = _mapper.Map<ICollection<MedicineDto>>(medicines);
        
        return mappedMedicines;
    }
}