using System.Security.Authentication;
using DoctorOnCall.DTOs.AuthDTOs;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Services.Interfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;


namespace DoctorOnCall.Services;

public class AuthService: IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<AppUser> userManager,
        IEmailService emailService,
        ITokenService tokenService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _emailService = emailService;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<AuthSuccess> Authenticate(LoginDto loginData)
    {
        var user = await _userManager.FindByEmailAsync(loginData.Email);
        
        if (user == null) throw new NotFoundException("User with such email doesn't exist");
        
        var result = await _userManager.CheckPasswordAsync(user, loginData.Password);
        
        if(!result) throw new InvalidCredentialException("Incorrect password");
        
        var token = await _tokenService.CreateToken(user);

        var success = new AuthSuccess() { Token = token };

        return success;
    }

    //TODO: Changer return type to smth better
    public async Task<string> SendPasswordResetLink(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null) throw new NotFoundException("User with such email doesn't exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var baseUrl = _configuration["AppSettings:BaseUrl"];
        
        var resetLink = $"{baseUrl}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

         
         await _emailService.SendEmailAsync(
             user.Email,
             "Password Reset",
             $"Click <a href='{resetLink}'>here</a> to reset your password.");

        return user.Email;
    }
    //TODO: Change return type to smth better

    public async Task<string> ResetPassword(ResetPasswordDto resetPasswordDto, string token)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        
        if(resetPasswordDto.ConfirmPassword != resetPasswordDto.NewPassword)
            throw new AuthenticationException("Passwords do not match");

        var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);

        return "Success";
    }
}