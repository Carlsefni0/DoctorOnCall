using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DoctorOnCall.DTOs;
using DoctorOnCall.Models;
using DoctorOnCall.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DoctorOnCall.Services;

public class TokenService (IConfiguration config, UserManager<AppUser> userManager): ITokenService
{
    public async Task<string> CreateToken(AppUser user)
    {
        string tokenKey = config["Jwt:Key"] ?? throw new Exception("TokenKey is null");
        if (tokenKey.Length < 64) throw new Exception("TokenKey is too short");
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ID користувача
            new Claim(ClaimTypes.Email, user.Email ?? ""),            // Email
            new Claim(ClaimTypes.GivenName, user.FirstName ?? ""),    // Ім'я
            new Claim(ClaimTypes.Surname, user.LastName ?? "")        // Прізвище
        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // Термін дії токена
            SigningCredentials = credentials,
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"]
        };

        // Генеруємо та повертаємо токен
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        return token;
    }
}

