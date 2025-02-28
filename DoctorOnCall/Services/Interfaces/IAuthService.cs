using DoctorOnCall.DTOs;
using DoctorOnCall.DTOs.AuthDTOs;
using DoctorOnCall.UtilClasses;
using Microsoft.AspNetCore.Mvc;

namespace DoctorOnCall.Services.Interfaces;

public interface IAuthService
{
    public Task<AuthSuccess> Authenticate(LoginDto loginData);
    public Task<string> SendPasswordResetLink(string email);
    
    public Task<string> ResetPassword(ResetPasswordDto resetPasswordDto, string token);
}
