using System.Text.Json.Serialization;
using DoctorOnCall.Enums;


namespace DoctorOnCall.DTOs.VisitRequestDTOs;

public class VisitRequestSummaryDto
{
    public int Id { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VisitRequestStatus Status { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VisitRequestType Type { get; set; }
    
    public DateTime RequestedDateTime { get; set; }
    
    public bool IsRegularVisit { get; set; } = false;
}