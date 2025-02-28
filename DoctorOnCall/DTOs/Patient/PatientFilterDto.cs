using DoctorOnCall.Enums;

namespace DoctorOnCall.DTOs;

public class PatientFilterDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public ICollection<string>? Districts { get; set; }
    public int? BirthYear { get; set; }
    public int? BirthMonth { get; set; }
    public Gender? Gender { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}