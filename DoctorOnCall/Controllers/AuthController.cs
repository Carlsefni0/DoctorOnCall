using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.AuthDTOs;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Services.Interfaces;
using DoctorOnCall.UtilClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Controllers;

[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AuthResult>> Login([FromBody]LoginDto loginData)
    {
        var result = await _authService.Authenticate(loginData);
        
        return Ok(result);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
    {
        var result = await _authService.SendPasswordResetLink(forgotPassword.Email);
        
        return Ok(result);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword, [FromHeader(Name = "X-Reset-Token")] string token)
    {
        var result = await _authService.ResetPassword(resetPassword, token);
        
        return Ok(result);
    }
}