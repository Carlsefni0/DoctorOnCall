using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.AutoMappers;

public class VisitRequestMappingProfile: Profile
{
    public VisitRequestMappingProfile()
    {
        CreateMap<VisitRequest, VisitRequestDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.Patient.User.FirstName + " " + src.Patient.User.LastName))
            .ForMember(dest => dest.PatientEmail, opt => opt.MapFrom(src => src.Patient.User.Email))
            .ForMember(dest => dest.RequestedDateTime, opt => opt.MapFrom(src => src.RequestedDateTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.RequestType, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.IsRegularVisit, opt => opt.MapFrom(src => src.IsRegularVisit))
            .ForMember(dest => dest.RequestDescription, opt => opt.MapFrom(src => src.RequestDescription))
            .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
            .ForMember(dest => dest.VisitAddress, opt => opt.MapFrom(src => src.VisitAddress))
            .ForMember(dest => dest.RejectionReason, opt => opt.MapFrom(src => src.RejectionReason))
            .ForMember(dest => dest.RegularVisitDates, opt => opt.MapFrom(src => src.RegularVisitDates))
            .ForMember(dest => dest.Interval, opt => opt.MapFrom(src => src.RegularVisitIntervalDays))
            .ForMember(dest => dest.Occurrences, opt => opt.MapFrom(src => src.RegularVisitOccurrences))
            .ForMember(dest => dest.RegularVisitDates, opt => opt.MapFrom(src => src.RegularVisitDates))
            .ForMember(
                dest => dest.VisitCoords,
                opt => opt.MapFrom(src => new CoordsDto()
                {
                    Lat = src.Patient.Location.X, Lng = src.Patient.Location.Y
                })
            );
        CreateMap<RegularVisitDate, RegularVisitDateDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.VisitStartDateTime, opt => opt.MapFrom(src => src.VisitStartDateTime))
            .ForMember(dest => dest.VisitEndDateTime, opt => opt.MapFrom(src => src.VisitEndDateTime))
            .ForMember(dest => dest.IsReported, opt => opt.MapFrom(src => src.IsReported));
        
        
        CreateMap<VisitRequest, VisitRequestSummaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RequestedDateTime, opt => opt.MapFrom(src => src.RequestedDateTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.IsRegularVisit, opt => opt.MapFrom(src => src.IsRegularVisit));
        
        CreateMap<VisitRequest, AssignedVisitRequestDto>()
            .ForMember(dest => dest.VisitRequestId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.VisitRequestType, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.RequestedDateTime))
            .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.ExpectedEndDateTime))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.VisitAddress));
        
        CreateMap<VisitRequest, CurrentVisitRequestDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PatientFirstName, opt => opt.MapFrom(src => src.Patient.User.FirstName))
            .ForMember(dest => dest.PatientLastName, opt => opt.MapFrom(src => src.Patient.User.LastName))
            .ForMember(dest => dest.VisitAddress, opt => opt.MapFrom(src => src.VisitAddress))
            .ForMember(dest => dest.PatientEmail, opt => opt.MapFrom(src => src.Patient.User.Email))
            .ForMember(
                dest => dest.VisitCoords,
                opt => opt.MapFrom(src => new CoordsDto()
                {
                    Lat = src.Patient.Location.X, Lng = src.Patient.Location.Y
                })
            );
        
        CreateMap<RegularVisitDate, CurrentVisitRequestDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PatientFirstName, opt => opt.MapFrom(src => src.VisitRequest.Patient.User.FirstName))
            .ForMember(dest => dest.PatientLastName, opt => opt.MapFrom(src => src.VisitRequest.Patient.User.LastName))
            .ForMember(dest => dest.VisitAddress, opt => opt.MapFrom(src => src.VisitRequest.VisitAddress))
            .ForMember(dest => dest.PatientEmail, opt => opt.MapFrom(src => src.VisitRequest.Patient.User.Email))
            .ForMember(
                dest => dest.VisitCoords,
                opt => opt.MapFrom(src => new CoordsDto()
                {
                    Lat = src.VisitRequest.Patient.Location.X, Lng = src.VisitRequest.Patient.Location.Y
                })
            );
        
        CreateMap<RegularVisitDate, AssignedVisitRequestDto>()
            .ForMember(dest => dest.VisitRequestId, opt => opt.MapFrom(src => src.VisitRequestId))
            .ForMember(dest => dest.VisitRequestType, opt => opt.MapFrom(src => src.VisitRequest.Type))
            .ForMember(dest => dest.StartDateTime, opt => opt.MapFrom(src => src.VisitStartDateTime))
            .ForMember(dest => dest.EndDateTime, opt => opt.MapFrom(src => src.VisitEndDateTime))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.VisitRequest.VisitAddress));
    }
}