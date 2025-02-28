using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using NetTopologySuite.Geometries;

public class Doctor
{
    public int Id { get; set; }
    
    [Required]
    public string Specialization { get; set; }
    
    [Required]
    public string WorkingDistrict { get; set; }
    
    [Required]
    public string Hospital { get; set; }
    
    [Required]
    public Point Location { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual AppUser User { get; set; }
    public virtual ICollection<DoctorScheduleAssignment> ScheduleAssignments { get; set; } = new List<DoctorScheduleAssignment>();
    public virtual ICollection<DoctorVisitRequest> DoctorVisitRequests { get; set; } = new List<DoctorVisitRequest>();

    public virtual ICollection<ScheduleException> Exceptions { get; set; } = new List<ScheduleException>();
}