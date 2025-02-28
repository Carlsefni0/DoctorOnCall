using AutoMapper;
using DoctorOnCall.DTOs;

namespace DoctorOnCall.AutoMappers;

public class DoctorMappingProfile: Profile
{
    public DoctorMappingProfile()
    {
        CreateMap<Doctor, DoctorDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
            .ForMember(dest => dest.WorkingDistrict, opt => opt.MapFrom(src => src.WorkingDistrict))
            .ForMember(dest => dest.Hospital, opt => opt.MapFrom(src => src.Hospital))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender));

        CreateMap<Doctor, DoctorSummaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName))
            .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
            .ForMember(dest => dest.WorkingDistrict, opt => opt.MapFrom(src => src.WorkingDistrict));
    }
}