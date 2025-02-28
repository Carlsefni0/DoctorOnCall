using System.ComponentModel.DataAnnotations;
using DoctorOnCall.DTOs.Analytics;

namespace DoctorOnCall.ValidationAttributes;

public class DateRangeAttribute : ValidationAttribute
{
    private readonly string _startDateProperty;

    public DateRangeAttribute(string startDateProperty, string errorMessage = null)
    {
        _startDateProperty = startDateProperty;
        if (!string.IsNullOrEmpty(errorMessage))
        {
            ErrorMessage = errorMessage;
        }
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var endDate = value as DateTime?;
        if (!endDate.HasValue)
        {
            return ValidationResult.Success;
        }

        var startDateProperty = validationContext.ObjectType.GetProperty(_startDateProperty);
        if (startDateProperty == null)
        {
            return new ValidationResult($"Unknown property: {_startDateProperty}");
        }

        var startDate = startDateProperty.GetValue(validationContext.ObjectInstance) as DateTime?;
        if (startDate.HasValue && endDate < startDate)
        {
            var errorMessage = ErrorMessage ?? "EndDate cannot be earlier than StartDate.";
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}

