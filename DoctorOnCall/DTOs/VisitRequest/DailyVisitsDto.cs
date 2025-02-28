using NetTopologySuite.Geometries;

namespace DoctorOnCall.DTOs.VisitRequest;

public class DailyVisitsDto
{
    public CoordsDto HospitalCoords { get; set; }
    public ICollection<CurrentVisitRequestDto> CurrentVisitRequests { get; set; }
}