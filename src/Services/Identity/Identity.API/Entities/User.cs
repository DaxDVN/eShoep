using Microsoft.AspNetCore.Identity;

namespace Identity.API.Entities;

public class User : IdentityUser
{
    public string? Initials { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}