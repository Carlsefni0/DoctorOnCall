using DoctorOnCall.DTOs;
using DoctorOnCall.Models;

namespace DoctorOnCall.RepositoryInterfaces;

public interface ITokenService
{
    public Task<string> CreateToken(AppUser user);
}