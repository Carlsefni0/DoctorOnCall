using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.AutoMappers;

public class MedicineMappingProfile: Profile
{
    public MedicineMappingProfile()
    {
        CreateMap<Medicine, MedicineDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
    }
}