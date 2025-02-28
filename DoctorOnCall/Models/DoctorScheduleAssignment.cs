using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorOnCall.Models;

public class DoctorScheduleAssignment
{
    public int Id { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [ForeignKey(nameof(DoctorId))]
    public Doctor Doctor { get; set; }

    [Required]
    public int ScheduleTypeId { get; set; }

    [ForeignKey(nameof(ScheduleTypeId))]
    public ScheduleType ScheduleType { get; set; }
}