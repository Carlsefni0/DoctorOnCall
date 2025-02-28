using System.ComponentModel.DataAnnotations;
using DoctorOnCall.DTOs.Schedule;

namespace DoctorOnCall.ValidationAttributes.Schedule;

public class WorkingHoursAttribute : ValidationAttribute
{
    private readonly TimeSpan _start;
    private readonly TimeSpan _end;

    public WorkingHoursAttribute(string start, string end)
    {
        _start = TimeSpan.Parse(start);
        _end = TimeSpan.Parse(end);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ScheduleDayDto dto)
        {
            if (dto.StartTime < _start || dto.EndTime > _end)
            {
                return new ValidationResult($"Time interval  must be within working hours ({_start} - {_end}).");
            }
        }

        return ValidationResult.Success;
    }
}