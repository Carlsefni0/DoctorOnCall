using AutoMapper;
using DoctorOnCall.DTOs.Vacation;

namespace DoctorOnCall.AutoMappers;

public class ScheduleExceptionMappingProfile: Profile
{
    public ScheduleExceptionMappingProfile()
    {
        CreateMap<ScheduleException, ScheduleExceptionSummaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ExceptionStatus))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ExceptionType))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Doctor.User.FirstName + " " + src.Doctor.User.LastName));



        CreateMap<ScheduleException, ScheduleExceptionDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ExceptionStatus))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.ExceptionType))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDateTime))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDateTime))
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
            .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
            .ForMember(dest => dest.DoctorEmail, opt => opt.MapFrom(src => src.Doctor.User.Email))
            .ForMember(dest => dest.DoctorFirstName, opt => opt.MapFrom(src => src.Doctor.User.FirstName))
            .ForMember(dest => dest.DoctorLastName, opt => opt.MapFrom(src => src.Doctor.User.LastName));
    }
}