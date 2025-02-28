
using System.Text.Json.Serialization;
using DoctorOnCall.DTOs.VisitRequest;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using NetTopologySuite.Geometries;

namespace DoctorOnCall.DTOs.VisitRequestDTOs;

public class VisitRequestDetailsDto
{
    public int Id { get; set; }
    
    public int PatientId { get; set; }
    public int VisitId { get; set; }
    public string PatientFullName { get; set; }
    public string PatientEmail { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VisitRequestStatus Status { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VisitRequestType RequestType { get; set; }
    public DateTime RequestedDateTime { get; set; }
    public string? RequestDescription { get; set; }
    public bool IsRegularVisit { get; set; }
    public string VisitAddress { get; set; }
    public CoordsDto VisitCoords { get; set; }
    public string District { get; set; }
    public string? RejectionReason { get; set; }
    public int Interval { get; set; }
    public int Occurrences { get; set; }
    public ICollection<RegularVisitDateDto>? RegularVisitDates { get; set; }
}