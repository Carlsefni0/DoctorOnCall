namespace DoctorOnCall.DTOs.VisitRequest;

public class CurrentVisitRequestDto
{
    public int Id { get; set; }
    public string PatientFirstName { get; set; }
    public string PatientLastName { get; set; }
    public string PatientEmail { get; set; }
    public string VisitAddress { get; set; }
    public CoordsDto VisitCoords{ get; set; }
    
}