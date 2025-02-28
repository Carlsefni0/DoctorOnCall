using System.Text.Json.Serialization;
using DoctorOnCall.Enums;

namespace DoctorOnCall.DTOs.VisitRequest;

public class AssignedVisitRequestDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VisitRequestType VisitRequestType { get; set; }
    public int VisitRequestId { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public string Address { get; set; }
}