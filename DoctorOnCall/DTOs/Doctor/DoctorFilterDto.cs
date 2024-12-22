using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.DTOs;

public class DoctorFilterDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Specialization { get; set; }
    public string? WorkingDistrict { get; set; }
    public DoctorStatus? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}