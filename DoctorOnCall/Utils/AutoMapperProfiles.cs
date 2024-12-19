using AutoMapper;
using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.ResponseDto;
using DoctorOnCall.DTOs.VisitRequestDTOs;
using DoctorOnCall.Models;
using Microsoft.OpenApi.Extensions;

namespace DoctorOnCall.Utils;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Doctor, ResponseDoctorBadgeDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization));

        CreateMap<Doctor, ResponseDoctorDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
            .ForMember(dest => dest.WorkingDistrict, opt => opt.MapFrom(src => src.WorkingDistrict))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender))
        .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.User.UserRoles));


        CreateMap<VisitRequest, VisitRequestSummaryDto>()
            .ForMember(dest => dest.VisitRequestId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RequestedDateTime, opt => opt.MapFrom(src => src.RequestedDateTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.IsRegularVisit, opt => opt.MapFrom(src => src.IsRegularVisit));

        CreateMap<VisitRequest, VisitRequestDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.RequestedDateTime, opt => opt.MapFrom(src => src.RequestedDateTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.IsRegularVisit, opt => opt.MapFrom(src => src.IsRegularVisit))
            .ForMember(dest => dest.RequestDescription, opt => opt.MapFrom(src => src.RequestDescription))
            .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
            .ForMember(dest => dest.VisitAddress, opt => opt.MapFrom(src => src.VisitAddress))
            .ForMember(dest => dest.RejectionReason, opt => opt.MapFrom(src => src.RejectionReason))
            .ForMember(
                dest => dest.RequestedMedicines,
                opt => opt.MapFrom(src => src.RequestedMedicines.Select(rm => new MedicineDto
                {
                    Id = rm.Medicine.Id,
                    Name = rm.Medicine.Name,
                    Description = rm.Medicine.Description,
                    UnitPrice = rm.Medicine.UnitPrice,
                    Dosage = rm.Medicine.Dosage,
                    Quantity = rm.Quantity
                }))
            );
        
        // CreateMap<Medicine, MedicineDto>()
        //     .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        //     .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        //     .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
        //     .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
        //     .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
        //     .ForMember(dest=> dest.Quantity,opt=>opt.MapFrom(src=>src.))

    }
    
}