using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

public abstract class BaseController : ControllerBase
{
    protected int GetUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new AuthenticationException("User ID claim is missing or invalid.");
        }
        return userId;
    }

}
