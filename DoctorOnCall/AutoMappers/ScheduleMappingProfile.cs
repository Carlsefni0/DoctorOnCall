using AutoMapper;
using DoctorOnCall.DTOs.Schedule;
using DoctorOnCall.Models;

namespace DoctorOnCall.AutoMappers;

public class ScheduleMappingProfile: Profile
{
    public ScheduleMappingProfile()
    {
        CreateMap<ScheduleType, ScheduleSummaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ScheduleName, opt => opt.MapFrom(src => src.Name));
        
        CreateMap<ScheduleType, ScheduleDetailsDto>()
            .ForMember(dest=> dest.ScheduleId, opt=> opt.MapFrom(src=> src.Id))
            .ForMember(dest => dest.ScheduleName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ScheduleDays, opt => opt.MapFrom(src => src.ScheduleDayMappings.Select(mapping => mapping.ScheduleDay)));
        
        CreateMap<ScheduleDay, ScheduleDayDto>()
            .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));
    }
}