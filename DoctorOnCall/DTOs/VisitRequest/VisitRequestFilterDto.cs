using DoctorOnCall.Enums;

public class VisitRequestFilterDto
{
    public int? Year { get; set; }
    public int? Month { get; set; }
    public ICollection<VisitRequestStatus>? Statuses { get; set; }
    public ICollection<VisitRequestType>? Types { get; set; }
    public bool? IsRegular { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
    public DateTime? DateRangeStart { get; set; }
    public DateTime? DateRangeEnd{ get; set; }
    public DateTime? Date { get; set; }
    public bool isSchedule { get; set; } = true;

}