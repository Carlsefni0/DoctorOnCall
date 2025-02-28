namespace DoctorOnCall.ValidationAttributes;

using System;
using System.ComponentModel.DataAnnotations;
public class NotFutureDateAttribute : ValidationAttribute
{
    public NotFutureDateAttribute(string errorMessage = "The date cannot be in the future.")
    {
        ErrorMessage = errorMessage;
    }

    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            return date <= DateTime.UtcNow;
        }
        return true;
    }
}

