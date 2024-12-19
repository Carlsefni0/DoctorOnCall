using DoctorOnCall.Enums;

public class VisitRequestFilterDto
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public VisitRequestStatus? Status { get; set; }
}