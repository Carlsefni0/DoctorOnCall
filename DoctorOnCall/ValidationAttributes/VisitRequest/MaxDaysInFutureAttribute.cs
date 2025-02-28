using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.ValidationAttributes.VisitRequest;

public class MaxDaysInFutureAttribute : ValidationAttribute
{
    private readonly int _maxDays;

    public MaxDaysInFutureAttribute(int maxDays) 
        : base($"Requested visit date cannot be more than {maxDays} days in the future.") 
    {
        _maxDays = maxDays;
    }

    public override bool IsValid(object value)
    {
        if (value is DateTime dateTime)
        {
            return dateTime <= DateTime.UtcNow.AddDays(_maxDays);
        }
        return false;
    }
}