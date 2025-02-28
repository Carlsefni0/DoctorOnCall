namespace DoctorOnCall.ValidationAttributes.Schedule;

using System.ComponentModel.DataAnnotations;

using System.Collections;

public class NotEmptyAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ICollection collection)
        {
            if (collection.Count == 0)
            {
                return new ValidationResult("The list of days must not be empty.");
            }
        }
        else if (value == null)
        {
            return new ValidationResult("The list of days must not be null or empty.");
        }

        return ValidationResult.Success;
    }
}

