using System.Text.Json.Serialization;
using DoctorOnCall.Enums;


namespace DoctorOnCall.DTOs.VisitRequestDTOs;

public class VisitRequestSummaryDto
{
    public int VisitRequestId { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VisitRequestStatus Status { get; set; }
    
    public DateTime RequestedDateTime { get; set; }
    
    public bool IsRegularVisit { get; set; } = false;
}