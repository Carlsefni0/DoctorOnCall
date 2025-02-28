using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.DTOs;

public class DoctorFilterDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<string>? Specializations { get; set; }
    public ICollection<string>? Districts { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
