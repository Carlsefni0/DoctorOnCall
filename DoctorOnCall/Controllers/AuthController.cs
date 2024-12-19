using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.AuthDTOs;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.UtilClasses;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }


    [HttpPost("login")]
    public async Task<ActionResult<AuthResult>> Login([FromBody]LoginDto loginData)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var authResult = await authService.Authenticate(loginData);
        
        return Ok(authResult);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await authService.SendPasswordResetLink(forgotPassword.Email);
        
        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword, [FromHeader(Name = "X-Reset-Token")] string token)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var resetResult = await authService.ResetPassword(resetPassword, token);
        
        return Ok(resetResult);
    }
    
    

    
}