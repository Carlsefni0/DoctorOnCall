using AutoMapper;
using DoctorOnCall.DTOs.Visit;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.Models;

namespace DoctorOnCall.AutoMappers;

public class VisitMappingProfile: Profile
{
    public VisitMappingProfile()
    {
        CreateMap<Visit, VisitSummaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.visitRequestId, opt => opt.MapFrom(src => src.VisitRequestId))
            .ForMember(dest => dest.ActualStartDateTime, opt => opt.MapFrom(src => src.ActualStartDateTime))
            .ForMember(dest => dest.ActualEndDateTime, opt => opt.MapFrom(src => src.ActualEndDateTime))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.CancellationReason, opt => opt.MapFrom(src => src.CancellationReason))
            .ForMember(dest => dest.DelayTime, opt => opt.MapFrom(src => src.DelayTime));

       
    }
}