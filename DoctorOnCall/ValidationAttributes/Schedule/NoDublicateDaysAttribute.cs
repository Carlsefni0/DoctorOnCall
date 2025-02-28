using DoctorOnCall.DTOs.Schedule;

namespace DoctorOnCall.ValidationAttributes.Schedule;

using System.ComponentModel.DataAnnotations;
using System.Linq;

public class NoDuplicateDaysAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ICollection<ScheduleDayDto> scheduleDays)
        {
            var duplicates = scheduleDays
                .GroupBy(d => new { d.DayOfWeek, d.StartTime, d.EndTime })
                .Where(g => g.Count() > 1);

            if (duplicates.Any())
            {
                return new ValidationResult("The list of schedule days contains duplicates.");
            }
        }

        return ValidationResult.Success;
    }
}
