using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

public class ScheduleException
{
    public int Id { get; set; }

    [Required]
    public int DoctorId { get; set; }

    [ForeignKey(nameof(DoctorId))]
    public Doctor Doctor { get; set; }

    [Required]
    public DateTime StartDateTime { get; set; }

    [Required]
    public DateTime EndDateTime { get; set; }

    [Required]
    public ScheduleExceptionType ExceptionType { get; set; }

    [Required]
    public ScheduleExceptionStatus ExceptionStatus { get; set; } = ScheduleExceptionStatus.Pending;

    [MaxLength(200)]
    public string Reason { get; set; }
}