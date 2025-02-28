using System.ComponentModel.DataAnnotations;

namespace DoctorOnCall.ValidationAttributes.VisitRequest;

public class NotDefaultDateAttribute : ValidationAttribute
{
    public NotDefaultDateAttribute() : base("RequestedDateTime is required and cannot be the default date.") { }

    public override bool IsValid(object value)
    {
        return value != null && (DateTime)value != default(DateTime);
    }
}