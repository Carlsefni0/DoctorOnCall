using DoctorOnCall.Models;

namespace DoctorOnCall.UtilClasses;

public class AuthResult {
    public bool Succeeded { get; private set; }
    public string AccessToken { get; private set; }
    public string ErrorMessage { get; private set; }

    public string UserRole { get; private set; }
    public static AuthResult Success(string token)
    {
        return new AuthResult
        {
            Succeeded = true,
            AccessToken = token,
        };
    }

    public static AuthResult Failure(string errorMessage)
    {
        return new AuthResult
        {
            Succeeded = false,
            ErrorMessage = errorMessage
        };
    }
}