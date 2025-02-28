using System.ComponentModel.DataAnnotations;
using DoctorOnCall.DTOs.Schedule;

namespace DoctorOnCall.ValidationAttributes.Schedule;

public class MaxUniqueDaysAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ICollection<ScheduleDayDto> scheduleDays)
        {
            if (scheduleDays.Select(d => d.DayOfWeek).Distinct().Count() > 7)
            {
                return new ValidationResult("The schedule cannot contain more than 7 unique days.");
            }
        }

        return ValidationResult.Success;
    }
}
