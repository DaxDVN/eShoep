using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Services;

public class TokenService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public TokenService(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? "unknown"),
            new Claim("FirstName", user.FirstName ?? "unknown"),
            new Claim("LastName", user.LastName ?? "unknown"),
            new Claim("Address", user.Address ?? "unknown"),
            new Claim("PhoneNumber", user.PhoneNumber ?? "")
        };

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Any())
        {
            await _userManager.AddToRoleAsync(user, "Customer");
            roles.Add("Customer");
        }

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "_7imDaxinDev"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);


        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}