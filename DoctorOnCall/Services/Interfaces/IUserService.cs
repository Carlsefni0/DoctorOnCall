using DoctorOnCall.DTOs;
using DoctorOnCall.Enums;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface IUserService
{
    Task<AppUser> CreateUser(CreateUserDto user, UserRole role);
    Task DeleteUser(int userId);

}