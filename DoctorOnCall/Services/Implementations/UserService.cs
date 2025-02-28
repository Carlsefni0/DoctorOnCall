using System.ComponentModel.DataAnnotations;
using DoctorOnCall.DTOs;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using DoctorOnCall.Utils;
using Microsoft.AspNetCore.Identity;

namespace DoctorOnCall.Services;

//TODO: add the method for soft delete
public class UserService: IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<AppUser> CreateUser(CreateUserDto userData, UserRole role)
    {
        var userExists = await _userManager.FindByEmailAsync(userData.Email);
    
        if (userExists != null)
        {
            throw new ValidationErrorsException(new Dictionary<string, List<string>>
            {
                { "Email", new List<string> { "User with this email already exists." } }
            });
        }

        var user = new AppUser
        {
            Email = userData.Email,
            FirstName = userData.FirstName,
            LastName = userData.LastName,
            Gender = userData.Gender,
            PhoneNumber = userData.PhoneNumber,
            UserName = userData.Email,
        };

        var result = await _userManager.CreateAsync(user, userData.Password);

        if (!result.Succeeded)
        {
            throw new ApplicationException("Failed to create user");
        }

        await _userManager.AddToRoleAsync(user, role.ToString());

        return await _userManager.FindByEmailAsync(userData.Email);
    }
    
    public async Task DeleteUser(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null) throw new NotFoundException($"User with ID {userId} was not found");
        
        await _userManager.DeleteAsync(user);
    }


    
}