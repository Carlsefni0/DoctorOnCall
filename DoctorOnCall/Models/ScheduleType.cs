using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorOnCall.Models;

//TODO: Maybe I should use name as identifier
public class ScheduleType
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    public ICollection<DoctorScheduleAssignment> DoctorScheduleAssignments { get; set; } = new List<DoctorScheduleAssignment>();
    public ICollection<ScheduleDayMapping> ScheduleDayMappings { get; set; } = new List<ScheduleDayMapping>(); 
}