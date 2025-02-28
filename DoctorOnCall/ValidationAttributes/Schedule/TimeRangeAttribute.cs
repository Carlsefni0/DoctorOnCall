using System.ComponentModel.DataAnnotations;
using DoctorOnCall.DTOs.Schedule;

namespace DoctorOnCall.ValidationAttributes.Schedule;

public class TimeRangeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ScheduleDayDto dto)
        {
            if (dto.StartTime >= dto.EndTime)
            {
                return new ValidationResult($"Invalid time interval for {dto.DayOfWeek}: StartTime must be earlier than EndTime.");
            }
        }

        return ValidationResult.Success;
    }
}