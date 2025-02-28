using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.ValidationAttributes.VisitRequest;

public class NotInPastOrNowAttribute : ValidationAttribute
{
    public NotInPastOrNowAttribute() : base("Requested visit date cannot be in the past or now.") { }

    public override bool IsValid(object value)
    {
        if (value is DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow;
        }
        return false;
    }
}